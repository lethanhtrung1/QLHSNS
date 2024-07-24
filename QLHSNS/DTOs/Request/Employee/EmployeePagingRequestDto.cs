using QLHSNS.DTOs.Pagination;

namespace QLHSNS.DTOs.Request.Employee {
	public class EmployeePagingRequestDto : PagingRequestBase {
		public string? Keyword { get; set; }
		public string? Cccd { get; set; }
		public int Gender { get; set; } = -1;
		public Guid? DepartmentId { get; set; }
		public Guid? JobTitleId { get; set; }
		public Guid? LocationId { get; set; }
		public Guid? BankBranchId { get; set; }
		public Guid? HealthCareId { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Email { get; set; }
		public string? BankNumber { get; set; }
		public int IsWorking { get; set; }

		public string? SortField { get; set; }
		public int SortOrder { get; set; }
	}
}
