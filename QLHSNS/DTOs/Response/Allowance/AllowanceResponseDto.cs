namespace QLHSNS.DTOs.Response.Allowance {
	public class AllowanceResponseDto {
		public Guid Id { get; set; }
		public string AllowanceName { get; set; }
		public decimal Value { get; set; }
		public string Unit { get; set; }
		public int Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
