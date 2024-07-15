namespace QLHSNS.Model {
	public class EmployeeFamily {
		public Guid Id { get; set; }
		public Guid EmployeeId { get; set; }
		public string Name { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string PhoneNumber { get; set; }
		public string Relationship { get; set; }
		public string Occupation { get; set; }

		public Employee Employee { get; set; }
	}
}
