namespace QLHSNS.DTOs.Response.Payroll {
	public class BonusDto {
		public List<PayrollAllowanceResponseDto> Allowances { get; set; }
		public List<PayrollBenefitResponseDto> Benefits { get; set; }
	}
}
