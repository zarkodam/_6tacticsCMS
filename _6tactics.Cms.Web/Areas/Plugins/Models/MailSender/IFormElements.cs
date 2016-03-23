namespace _6tactics.Cms.Web.Areas.Plugins.Models.MailSender
{
    public interface IFormElements
    {
        string Language { get; set; }
        string Email { get; set; }
        string Name { get; set; }
        string Subject { get; set; }
        string Content { get; set; }
        string File { get; set; }
    }
}
