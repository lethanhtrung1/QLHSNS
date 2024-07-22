namespace QLHSNS.Model {
	public class EmployeeFamilyDetail {
		public Guid Id { get; set; }
		public Guid EmployeeFamilyId { get; set; }
		public string Name { get; set; }
		public int Gender { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string PhoneNumber { get; set; }
		public string Relationship { get; set; }
		public string Occupation { get; set; }

		public EmployeeFamily EmployeeFamily { get; set; }
	}
}
