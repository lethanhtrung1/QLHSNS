namespace QLHSNS.DTOs.Request.Contract {
	public class GetTotalEmployeeSalaryRequestDto {
		public Guid EmployeeId { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
	}
}
