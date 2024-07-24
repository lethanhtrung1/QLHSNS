namespace QLHSNS.DTOs.Request.Department {
	public class UpdateDepartmentRequestDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public List<Guid>? JobTitleIds { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
