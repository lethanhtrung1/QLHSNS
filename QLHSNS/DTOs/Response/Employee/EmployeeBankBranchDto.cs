namespace QLHSNS.DTOs.Response.Employee {
	public class EmployeeBankBranchDto {
		public Guid Id { get; set; }
		public string BranchName { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public EmployeeBankDto Bank { get; set; }
	}
}
