using AutoMapper;
using QLHSNS.DTOs.Request.Allowance;
using QLHSNS.DTOs.Request.Asset;
using QLHSNS.DTOs.Request.Benefit;
using QLHSNS.DTOs.Request.Contract;
using QLHSNS.DTOs.Request.ContractType;
using QLHSNS.DTOs.Request.Department;
using QLHSNS.DTOs.Request.EmployeeFamily;
using QLHSNS.DTOs.Request.EmployeeRequestDto;
using QLHSNS.DTOs.Request.HealthCareRequestDto;
using QLHSNS.DTOs.Request.JobTitle;
using QLHSNS.DTOs.Request.Payroll;
using QLHSNS.DTOs.Response.Allowance;
using QLHSNS.DTOs.Response.Asset;
using QLHSNS.DTOs.Response.Bank;
using QLHSNS.DTOs.Response.BankBranch;
using QLHSNS.DTOs.Response.Benefit;
using QLHSNS.DTOs.Response.Contract;
using QLHSNS.DTOs.Response.Department;
using QLHSNS.DTOs.Response.Employee;
using QLHSNS.DTOs.Response.EmployeeFamily;
using QLHSNS.DTOs.Response.JobTitle;
using QLHSNS.DTOs.Response.Payroll;
using QLHSNS.Model;

namespace QLHSNS.MappingConfigurations {
	public class MappingConfig {
		public static MapperConfiguration RegisterMaps() {
			var mappingConfig = new MapperConfiguration(config => {
				config.CreateMap<JobTitle, CreateJobTitleRequestDto>().ReverseMap();
				config.CreateMap<JobTitle, JobTitleResponseDto>().ReverseMap();

				config.CreateMap<ContractType, CreateContractTypeRequestDto>().ReverseMap();
				config.CreateMap<ContractType, UpdateContractTypeRequestDto>().ReverseMap();

				config.CreateMap<Allowance, CreateAllowanceRequestDto>().ReverseMap();
				config.CreateMap<Allowance, AllowanceResponseDto>().ReverseMap();

				config.CreateMap<Benefit, CreateBenefitRequestDto>().ReverseMap();
				config.CreateMap<Benefit, BenefitResponseDto>().ReverseMap();

				config.CreateMap<Department, CreateDepartmentRequestDto>().ReverseMap();
				config.CreateMap<Department, DepartmentResponseDto>().ReverseMap();
				config.CreateMap<Department, DepartmentBaseResponseDto>().ReverseMap();
				config.CreateMap<DepartmentJobTitle, DepartmentJobTitleDto>().ReverseMap();

				config.CreateMap<Employee, CreateEmployeeRequestDto>().ReverseMap();
				config.CreateMap<Employee, EmployeeResponseDto>().ReverseMap();
				config.CreateMap<EmployeeAsset, EmployeeAssetDto>().ReverseMap();

				config.CreateMap<HealthCare, CreateHealthCareRequestDto>().ReverseMap();

				config.CreateMap<Bank, BankResponseDto>().ReverseMap();

				config.CreateMap<BankBranch, BankBranchResponseDto>().ReverseMap();
				config.CreateMap<BankBranch, BankBranchDto>().ReverseMap();

				config.CreateMap<Asset, AssetResponseDto>().ReverseMap();
				config.CreateMap<Asset, CreateAssetRequestDto>().ReverseMap();

				config.CreateMap<Payroll, PayrollResponseDto>().ReverseMap();
				config.CreateMap<Payroll, CreatePayrollRequestDto>().ReverseMap();
				config.CreateMap<PayrollAllowance, PayrollAllowaceRequestDto>().ReverseMap();
				config.CreateMap<PayrollBenefit, PayrollBenefitRequestDto>().ReverseMap();

				config.CreateMap<Contract, ContractResponseDto>().ReverseMap();
				config.CreateMap<Contract, CreateContractRequestDto>().ReverseMap();
				config.CreateMap<Attachment, AttachmentResponseDto>().ReverseMap();

				config.CreateMap<EmployeeFamily, EmployeeFamilyResponseDto>().ReverseMap();
			});

			return mappingConfig;
		}
	}
}
