using AutoMapper;
using A4KPI.Data;
using A4KPI.DTO;
using A4KPI.Models;
using A4KPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace A4KPI.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
    public class MailService: IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.UserName);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
           
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Server, _mailSettings.Port, _mailSettings.EnableSsl);
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
