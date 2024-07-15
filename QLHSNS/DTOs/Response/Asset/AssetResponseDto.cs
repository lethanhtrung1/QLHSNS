namespace QLHSNS.DTOs.Response.Asset {
	public class AssetResponseDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public DateTime PurchaseDate { get; set; }
	}
}
