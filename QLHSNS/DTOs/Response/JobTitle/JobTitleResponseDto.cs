namespace QLHSNS.DTOs.Response.JobTitle {
	public class JobTitleResponseDto {
		public Guid Id { get; set; }
		public string JobTitleName { get; set; }
		public int Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
