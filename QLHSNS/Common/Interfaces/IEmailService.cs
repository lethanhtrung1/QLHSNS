using QLHSNS.DTOs.SendEmail;

namespace QLHSNS.Common.Interfaces {
	public interface IEmailService {
		Task SendEmailAsync(SendEmailRequest sendEmailRequest);
	}
}
