namespace QLHSNS.DTOs.Request.EmployeeFamily {
	public class UpdateEmployeeFamilyRequestDto {
		public Guid Id { get; set; }
		public Guid? EmployeeId { get; set; }
		public int Deduction { get; set; }
		public DateTime? EffectiveDate { get; set; }
	}
}
