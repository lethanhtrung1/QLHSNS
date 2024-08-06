namespace QLHSNS.DTOs.Response.Contract {
	public class ContractResponseDto {
		public Guid Id { get; set; }
		public ContractTypeDto ContractTypeResponse { get; set; }
		public ContractEmployeeDto EmployeeResponse { get; set; }
		public ContractPayrollDto PayrollResponse { get; set; }

		//public string? FilePath { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int IsDeleted { get; set; } = 0;

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
