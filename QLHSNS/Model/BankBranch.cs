using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class BankBranch {
		[Key]
		public Guid Id { get; set; }
		public Guid BankId { get; set; }
		public string BranchName { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public int Status { get; set; } = 1;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public Bank Bank { get; set; }
	}
}
