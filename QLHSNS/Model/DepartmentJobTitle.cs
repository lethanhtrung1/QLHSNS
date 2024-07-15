using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class DepartmentJobTitle {
		[Key]
		public Guid Id { get; set; }
		public Guid DepartmentId { get; set; }
		public Department Department { get; set; }
		public Guid JobTitleId { get; set; }
		public JobTitle JobTitle { get; set; }
	}
}
