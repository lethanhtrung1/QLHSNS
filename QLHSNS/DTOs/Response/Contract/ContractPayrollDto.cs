using QLHSNS.DTOs.Response.Payroll;

namespace QLHSNS.DTOs.Response.Contract {
	public class ContractPayrollDto {
		public Guid Id { get; set; }
		public decimal BasicSalary { get; set; }
		public double SalaryCoefficient { get; set; }
		public decimal TotalSalary { get; set; }
		public string? Notes { get; set; }
		public BonusDto Bonus { get; set; }
	}
}
