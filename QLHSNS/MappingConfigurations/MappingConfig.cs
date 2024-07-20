using AutoMapper;
using QLHSNS.DTOs.Request.Allowance;
using QLHSNS.DTOs.Request.Asset;
using QLHSNS.DTOs.Request.Benefit;
using QLHSNS.DTOs.Request.Contract;
using QLHSNS.DTOs.Request.ContractType;
using QLHSNS.DTOs.Request.Department;
using QLHSNS.DTOs.Request.EmployeeRequestDto;
using QLHSNS.DTOs.Request.HealthCareRequestDto;
using QLHSNS.DTOs.Request.JobTitle;
using QLHSNS.DTOs.Request.Payroll;
using QLHSNS.DTOs.Request.Reward;
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
using QLHSNS.DTOs.Response.Reward;
using QLHSNS.Model;

namespace QLHSNS.MappingConfigurations {
	public class MappingConfig {
		public static MapperConfiguration RegisterMaps() {
			var mappingConfig = new MapperConfiguration(config => {

				#region Domain to Response

				config.CreateMap<Reward, RewardResponseDto>();

				config.CreateMap<EmployeeFamily, EmployeeFamilyResponseDto>();

				config.CreateMap<JobTitle, JobTitleResponseDto>();

				config.CreateMap<Allowance, AllowanceResponseDto>();

				config.CreateMap<Benefit, BenefitResponseDto>();

				config.CreateMap<Department, DepartmentResponseDto>();
				config.CreateMap<Department, DepartmentBaseResponseDto>();


				config.CreateMap<Employee, EmployeeResponseDto>();
				config.CreateMap<EmployeeAsset, EmployeeAssetDto>();

				config.CreateMap<Bank, BankResponseDto>();

				config.CreateMap<BankBranch, BankBranchResponseDto>();
				config.CreateMap<BankBranch, BankBranchDto>();

				config.CreateMap<Asset, AssetResponseDto>();

				config.CreateMap<Payroll, PayrollResponseDto>();

				config.CreateMap<Contract, ContractResponseDto>();
				config.CreateMap<Attachment, AttachmentResponseDto>();

				#endregion



				#region Request to Domain

				config.CreateMap<CreateRewardRequestDto, Reward>()
					.ForMember(
						dest => dest.IsReceived,
						opt => opt.MapFrom(src => 0))
					.ForMember(
						dest => dest.CreatedAt,
						opt => opt.MapFrom(src => DateTime.Now))
					.ForMember(
						dest => dest.UpdatedAt,
						opt => opt.MapFrom(src => DateTime.Now));

				config.CreateMap<CreateJobTitleRequestDto, JobTitle>()
					.ForMember(
						dest => dest.Status,
						opt => opt.MapFrom(src => 1));

				config.CreateMap<CreateContractTypeRequestDto, ContractType>()
					.ForMember(
							dest => dest.Status,
							opt => opt.MapFrom(src => 1));

				config.CreateMap<CreateAllowanceRequestDto, Allowance>()
					.ForMember(
						dest => dest.Status,
						opt => opt.MapFrom(src => 1));

				config.CreateMap<CreateBenefitRequestDto, Benefit>()
					.ForMember(
						dest => dest.Status,
						opt => opt.MapFrom(src => 1));

				config.CreateMap<CreateDepartmentRequestDto, Department>()
					.ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1));

				config.CreateMap<CreateDepartmentJobTitleDto, DepartmentJobTitle>();

				config.CreateMap<CreateEmployeeRequestDto, Employee>();

				config.CreateMap<CreateHealthCareRequestDto, HealthCare>()
					.ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1));

				config.CreateMap<CreateAssetRequestDto, Asset>()
					.ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1));

				config.CreateMap<CreatePayrollRequestDto, Payroll>()
					.ForMember(
						dest => dest.Status,
						opt => opt.MapFrom(src => 1));

				config.CreateMap<PayrollAllowaceDto, PayrollAllowance>();
				config.CreateMap<PayrollBenefitDto, PayrollBenefit>();

				config.CreateMap<CreateContractRequestDto, Contract>()
					.ForMember(
						dest => dest.IsDeleted,
						opt => opt.MapFrom(src => 0));

				#endregion
			});

			return mappingConfig;
		}
	}
}
