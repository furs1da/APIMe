using APIMe.Services.Email;
using APIMe.Utilities.EmailSender;

namespace APIMe.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
