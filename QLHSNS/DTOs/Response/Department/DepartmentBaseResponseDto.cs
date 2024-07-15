namespace QLHSNS.DTOs.Response.Department {
	public class DepartmentBaseResponseDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
