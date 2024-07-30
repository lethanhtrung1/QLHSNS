namespace QLHSNS.DTOs.SendEmail {
	public record SendEmailRequest(string Recipient, string Subject, string Body, bool IsBodyHtml = true);

	//public class SendEmailRequest {
	//	public string Recipient { get; set; }
	//	public string Subject { get; set; }
	//	public string Body { get; set; }
	//	public bool IsBodyHtml { get; set; } = true;
	//}
}
