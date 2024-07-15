namespace QLHSNS.DTOs.Response.Employee {
	public class EmployeeAssetDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Status { get; set; }
		public DateTime PurchaseDate { get; set; }
	}
}
