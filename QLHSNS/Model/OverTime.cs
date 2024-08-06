using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class OverTime {
		[Key]
		public Guid Id { get; set; }
		public Guid EmployeeId { get; set; }
		//public int Month { get; set; }
		//public int Year { get; set; }
		public int TotalHour { get; set; }
		public DateTime OverTimeDate { get; set; }

		public Employee Employee { get; set; }
	}
}
