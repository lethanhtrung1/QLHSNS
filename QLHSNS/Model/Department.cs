using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class Department {
		[Key]
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public virtual List<DepartmentJobTitle> DepartmentJobTitles { get; set; }
		public virtual List<Employee>? Employees { get; set; }
	}
}
