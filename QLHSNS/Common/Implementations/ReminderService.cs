using Microsoft.EntityFrameworkCore;
using QLHSNS.Common.Interfaces;
using QLHSNS.Data;
using QLHSNS.DTOs.SendEmail;

namespace QLHSNS.Common.Implementations {
	public class ReminderService : IReminderService {
		private readonly AppDbContext _dbContext;
		private readonly IEmailService _emailService;

		public ReminderService(AppDbContext dbContext, IEmailService emailService) {
			_dbContext = dbContext;
			_emailService = emailService;
		}

		public async Task BirthdayReminder() {
			var employees = await _dbContext.Employees
				.Where(x => x.DateOfBirth.Year == DateTime.Now.Year && 
				x.DateOfBirth.Month == DateTime.Now.Month && 
				x.DateOfBirth.Date == DateTime.Now.Date)
				.ToListAsync();

			if (employees != null && employees.Count != 0) {
				foreach (var employee in employees) {
					string recipient = employee.Email!;

					string subject = $"Chúc Mừng Sinh Nhật {employee.Name}!";

					string body = $@"
						<html>
						<head>
							<style>
								body {{ font-family: Arial, sans-serif; }}
								.header {{ font-size: 24px; font-weight: bold; color: #333; }}
								.content {{ margin-top: 10px; }}
								.footer {{ margin-top: 20px; font-size: 12px; color: gray; }}
							</style>
						</head>
						<body>
							<div class='header'>Chúc Mừng Sinh Nhật {employee.Name}!</div>
							<div class='content'>
								<p>Kính gửi {employee.Name},</p>
								<p>Chúng tôi xin gửi đến bạn những lời chúc tốt đẹp nhất nhân dịp sinh nhật của bạn hôm nay! 🎉</p>
								<p>Chúc bạn có một ngày sinh nhật vui vẻ, tràn đầy niềm vui và hạnh phúc bên gia đình và bạn bè. Hy vọng rằng năm tới sẽ mang đến cho bạn nhiều thành công, sức khỏe và những trải nghiệm tuyệt vời.</p>
								<p>Cảm ơn bạn vì những đóng góp và nỗ lực không ngừng của bạn cho công ty. Chúng tôi rất tự hào khi có bạn là một phần của đội ngũ VMO.</p>
								<p>Chúc bạn một ngày sinh nhật thật đặc biệt!</p>
							</div>
							<div class='footer'>
								<p>Trân trọng,</p>
								<p>VMO</p>
							</div>
						</body>
						</html>";

					var emailRequest = new SendEmailRequest(recipient, subject, body);

					if (emailRequest.Recipient != null) {
						await _emailService.SendEmailAsync(emailRequest);
					}
				}
			}
		}

		public async Task EmployeeContractExpiryReminder() {
			var contracts = await _dbContext.Contracts
				.Where(x => x.EndDate.Year == DateTime.Now.Year && 
				x.EndDate.Month == DateTime.Now.Month && 
				x.EndDate.Date == DateTime.Now.Date && 
				x.IsDeleted == 0).ToListAsync();

			if (contracts != null && contracts.Count != 0) {
				foreach (var contract in contracts) {
					string employeeName = await _dbContext.Employees.Where(x => x.Id == contract.EmployeeId)
						.Select(x => x.Name).FirstOrDefaultAsync() ?? "";

					string recipient = await _dbContext.Employees.Where(x => x.Id == contract.EmployeeId)
						.Select(x => x.Email).FirstOrDefaultAsync() ?? "";

					string subject = $"Thông Báo: Hợp Đồng Lao Động của {employeeName} Sắp Hết Hạn";

					string body = $@"
						<html>
						<head>
							<style>
								body {{ font-family: Arial, sans-serif; }}
								.header {{ font-size: 20px; font-weight: bold; }}
								.content {{ margin-top: 10px; }}
								.footer {{ margin-top: 20px; font-size: 12px; color: gray; }}
							</style>
						</head>
						<body>
							<div class='header'>Thông Báo Hợp Đồng Sắp Hết Hạn</div>
							<div class='content'>
								<p>Kính gửi {employeeName},</p>
								<p>Hợp đồng lao động của bạn với công ty <strong>VMO</strong> sẽ hết hạn vào ngày <strong>{contract.EndDate.ToShortDateString()}</strong>.</p>
								<p>Vui lòng liên hệ với bộ phận nhân sự để thảo luận về các bước tiếp theo.</p>
							</div>
							<div class='footer'>
								<p>Trân trọng,</p>
								<p>VMO</p>
							</div>
						</body>
						</html>";

					var emailRequest = new SendEmailRequest(recipient, subject, body);

					// Send email
					if (emailRequest.Recipient != null) {
						await _emailService.SendEmailAsync(emailRequest);
					}

					// Update Contract
					contract.IsDeleted = 1;
				}
				await _dbContext.SaveChangesAsync();
			}
		}
	}
}
