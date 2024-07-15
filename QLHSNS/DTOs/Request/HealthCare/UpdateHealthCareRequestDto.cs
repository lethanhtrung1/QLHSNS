namespace QLHSNS.DTOs.Request.HealthCareRequestDto {
	public class UpdateHealthCareRequestDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public int Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
