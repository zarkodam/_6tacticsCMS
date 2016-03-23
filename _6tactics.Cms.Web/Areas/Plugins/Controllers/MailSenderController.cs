using _6tactics.Cms.Core.Attributes;
using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Services.Admin;
using _6tactics.Cms.Services.Plugins;
using _6tactics.Cms.Web.App_Logic;
using _6tactics.Cms.Web.Areas.Plugins.Models.MailSender;
using _6tactics.Cms.Web.Areas.Plugins.ViewModels.MailSender;
using _6tactics.SimpleCaptcha;
using _DataAccess.Repositories;
using SendGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Areas.Plugins.Controllers
{
    public class MailSenderController : Controller
    {
        #region Fields

        private readonly IPluginsManagerService _pluginsManagerService;
        private readonly IContentItemRepository _contentItemRepository;
        private readonly IUserActivityTrackingService _activityTracking;
        private readonly IPostAbusingSettingsService _postAbusingSettings;
        private static AccountSettings _accountSettings;
        private static FormSettings _formSettings;
        private static List<InputTitleSettings> _inputTitleSettings = new List<InputTitleSettings>();
        private static List<ValidationMessage> _validationMessages;

        #endregion


        #region Ctor

        public MailSenderController(
            IPluginsManagerService pluginsManagerService,
            IContentItemRepository contentItemRepository,
            IUserActivityTrackingService activityTracking,
            IPostAbusingSettingsService postAbusingSettings)
        {
            _pluginsManagerService = pluginsManagerService;
            _contentItemRepository = contentItemRepository;
            _activityTracking = activityTracking;
            _postAbusingSettings = postAbusingSettings;

            LoadSettings(contentItemRepository);
        }

        #endregion


        #region MailFormPreview

        [AjaxOnly]
        public PartialViewResult MailFormPreview()
        {
            return MailFormHandler();
        }

        #endregion


        #region MailForm

        [ChildActionOnlyCustom, ExcludeFromPluginActionList]
        public PartialViewResult MailForm()
        {
            return MailFormHandler();
        }

        #endregion


        #region MailFormPOST

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task MailFormSenderHandler(MailFormViewModel model)
        {
            if (!_formSettings.IsCaptchaEnabled)
                ModelState.Remove("MailForm.CaptchaCode");

            // In case of possible Brute-force attack block ip
            if (_activityTracking.IsRequestComingFromPossibleBot(Request.UserHostAddress, Request.UserHostName,
                _postAbusingSettings.Get(PostAbusingSettingsFor.LoginForm)))
                return;

            var credentials = new NetworkCredential(_accountSettings.Username, _accountSettings.Password);

            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();

            // Add multiple addresses to the To field.
            //var recipients = new List<String>
            //{
            //    @"Zarko Dam <zdam@live.com>",
            //    @"Zarko Dam <zarko@6tactics.com>",
            //};

            myMessage.AddTo(_accountSettings.TargetAddress);
            myMessage.From = new MailAddress(model.MailForm.Email, model.MailForm.Name);
            myMessage.Subject = model.MailForm.Subject;
            myMessage.Text = model.MailForm.Content;
            //myMessage.Html = "<p><a href=\"http://www.example.com\">Hello World Link!</a></p>";

            // Allowed extension
            string[] allowedExtension = { ".rar", ".zip", ".7z" };

            if (model.MailForm.File != null && !allowedExtension.Contains(Path.GetExtension(model.MailForm.File.FileName.ToLower())))
                myMessage.AddAttachment(model.MailForm.File.InputStream, model.MailForm.File.FileName);

            // Create an Web transport for sending email.
            var transportWeb = new SendGrid.Web(credentials);

            // Send the email.
            await transportWeb.DeliverAsync(myMessage);
        }

        #endregion


        #region SendGridAccountSettings

        [AjaxOnly]
        public PartialViewResult SendGridAccountSettings()
        {
            return PartialView(_accountSettings);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void SendGridAccountSettings(AccountSettings model)
        {
            _accountSettings = model;
            _pluginsManagerService.SavePluginData("MailSender", "accountSettings", model);
        }

        #endregion


        #region InputTitleSettings

        [AjaxOnly]
        public PartialViewResult PlaceholderTitles()
        {
            var model = new InputTitleSettingsViewModel
            {
                LanguageList = _contentItemRepository.LanguageList.Select(i => i.ToUpper()),
                InputTitleSettings = _inputTitleSettings,
                FormSettings = _formSettings
            };
            return PartialView(model);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void PlaceholderTitles(FormCollection formCollection)
        {
            string[] language = formCollection.GetValues("Language");
            string[] name = formCollection.GetValues("Name");
            string[] email = formCollection.GetValues("Email");
            string[] subject = formCollection.GetValues("Subject");
            string[] content = formCollection.GetValues("Content");
            string[] captchaCode = formCollection.GetValues("CaptchaCode");
            string[] file = formCollection.GetValues("File");
            string[] uploadButton = formCollection.GetValues("UploadButton");
            string[] sendButton = formCollection.GetValues("SendButton");

            var dataToSerialize = new List<InputTitleSettings>();

            if (language != null && name != null && email != null && subject != null && content != null && sendButton != null)
            {
                for (var i = 0; i < language.Length; i++)
                {
                    var inputTitleSettings = new InputTitleSettings
                    {
                        Language = language[i],
                        Name = name[i],
                        Email = email[i],
                        Subject = subject[i],
                        Content = content[i],
                        SendButton = sendButton[i]
                    };

                    if (file != null && uploadButton != null)
                        inputTitleSettings.File = file[i];

                    if (captchaCode != null)
                        inputTitleSettings.CaptchaCode = captchaCode[i];

                    dataToSerialize.Add(inputTitleSettings);
                }
            }

            _inputTitleSettings = dataToSerialize;
            _pluginsManagerService.SavePluginData("MailSender", "inputTitleSettings", dataToSerialize);

        }

        #endregion


        #region ValidationMessages

        [AjaxOnly]
        public PartialViewResult ValidationMessages()
        {
            var model = new ValidationsViewModel
            {
                LanguageList = _contentItemRepository.LanguageList.Select(i => i.ToUpper()),
                ValidationMessages = _validationMessages,
                InputTitleSettings = _inputTitleSettings,
                FormSettings = _formSettings
            };

            return PartialView(model);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void ValidationMessages(FormCollection formCollection)
        {
            string[] language = formCollection.GetValues("Language");
            string[] name = formCollection.GetValues("Name");
            string[] email = formCollection.GetValues("Email");
            string[] subject = formCollection.GetValues("Subject");
            string[] content = formCollection.GetValues("Content");
            string[] file = formCollection.GetValues("File");
            string[] captchaCode = formCollection.GetValues("CaptchaCode");

            var dataToSerialize = new List<ValidationMessage>();

            if (language != null && name != null && email != null && subject != null && content != null)
            {
                for (var i = 0; i < language.Length; i++)
                {
                    var validationMessage = new ValidationMessage
                    {
                        Language = language[i],
                        Name = name[i],
                        Email = email[i],
                        Subject = subject[i],
                        Content = content[i],
                    };

                    if (file != null)
                        validationMessage.File = file[i];

                    if (captchaCode != null)
                        validationMessage.CaptchaCode = captchaCode[i];

                    dataToSerialize.Add(validationMessage);

                }
            }

            _validationMessages = dataToSerialize;
            _pluginsManagerService.SavePluginData("MailSender", "validationMessages", dataToSerialize);
        }

        #endregion


        #region FormSettings

        [AjaxOnly]
        public PartialViewResult FormSettings()
        {
            return PartialView(_formSettings);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void FormSettings(FormSettings model)
        {
            _pluginsManagerService.SavePluginData("MailSender", "formSettings", model);
        }

        #endregion


        #region Helpers

        [ExcludeFromPluginActionList]
        private void LoadSettings(IContentItemRepository contentItemRepository)
        {
            // Account settings
            var accountSettingsData = _pluginsManagerService.GetPluginData<AccountSettings>("MailSender", "accountSettings");
            _accountSettings = accountSettingsData ?? new AccountSettings();

            // Form settings
            var formSettingsData = _pluginsManagerService.GetPluginData<FormSettings>("MailSender", "formSettings");
            _formSettings = formSettingsData ?? new FormSettings();

            // Input title settings
            var inputSettingsData = _pluginsManagerService.GetPluginData<List<InputTitleSettings>>("MailSender", "inputTitleSettings");
            var inputSettings = inputSettingsData.Count != 0 ? inputSettingsData : new List<InputTitleSettings> { new InputTitleSettings() };

            foreach (var inputSetting in inputSettings.Where(i => !i.Language.Equals("EN")))
                if (!contentItemRepository.LanguageList.Any(i => i.Equals(inputSetting.Language, StringComparison.InvariantCultureIgnoreCase)))
                {
                    inputSettings.Remove(inputSetting);
                    break;
                }

            _inputTitleSettings = inputSettings;


            // Validation messages
            var validationMessagesData = _pluginsManagerService.GetPluginData<List<ValidationMessage>>("MailSender", "validationMessages");
            var validationMessages = validationMessagesData.Count != 0 ? validationMessagesData : new List<ValidationMessage> { new ValidationMessage() };

            foreach (var validationMessage in validationMessages.Where(i => !i.Language.Equals("EN")))
                if (!contentItemRepository.LanguageList.Any(i => i.Equals(validationMessage.Language, StringComparison.InvariantCultureIgnoreCase)))
                {
                    validationMessages.Remove(validationMessage);
                    break;
                }

            _validationMessages = validationMessages;
        }

        [ExcludeFromPluginActionList]
        private PartialViewResult MailFormHandler()
        {
            InputTitleSettings inputTitleSetting = ElementsVisibilityUtility.CurrentController.Equals("mailsender", StringComparison.InvariantCultureIgnoreCase)
                ? _inputTitleSettings.FirstOrDefault() ?? new InputTitleSettings()
                : _inputTitleSettings.FirstOrDefault
                (i => i.Language.Equals(ElementsVisibilityUtility.CurrentLanguage, StringComparison.InvariantCultureIgnoreCase)) ?? new InputTitleSettings();

            ValidationMessage validationMessage = _validationMessages.FirstOrDefault(i => i.Language.Equals(inputTitleSetting.Language));

            var model = new MailFormViewModel
            {
                MailForm = new MailForm { MailFormCaptcha = new SimpleCaptchaInitializer().GenerateCaptchaString("MailFormCaptcha") },
                InputTitleSettings = inputTitleSetting,
                ValidationMessage = validationMessage,
                FormSettings = _formSettings
            };

            return PartialView(model);
        }

        #endregion
    }
}
