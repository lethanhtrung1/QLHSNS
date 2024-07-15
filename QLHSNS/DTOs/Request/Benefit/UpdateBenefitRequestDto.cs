namespace QLHSNS.DTOs.Request.Benefit {
	public class UpdateBenefitRequestDto {
		public Guid Id { get; set; }
		public string BenefitName { get; set; }
		public string? Description { get; set; }
		public decimal Amount { get; set; }
		public int Status { get; set; } = 1;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
