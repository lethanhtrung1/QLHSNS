namespace QLHSNS.DTOs.Request.Benefit {
	public class CreateBenefitRequestDto {
		public string BenefitName { get; set; }
		public string? Description { get; set; }
		public decimal Amount { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
