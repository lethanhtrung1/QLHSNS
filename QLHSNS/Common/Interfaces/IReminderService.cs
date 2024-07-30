namespace QLHSNS.Common.Interfaces {
	public interface IReminderService {
		Task BirthdayReminder();
		Task EmployeeContractExpiryReminder();
	}
}
