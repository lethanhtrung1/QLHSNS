namespace QLHSNS.DTOs.Response.Employee {
	public class EmployeeAssetResponseDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public List<EmployeeAssetDto> Assets { get; set; }
	}
}
