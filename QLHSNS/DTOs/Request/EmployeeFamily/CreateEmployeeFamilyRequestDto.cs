namespace QLHSNS.DTOs.Request.EmployeeFamily {
	public class CreateEmployeeFamilyRequestDto {
		public Guid EmployeeId { get; set; }
		public int Deduction { get; set; }
		public DateTime? EffectiveDate { get; set; }
	}
}
