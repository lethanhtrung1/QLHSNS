namespace QLHSNS.DTOs.Request.Asset {
	public class UpdateAssetRequestDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int Status { get; set; }
		public DateTime PurchaseDate { get; set; }
	}
}
