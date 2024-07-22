namespace QLHSNS.DTOs.Response.EmployeeFamily {
	public class GetEmployeeFamilyWithDetailResponseDto {
		public Guid Id { get; set; }
		public Guid EmployeeId { get; set; }
		public int Deduction { get; set; }
		public DateTime? EffectiveDate { get; set; }
		public List<EmployeeFamilyDetailResponseDto>? FamilyDetails { get; set; }
	}
}
