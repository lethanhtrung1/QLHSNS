using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class Contract {
		[Key]
		public Guid Id { get; set; }
		public Guid ContractTypeId { get; set; }
		public Guid EmployeeId { get; set; }
		public Guid PayrollId { get; set; }

		//public string? FilePath { get; set; }
		public int IsDeleted { get; set; } = 0;
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }

		public ContractType ContractType { get; set; }
		public Employee Employee { get; set; }
		public Payroll Payroll { get; set; }
		public List<Attachment>? Attachments { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
