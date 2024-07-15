namespace QLHSNS.DTOs.Request.Employee {
	public class EmployeeAssetRequestDto {
		public Guid EmployeeId { get; set; }
		public List<Guid> AssetIds {  get; set; }
	}
}
