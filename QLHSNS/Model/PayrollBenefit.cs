using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class PayrollBenefit {
		[Key]
		public Guid Id { get; set; }
		public Guid BenefitId { get; set; }
		public Guid PayrollId { get; set; }
		public Benefit Benefit { get; set; }
		public Payroll Payroll { get; set; }
	}
}
