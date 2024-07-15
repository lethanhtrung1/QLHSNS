using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class EmployeeAsset {
		[Key]
		public Guid Id { get; set; }
		public Guid EmployeeId { get; set; }
		public Guid AssetId { get; set; }
		//public DateTime IssueDate { get; set; }
		public Employee Employee { get; set; }
		public Asset Asset { get; set; }
	}
}
