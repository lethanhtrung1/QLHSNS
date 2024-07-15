namespace QLHSNS.DTOs.Response.Payroll {
	public class PayrollBenefitResponseDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Amount { get; set; }
	}
}
