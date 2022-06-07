using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.EmailDto;
using BlogProject.Services.Abstract;
using BlogProject.Shared.Utilities.Results.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using BlogProject.Shared.Utilities.Results.Concrete;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Concrete
{
    public class MailManager : IMailService
    {
        #region Variables
        private readonly SmtpSettings _smtpSettings;

        #endregion

        #region Constructor

        public MailManager(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        #endregion


        public IResult SendContactMail(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail),
                To = { new MailAddress("cuseg09@gmail.com") },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = $"<h1>{emailSendDto.Name}</h1>,<h2>Gönderen E-Posta Adresi:{emailSendDto.Email}</h2>\n<p>{emailSendDto.Message}</p>"
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);
            return new Result(ResultStatus.Success, $"E-Posta Gönderildi");
        }

        public IResult SendMail(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail),
                To = { new MailAddress(emailSendDto.Email) },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = emailSendDto.Message
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);
            return new Result(ResultStatus.Success, $"E-Posta Gönderildi");
        }
    }
}
