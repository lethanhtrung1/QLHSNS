using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class Employee {
		[Key]
		public Guid Id { get; set; }
		public Guid DepartmentId { get; set; }
		public Guid JobTitleId { get; set; }
		public string Name { get; set; }
		public DateOnly DateOfBirth { get; set; }
		public int Gender { get; set; }
		public string Cccd { get; set; }
		public Guid? LocationId { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Email { get; set; }
		public string BankNumber { get; set; }
		public Guid? BankBranchId { get; set; }
		public Guid? HealthCareId { get; set; }
		public int IsWorking { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public Department Department { get; set; }
		public JobTitle JobTitle { get; set; }
		public Location? Location { get; set; }
		public BankBranch? BankBranch { get; set; }
		public virtual List<EmployeeAsset>? EmployeeAssets { get; set; }
		public EmployeeFamily? EmployeeFamily { get; set; }
		public HealthCare? HealthCare { get; set; }
		public virtual List<Contract> Contracts { get; set; }
		public virtual List<OverTime> OverTimes { get; set; }
		public virtual List<Reward> Rewards { get; set; }

		public DateOnly DateOfJoining { get; set; }
		public DateOnly? DateOfLeaving { get; set; }
	}
}
