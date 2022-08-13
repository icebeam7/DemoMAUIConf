using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace DemoMAUIConf.ViewModels
{
    public partial class ContactViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Contact contact;

        [ObservableProperty]
        private string contactPhone;

        [ObservableProperty]
        private string contactEmail;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private string subject;

        [ObservableProperty]
        private string document;

        [ObservableProperty]
        private ImageSource image;

        private IContacts contacts;
        private IPhoneDialer phoneDialer;
        private ISms sms;
        private IEmail email;
        private IClipboard clipboard;
        private IFilePicker filePicker;
        private ITextToSpeech textToSpeech;
        private IShare share;
        private ILauncher launcher;
        private IScreenshot screenshot;
        private IMediaPicker mediaPicker;
        private IFileSystem fileSystem;

        public ContactViewModel(IContacts contacts,
            IPhoneDialer phoneDialer,
            ISms sms,
            IEmail email,
            IClipboard clipboard,
            IFilePicker filePicker,
            ITextToSpeech textToSpeech,
            IShare share,
            ILauncher launcher, 
            IMediaPicker mediaPicker,
            IFileSystem fileSystem,
            IScreenshot screenshot)
        {
            contact = new Contact();

            this.contacts = contacts;
            this.phoneDialer = phoneDialer;
            this.sms = sms;
            this.email = email;
            this.clipboard = clipboard;
            this.filePicker = filePicker;
            this.textToSpeech = textToSpeech;
            this.share = share;
            this.launcher = launcher;
            this.mediaPicker = mediaPicker;
            this.fileSystem = fileSystem;
            this.screenshot = screenshot;
        }

        public async Task<PermissionStatus> CheckAndRequestContactsPermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            status = await Permissions.RequestAsync<Permissions.ContactsRead>();
            return status;
        }

        [RelayCommand]
        private async Task SelectContact()
        {
            try
            {
                var permission = await CheckAndRequestContactsPermission();

                if (permission == PermissionStatus.Granted)
                {
                    var selectedContact = await contacts.PickContactAsync();

                    if (selectedContact == null)
                        return;

                    Contact = selectedContact;
                    ContactPhone = Contact.Phones?.FirstOrDefault()?.PhoneNumber;
                    ContactEmail = Contact.Emails?.FirstOrDefault()?.EmailAddress;
                }
            }
            catch (Exception ex)
            {

            }
        }

        [RelayCommand]
        private async Task Call()
        {
            try
            {
                phoneDialer.Open(ContactPhone);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Info", "No call. There was an error.", "OK");
            }
        }

        [RelayCommand]
        private async Task SendSMS()
        {
            try
            {
                var numbers = new List<string>() { ContactPhone };

                var smsMessage = new SmsMessage(Message, numbers);
                await sms.ComposeAsync(smsMessage);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Info", "No message. There was an error.", "OK");
            }
        }

        [RelayCommand]
        private async Task SendEmail()
        {
            try
            {
                var emailAddresses = new List<string>() { ContactEmail };

                var message = new EmailMessage
                {
                    Subject = Subject,
                    Body = Message,
                    To = emailAddresses
                };

                if (!string.IsNullOrWhiteSpace(Document))
                    message.Attachments.Add(new EmailAttachment(Document));

                await email.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Info", "No email. There was an error.", "OK");
            }
        }

        [RelayCommand]
        private async Task CopyText()
        {
            await clipboard.SetTextAsync(Message);
        }

        [RelayCommand]
        private async Task PasteText()
        {
            if (clipboard.HasText)
                Message = await clipboard.GetTextAsync();
        }

        [RelayCommand]
        private async Task<FileResult> SelectFile()
        {
            try
            {
                var options = new PickOptions
                {
                    PickerTitle = "Please select a file"
                };

                var result = await filePicker.PickAsync(options);

                if (result != null)
                {
                    Document = result.FullPath;

                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        var stream = await result.OpenReadAsync();
                        Image = ImageSource.FromStream(() => stream);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return null;
        }

        [RelayCommand]
        private async Task ReadMessageAloud()
        {
            if (!string.IsNullOrWhiteSpace(Message))
            {
                var settings = new SpeechOptions()
                {
                    Volume = 0.75f,
                    Pitch = 1.0f
                };

                await textToSpeech.SpeakAsync(Message, settings);
            }
        }

        [RelayCommand]
        private async Task ShareText()
        {
            await share.RequestAsync(new ShareTextRequest
            {
                Title = "Information",
                Text = Message,
                Uri = "https://luisbeltran.mx"
            });
        }

        [RelayCommand]
        private async Task ShareFile()
        {
            if (!string.IsNullOrWhiteSpace(Document))
            {
                await share.RequestAsync(new ShareFileRequest
                {
                    Title = "Information",
                    File = new ShareFile(Document)
                });
            }
        }

        [RelayCommand]
        private async Task OpenFile()
        {
            if (!string.IsNullOrWhiteSpace(Document))
            {
                await launcher.OpenAsync(new OpenFileRequest
                {
                    Title = "Information",
                    File = new ReadOnlyFile(Document)
                });
            }
        }

        [RelayCommand]
        private async Task SendToWhatsApp()
        {
            try
            {
                var url = $"https://wa.me/{ContactPhone}?text={Message}";
                await launcher.OpenAsync(url);
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        private async Task TakeScreenshot()
        {
            var image = await screenshot.CaptureAsync();

            var stream = await image.OpenReadAsync();
            Image = ImageSource.FromStream(() => stream);

            await SaveLocalFile(image);
        }

        [RelayCommand]
        private async Task SaveLocalFile(IScreenshotResult image)
        {
            var newFile = Path.Combine(
                fileSystem.CacheDirectory,
                $"{Guid.NewGuid()}.jpg");

            using var stream = await image.OpenReadAsync();
            using var newStream = File.OpenWrite(newFile);
            await stream.CopyToAsync(newStream);

            Document = newFile;
        }

        [RelayCommand]
        private async Task TakePicture()
        {
            try
            {
                var photo = await mediaPicker.CapturePhotoAsync();

                var stream = await photo.OpenReadAsync();
                Image = ImageSource.FromStream(() => stream);

                await LoadPhotoAsync(photo);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Info", "Camera error", "OK");
            }
        }

        [RelayCommand]
        private async Task LoadPhotoAsync(FileResult photo)
        {
            if (photo == null)
                return;

            var newFile = Path.Combine(
                fileSystem.CacheDirectory,
                photo.FileName);

            using var stream = await photo.OpenReadAsync();
            using var newStream = File.OpenWrite(newFile);
            await stream.CopyToAsync(newStream);

            Document = newFile;
        }

    }
}
