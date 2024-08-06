namespace QLHSNS.DTOs.Response.OverTime {
	public class OverTimeResponse {
		public Guid Id { get; set; }
		public Guid EmployeeId { get; set; }
		public int TotalHour { get; set; }
		public DateTime OverTimeDate { get; set; }
	}
}
