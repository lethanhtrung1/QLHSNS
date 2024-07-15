namespace QLHSNS.Model {
	public class Reward {
		public Guid Id { get; set; }
		public Guid EmployeeId { get; set; }
		public string? Description { get; set; }
		public decimal RewardAmount { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public int IsReceived { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		
		//public Employee Employee { get; set; }
	}
}
