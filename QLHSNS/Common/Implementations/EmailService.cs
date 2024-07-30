using Microsoft.Extensions.Options;
using QLHSNS.Common.Interfaces;
using QLHSNS.DTOs.SendEmail;
using QLHSNS.Options;
using System.Net.Mail;
using System.Net;

namespace QLHSNS.Common.Implementations {
	public class EmailService : IEmailService {
		private readonly GmailOptions _gmailOptions;

		public EmailService(IOptions<GmailOptions> gmailOptions) {
			_gmailOptions = gmailOptions.Value;
		}

		public async Task SendEmailAsync(SendEmailRequest sendEmailRequest) {
			MailMessage mailMessage = new MailMessage {
				From = new MailAddress(_gmailOptions.Email),
				Subject = sendEmailRequest.Subject,
				Body = sendEmailRequest.Body,
				IsBodyHtml = sendEmailRequest.IsBodyHtml
			};

			mailMessage.To.Add(sendEmailRequest.Recipient);

			using var smtpClient = new SmtpClient();
			smtpClient.Host = _gmailOptions.Host;
			smtpClient.Port = _gmailOptions.Port;
			smtpClient.Credentials = new NetworkCredential(_gmailOptions.Email, _gmailOptions.Password);
			smtpClient.EnableSsl = true;

			await smtpClient.SendMailAsync(mailMessage);
		}
	}
}
