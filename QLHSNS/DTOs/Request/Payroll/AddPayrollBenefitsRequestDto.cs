namespace QLHSNS.DTOs.Request.Payroll {
	public class AddPayrollBenefitsRequestDto {
		public Guid PayrollId { get; set; }
		public List<Guid> BenefitIds { get; set; }
	}
}
