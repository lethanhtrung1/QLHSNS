namespace QLHSNS.DTOs.Request.Allowance {
	public class UpdateAllowanceRequestDto {
		public Guid Id { get; set; }
		public string AllowanceName { get; set; }
		public decimal Value { get; set; }
		public string Unit { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
