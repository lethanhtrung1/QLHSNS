namespace QLHSNS.DTOs.Response.Contract {
	public class GetTotalEmployeeSalaryResponseDto {
		public Guid EmployeeId { get; set; }
		public string EmployeeName { get; set; }
		public decimal TotalSalary { get; set; }
		public int Month {  get; set; }
		public int Year { get; set; }
	}
}
