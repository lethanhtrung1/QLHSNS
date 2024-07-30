namespace QLHSNS.DTOs.Request.Contract {
	public class UpdateContractRequestDto {
		public Guid Id { get; set; }
		public Guid? ContractTypeId { get; set; }
		public Guid? EmployeeId { get; set; }
		public Guid? PayrollId { get; set; }

		public DateOnly? StartDate { get; set; }
		public DateOnly? EndDate { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
