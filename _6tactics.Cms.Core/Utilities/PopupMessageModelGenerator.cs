using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Models.Popup;

namespace _6tactics.Cms.Core.Utilities
{
    public class PopupMessageModelGenerator
    {
        public static Popup Generate(ContentItemAction contentItemAction, MessageType messageType)
        {
            string popupMessageType;

            switch (messageType)
            {
                case MessageType.Info:
                    popupMessageType = "popup-info";
                    break;
                case MessageType.Success:
                    popupMessageType = "popup-success";
                    break;
                case MessageType.Warning:
                    popupMessageType = "popup-warning";
                    break;
                case MessageType.Error:
                    popupMessageType = "popup-error";
                    break;
                default:
                    popupMessageType = "";
                    break;
            }

            string popupMessage;

            switch (contentItemAction)
            {
                case ContentItemAction.Create:
                    popupMessage = "New record added successfully!";
                    break;
                case ContentItemAction.Edit:
                    popupMessage = "Record successfully updated!";
                    break;
                case ContentItemAction.Priorities:
                    popupMessage = "New priorities are successfully placed!";
                    break;
                case ContentItemAction.Delete:
                    popupMessage = "Record deleted successfully!";
                    break;
                case ContentItemAction.FileUploaded:
                    popupMessage = "Files was successfully uploaded!";
                    break;
                case ContentItemAction.FileExtensionNotPermited:
                    popupMessage = "<b>File format which you want to upload is not permitted!</b>" +
                                           "<br> Allowed file extensions for images are: <b>.bmp, .jpeg, .jpg, .png, .gif</b>" +
                                           ", for compressed files are: <b>.zip, .rar, .7z.</b> and " +
                                           "for documents file are: <b>.doc, .docx, .xls, .xlsx, .ppt, .pptx, .pdf, .txt</b>";
                    break;
                case ContentItemAction.NewProject:
                    popupMessage = "You have to create <b>project</b> element first!";
                    break;
                case ContentItemAction.ModelState:
                    popupMessage = "Form data is not valid!";
                    break;
                case ContentItemAction.EmptyLanguageObject:
                    popupMessage = "Selected language hasn't generated content yet!";
                    break;
                case ContentItemAction.AlreadyExistInDbOnCreate:
                    popupMessage = "<b>Element</b> with same name <b>already exist</b>!";
                    break;
                case ContentItemAction.AlreadyExistInDbOnEdit:
                    popupMessage = "<b>Element</b> with same name <b>already exist</b>!";
                    break;
                case ContentItemAction.FooterUnderLanguageAlreadyExistOnCreate:
                    popupMessage = "You can have only one <b>Footer</b> element under <b>language</b>!";
                    break;
                case ContentItemAction.FooterUnderLanguageAlreadyExistOnEdit:
                    popupMessage = "You can have only one <b>Footer</b> element under <b>language</b>!";
                    break;
                default:
                    popupMessage = "";
                    break;
            }

            return new Popup { MessageType = popupMessageType, Message = popupMessage };
        }
    }
}
