using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class PayrollAllowance {
		[Key]
		public Guid Id { get; set; }
		public Guid AllowanceId { get; set; }
		public Guid PayrollId { get; set; }

		public Allowance Allowance { get; set; }
		public Payroll Payroll { get; set; }
	}
}
