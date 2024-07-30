using AutoMapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Common.Implementations;
using QLHSNS.Common.Interfaces;
using QLHSNS.Data;
using QLHSNS.MappingConfigurations;
using QLHSNS.Options;
using QLHSNS.Services;
using QLHSNS.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect to DB
builder.Services.AddDbContext<AppDbContext>(option => {
	option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<GmailOptions>(builder.Configuration.GetSection(GmailOptions.GmailOptionsKey));

// Config auto mapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// DI
builder.Services.AddScoped<IJobTitleService, JobTitlteService>();
builder.Services.AddScoped<IContractTypeService, ContractTypeService>();
builder.Services.AddScoped<IAllowanceService, AllowanceService>();
builder.Services.AddScoped<IBenefitService, BenefitService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IBankBranchService, BankBranchService>();
builder.Services.AddScoped<IHealthCareService, HealthCareService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IEmployeeFamilyService, EmployeeFamilyService>();
builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddScoped<IPayrollAllowanceService, PayrollAllowanceService>();
builder.Services.AddScoped<IPayrollBenefitService, PayrollBenefitService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IReminderService, ReminderService>();

// Hangfire Client
builder.Services.AddHangfire(config => config
	.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
	.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

// Hangfire Server
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();
app.UseHangfireDashboard("/hangfire");

RecurringJob.AddOrUpdate("TestHangfire", () => Console.WriteLine("Hello from hangfire"), "* * * * *");

Reminder();

app.Run();

void Reminder() {
	//using (var scope = app.Services.CreateScope()) {
	//	var reminder = scope.ServiceProvider.GetService<ReminderService>();

	RecurringJob.AddOrUpdate<IReminderService>(
		recurringJobId: "ReminderBirthdayJob",
		methodCall: x => x.BirthdayReminder(),
		cronExpression: Cron.Daily(7, 0),
		options: new RecurringJobOptions()
	);

	RecurringJob.AddOrUpdate<IReminderService>(
		recurringJobId: "ReminderEmployeeContractExpiryJob",
		methodCall: x => x.EmployeeContractExpiryReminder(),
		cronExpression: Cron.Daily(8, 0),
		options: new RecurringJobOptions()
	);
	//}
}