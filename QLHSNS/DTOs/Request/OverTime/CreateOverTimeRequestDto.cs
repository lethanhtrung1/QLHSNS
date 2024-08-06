namespace QLHSNS.DTOs.Request.OverTime {
	public class CreateOverTimeRequestDto {
		public Guid EmployeeId { get; set; }
		public int TotalHour { get; set; }
		public DateTime OverTimeDate { get; set; }
	}
}
