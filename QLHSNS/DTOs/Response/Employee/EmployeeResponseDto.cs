namespace QLHSNS.DTOs.Response.Employee {
	public class EmployeeResponseDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime DateOfBirth { get; set; }
		public int Gender { get; set; }
		public string Cccd { get; set; }
		public EmployeeCVDto? Address { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Email { get; set; }
		public string? BankNumber { get; set; }
		public EmployeeBankBranchDto? BankBranch { get; set; }
		public EmployeeDepartmentDto? Department { get; set; }
		public EmployeeJobTitleDto? JobTitle { get; set; }
		public EmployeeHealthCareDto? HealthCare { get; set; }
		public int IsWorking { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
