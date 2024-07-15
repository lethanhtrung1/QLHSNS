namespace QLHSNS.DTOs.Request.Allowance {
	public class CreateAllowanceRequestDto {
		public string AllowanceName { get; set; }
		public decimal Value { get; set; }
		public string Unit {  get; set; }
		public int Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
