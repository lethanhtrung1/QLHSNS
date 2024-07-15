using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class Allowance {
		[Key]
		public Guid Id { get; set; }
		public string AllowanceName { get; set; }
		public decimal Value { get; set; }
		public string Unit { get; set; }
		public int Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public List<PayrollAllowance> PayrollAllowances { get; set; }
	}
}
