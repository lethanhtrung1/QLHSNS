namespace QLHSNS.DTOs.Request.OverTime {
	public class UpdateOverTimeRequestDto {
		public Guid Id { get; set; }
		public Guid EmployeeId { get; set; }
		public int TotalHour { get; set; }
		public DateOnly OverTimeDate { get; set; }
	}
}
