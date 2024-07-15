using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class ContractType {
		[Key]
		public Guid Id { get; set; }
		public string ContractTypeName { get; set; }
		public int Status { get; set; } = 1;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
