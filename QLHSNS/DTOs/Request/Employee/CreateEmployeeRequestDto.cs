namespace QLHSNS.DTOs.Request.EmployeeRequestDto {
	public class CreateEmployeeRequestDto {
		//public Guid Id { get; set; }
		public Guid DepartmentId { get; set; }
		public Guid JobTitleId { get; set; }
		public string Name { get; set; }
		public DateTime DateOfBirth { get; set; }
		public int Gender { get; set; }
		public string Cccd { get; set; }
		public Guid? LocationId { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Email { get; set; }
		public string BankNumber { get; set; }
		public Guid? BankBranchId { get; set; }
		public Guid? HealthCareId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}