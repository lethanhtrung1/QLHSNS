using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class Employee {
		[Key]
		public Guid Id { get; set; }
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
		public Department Department { get; set; }
		public JobTitle JobTitle { get; set; }
		public Location? Location { get; set; }
		public BankBranch? BankBranch { get; set; }
		public List<EmployeeAsset>? EmployeeAssets { get; set; }
		public List<EmployeeFamily>? EmployeeFamilies { get; set; }
		public HealthCare? HealthCare { get; set; }
		public virtual List<Contract> Contracts { get; set; }
		public List<OverTime> OverTimes { get; set; }
		public virtual List<Reward> Rewards { get; set; }
	}
}
