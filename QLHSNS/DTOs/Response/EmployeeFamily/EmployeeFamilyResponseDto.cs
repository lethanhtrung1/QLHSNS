﻿namespace QLHSNS.DTOs.Response.EmployeeFamily {
	public class EmployeeFamilyResponseDto {
		public Guid Id { get; set; }
		public Guid EmployeeId { get; set; }
		public int Deduction { get; set; }
		public DateTime? EffectiveDate { get; set; }
	}
}
