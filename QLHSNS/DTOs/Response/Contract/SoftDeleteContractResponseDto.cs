namespace QLHSNS.DTOs.Response.Contract {
	public class SoftDeleteContractResponseDto {
		public Guid Id { get; set; }
		public int IsDeleted { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
