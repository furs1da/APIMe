using APIMe.Utilities.EmailSender;

namespace APIMe.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
