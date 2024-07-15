using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class Bank {
		[Key]
		public Guid Id { get; set; }
		public string BankName { get; set; }
		public int Status { get; set; }
		public List<BankBranch> BankBranches { get; set; }
	}
}
