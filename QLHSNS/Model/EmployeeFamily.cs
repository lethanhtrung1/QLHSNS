namespace QLHSNS.Model {
	public class EmployeeFamily {
		public Guid Id { get; set; }
		public Guid EmployeeId { get; set; }

		public int Deduction { get; set; }
		public DateTime? EffectiveDate { get; set; }

		public Employee Employee { get; set; }
		public List<EmployeeFamilyDetail>? EmployeeFamilyDetails { get; set; }
	}
}
