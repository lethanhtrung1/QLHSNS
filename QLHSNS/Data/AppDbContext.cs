using Microsoft.EntityFrameworkCore;
using QLHSNS.Model;

namespace QLHSNS.Data {
	public class AppDbContext : DbContext {
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		#region DbSet

		public DbSet<JobTitle> JobTitles { get; set; }
		public DbSet<ContractType> ContractTypes { get; set; }
		public DbSet<Allowance> Allowances { get; set; }
		public DbSet<Benefit> Benefits { get; set; }
		public DbSet<Bank> Banks { get; set; }
		public DbSet<BankBranch> BankBranches { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<DepartmentJobTitle> DepartmentJobTitles { get; set; }
		public DbSet<Location> Locations { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Contract> Contracts { get; set; }
		public DbSet<Payroll> Payrolls { get; set; }
		public DbSet<PayrollAllowance> PayrollAllowances { get; set; }
		public DbSet<PayrollBenefit> PayrollBenefits { get; set; }
		public DbSet<OverTime> OverTimes { get; set; }
		public DbSet<HealthCare> HealthCares { get; set; }
		public DbSet<Asset> Assets { get; set; }
		public DbSet<EmployeeAsset> EmployeeAssets { get; set; }
		public DbSet<EmployeeFamily> EmployeeFamilies { get; set; }
		public DbSet<EmployeeFamilyDetail> EmployeeFamilyDetails { get; set; }
		public DbSet<Attachment> Attachments { get; set; }
		public DbSet<Reward> Rewards { get; set; }

		#endregion DbSet

		//protected override void OnModelCreating(ModelBuilder modelBuilder) {
		//	base.OnModelCreating(modelBuilder);

		//	modelBuilder.Entity<JobTitle>().HasData(
		//			new JobTitle { Id = 1, JobTitleName = "Giám đốc trung tâm", Description = "", Status = 1 },
		//			new JobTitle { Id = 2, JobTitleName = "Phó giám đốc trung tâm", Description = "", Status = 1 },
		//			new JobTitle { Id = 3, JobTitleName = "Leader", Description = "", Status = 1 },
		//			new JobTitle { Id = 4, JobTitleName = "Nhân viên", Description = "", Status = 1 }
		//		);

		//	modelBuilder.Entity<ConstractType>().HasData(
		//			new ConstractType { Id = 1, ConstractTypeName = "Hợp đồng đào tạo", Description = "", Status = 1 },
		//			new ConstractType { Id = 2, ConstractTypeName = "Hợp đồng học viên", Description = "", Status = 1 },
		//			new ConstractType { Id = 3, ConstractTypeName = "Hợp đồng thử việc", Description = "", Status = 1 },
		//			new ConstractType { Id = 4, ConstractTypeName = "Hợp đồng chính thức", Description = "", Status = 1 },
		//			new ConstractType { Id = 5, ConstractTypeName = "Hợp đồng cộng tác viên", Description = "", Status = 1 }
		//		);

		//	modelBuilder.Entity<Allowance>().HasData(
		//			new Allowance { Id = 1, AllowanceName = "Phụ cấp gửi xe", Description = "", Status = 1, Value = 300000, Unit = "Month" },
		//			new Allowance { Id = 2, AllowanceName = "Phụ cấp OT", Description = "", Status = 1, Value = 100000, Unit = "Hour" },
		//			new Allowance { Id = 3, AllowanceName = "Phụ cấp đào tạo", Description = "", Status = 1, Value = 2000000, Unit = "Month" }
		//		);
		//}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<DepartmentJobTitle>(entity => {
				entity.HasOne(d => d.Department)
					.WithMany(p => p.DepartmentJobTitles)
					.HasForeignKey(d => d.DepartmentId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.JobTitle)
					.WithMany(p => p.DepartmentJobTitles)
					.HasForeignKey(d => d.JobTitleId)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<PayrollAllowance>(entity => {
				entity.HasOne(d => d.Payroll)
					.WithMany(p => p.PayrollAllowances)
					.HasForeignKey(d => d.PayrollId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.Allowance)
					.WithMany(p => p.PayrollAllowances)
					.HasForeignKey(d => d.AllowanceId)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<PayrollBenefit>(entity => {
				entity.HasOne(d => d.Payroll)
					.WithMany(p => p.PayrollBenefits)
					.HasForeignKey(d => d.PayrollId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.Benefit)
					.WithMany(p => p.PayrollBenefits)
					.HasForeignKey(d => d.BenefitId)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<BankBranch>(entity => {
				entity.HasOne(d => d.Bank)
					.WithMany(p => p.BankBranches)
					.HasForeignKey(d => d.BankId)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<EmployeeAsset>(entity => {
				entity.HasOne(d => d.Asset)
					.WithMany(p => p.EmployeeAssets)
					.HasForeignKey(d => d.AssetId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.Employee)
					.WithMany(p => p.EmployeeAssets)
					.HasForeignKey(d => d.EmployeeId)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<EmployeeFamily>(entity => {
				entity.HasOne(d => d.Employee)
					.WithOne(p => p.EmployeeFamily)
					.HasForeignKey<EmployeeFamily>(d => d.EmployeeId)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<EmployeeFamilyDetail>(entity => {
				entity.HasOne(d => d.EmployeeFamily)
					.WithMany(p => p.EmployeeFamilyDetails)
					.HasForeignKey(d => d.EmployeeFamilyId)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Employee>(entity => {
				entity.HasOne(d => d.Department)
					.WithMany(p => p.Employees)
					.HasForeignKey(x => x.DepartmentId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.JobTitle)
					.WithMany(p => p.Employees)
					.HasForeignKey(d => d.JobTitleId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.BankBranch)
					.WithMany()
					.HasForeignKey(d => d.BankBranchId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.Location)
					.WithMany()
					.HasForeignKey(d => d.LocationId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.HealthCare)
					.WithMany()
					.HasForeignKey(d => d.HealthCareId)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Contract>(entity => {
				entity.HasOne(d => d.ContractType)
					.WithMany()
					.HasForeignKey(d => d.ContractTypeId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.Employee)
					.WithMany(p => p.Contracts)
					.HasForeignKey(d => d.EmployeeId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.Payroll)
					.WithMany(p => p.Contracts)
					.HasForeignKey(d => d.PayrollId)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<Reward>(entity => {
				entity.HasOne(d => d.Employee)
					.WithMany(p => p.Rewards)
					.HasForeignKey(d => d.EmployeeId)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<OverTime>(entity => {
				entity.HasOne(d => d.Employee)
					.WithMany(p => p.OverTimes)
					.HasForeignKey(d => d.EmployeeId)
					.OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}
