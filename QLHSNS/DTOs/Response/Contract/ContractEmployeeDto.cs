namespace QLHSNS.DTOs.Response.Contract {
	public class ContractEmployeeDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Cccd { get; set; }
		public int Gender { get; set; }
		public DateOnly DateOfBirth { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Department { get; set; }
		public string JobTitle { get; set; }
		public string BankNumber { get; set; }
		public string Bank {  get; set; }

	}
}
