﻿using AutoMapper;
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
using QLHSNS.DTOs.Request.OverTime;
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
using QLHSNS.DTOs.Response.OverTime;
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


				config.CreateMap<Employee, EmployeeResponseDto>()
					.ForMember(
						dest => dest.DateOfBirth,
						opt => opt.MapFrom(src => src.DateOfBirth.Date));
				config.CreateMap<EmployeeAsset, EmployeeAssetDto>();

				config.CreateMap<Bank, BankResponseDto>();

				config.CreateMap<BankBranch, BankBranchResponseDto>()
					.ForMember(
						dest => dest.BankId,
						opt => opt.MapFrom(src => src.Bank.Id))
					.ForMember(
						dest => dest.BankName,
						opt => opt.MapFrom(src => src.Bank.BankName));

				config.CreateMap<BankBranch, BankBranchDto>()
					.ForMember(
						dest => dest.BankName,
						opt => opt.MapFrom(src => src.Bank.BankName));

				config.CreateMap<Asset, AssetResponseDto>()
					.ForMember(
						dest => dest.Status,
						opt => opt.MapFrom(src => src.Status));

				config.CreateMap<Payroll, PayrollResponseDto>();

				config.CreateMap<Contract, ContractResponseDto>();

				config.CreateMap<Attachment, AttachmentResponseDto>();

				config.CreateMap<EmployeeFamilyDetail, EmployeeFamilyDetailResponseDto>();

				config.CreateMap<EmployeeFamily, GetEmployeeFamilyWithDetailResponseDto>()
					.ForMember(
						dest => dest.FamilyDetails,
						opt => opt.MapFrom(src => new List<EmployeeFamilyDetailResponseDto>()));

				config.CreateMap<ContractType, ContractTypeDto>()
					.ForMember(
						dest => dest.Name,
						opt => opt.MapFrom(src => src.ContractTypeName));

				config.CreateMap<OverTime, OverTimeResponse>();

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

				config.CreateMap<CreateEmployeeRequestDto, Employee>()
					.ForMember(
						dest => dest.IsWorking,
						opt => opt.MapFrom(src => 1))
					.ForMember(
						dest => dest.DateOfBirth,
						opt => opt.MapFrom(src => src.DateOfBirth.Date))
					.ForMember(
						dest => dest.DateOfJoining,
						opt => opt.MapFrom(src => DateTime.Now.Date));

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

				config.CreateMap<AddEmployeeFamilyDetailRequestDto, EmployeeFamilyDetail>()
					.ForMember(
						dest => dest.DateOfBirth,
						opt => opt.MapFrom(src => src.DateOfBirth.Date));

				config.CreateMap<CreateOverTimeRequestDto, OverTime>();

				#endregion
			});

			return mappingConfig;
		}
	}
}
