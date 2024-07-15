using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class JobTitle {
		[Key]
		public Guid Id { get; set; }
		public string JobTitleName { get; set; }
		public int Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public List<DepartmentJobTitle> DepartmentJobTitles { get; set; }
	}
}
