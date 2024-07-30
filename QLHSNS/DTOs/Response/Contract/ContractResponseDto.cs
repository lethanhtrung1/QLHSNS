namespace QLHSNS.DTOs.Response.Contract {
	public class ContractResponseDto {
		public Guid Id { get; set; }
		public ContractTypeDto ContractType { get; set; }
		public ContractEmployeeDto Employee { get; set; }
		public ContractPayrollDto Payroll { get; set; }

		public string? FilePath { get; set; }
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }
		public int IsDeleted { get; set; } = 0;

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
