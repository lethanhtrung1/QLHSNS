namespace QLHSNS.DTOs.Request.ContractType {
	public class CreateContractTypeRequestDto {
		public string ContractTypeName { get; set; }
		public int Status { get; set; } = 1;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
