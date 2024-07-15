namespace QLHSNS.DTOs.Response.Department {
	public class DepartmentResponseDto : DepartmentBaseResponseDto {
		public List<DepartmentJobTitleResponseDto> JobTitles { get; set; }
	}
}
