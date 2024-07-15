using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class Location {
		[Key]
		public Guid Id { get; set; }
		public string Country { get; set; }
		public string Province { get; set; }
		public string District { get; set; }
		public string Ward { get; set; }
	}
}
