namespace QLHSNS.DTOs.Request.Department {
	public class DepartmentJobTitleRequestDto {
		public Guid DepartmentId { get; set; }
		public List<Guid> JobTitleIds { get; set; }
	}
}
