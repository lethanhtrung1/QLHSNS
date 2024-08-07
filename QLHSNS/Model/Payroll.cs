﻿using System.ComponentModel.DataAnnotations;

namespace QLHSNS.Model {
	public class Payroll {
		[Key]
		public Guid Id { get; set; }
		public decimal BasicSalary { get; set; }
		public double SalaryCoefficient { get; set; }
		//public string Decisions { get; set; }
		public int Status {  get; set; }
		public string? Notes { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public virtual List<PayrollAllowance> PayrollAllowances { get; set; }
		public virtual List<PayrollBenefit> PayrollBenefits { get; set; }
		public virtual List<Contract> Contracts { get; set; }
	}
}
