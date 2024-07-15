namespace QLHSNS.DTOs.Request.Payroll {
	public class CreatePayrollRequestDto {
		public decimal BasicSalary { get; set; }
		public double SalaryCoefficient { get; set; }
		//public decimal TotalSalary { get; set; }
		public int Status { get; set; }
		public string? Notes { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public List<Guid>? AllowaceIds { get; set; }
		public List<Guid>? BennefitIds { get; set; }
	}
}
