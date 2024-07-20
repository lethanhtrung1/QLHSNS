namespace QLHSNS.DTOs.Request.Asset {
	public class CreateAssetRequestDto {
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public DateTime PurchaseDate { get; set; }
	}
}
