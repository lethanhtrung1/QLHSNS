namespace QLHSNS.DTOs.Request.Contract {
	public class CreateContractRequestDto {
		public Guid ContractTypeId { get; set; }
		public Guid EmployeeId { get; set; }
		public Guid PayrollId { get; set; }

		//public string? FilePath { get; set; }
		//public IFormFile File { get; set; }
		public int IsDeleted { get; set; } = 0;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
