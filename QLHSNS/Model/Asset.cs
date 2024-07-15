using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class Asset {
		[Key]
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int Status { get; set; }
		public DateTime PurchaseDate { get; set; }
		public List<EmployeeAsset>? EmployeeAssets { get; set; }
	}
}
