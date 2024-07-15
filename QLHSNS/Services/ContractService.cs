using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Contract;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Contract;
using QLHSNS.DTOs.Response.Payroll;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class ContractService : IContractService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public ContractService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<ContractResponseDto>> CreateContractAsync(CreateContractRequestDto request) {
			try {
				if (request != null) {
					var checkContract = await _dbContext.Contracts.Where(x => x.EmployeeId == request.EmployeeId && x.IsDeleted == 0)
																  .FirstOrDefaultAsync();

					if (checkContract != null) {
						return new ApiResponse<ContractResponseDto> {
							IsSuccess = false,
							Message = "Contract already exist"
						};
					}

					var checkPayroll = await _dbContext.Payrolls.Where(x => x.Id == request.PayrollId && x.Status == 1).FirstOrDefaultAsync();
					var checkEmployee = await _dbContext.Employees.Where(x => x.Id == request.EmployeeId).FirstOrDefaultAsync();

					if (checkPayroll == null || checkEmployee == null) {
						return new ApiResponse<ContractResponseDto> {
							IsSuccess = false,
							Message = "Employee or Payroll does not exist in Database"
						};
					}

					var newContract = _mapper.Map<Contract>(request);

					// add new contract to db
					await _dbContext.Contracts.AddAsync(newContract);
					await _dbContext.SaveChangesAsync();

					// Get new contract from db
					var contractType = await _dbContext.ContractTypes.Where(x => x.Id == newContract.ContractTypeId && x.Status == 1)
					.Select(x => new ContractTypeDto {
						Id = x.Id,
						Name = x.ContractTypeName
					}).FirstOrDefaultAsync();

					var employee = await (from e in _dbContext.Employees.Where(x => x.Id == newContract.EmployeeId)
										  join d in _dbContext.Departments.Where(x => x.Status == 1) on e.DepartmentId equals d.Id
										  join j in _dbContext.JobTitles.Where(x => x.Status == 1) on e.JobTitleId equals j.Id
										  join b in _dbContext.BankBranches.Where(x => x.Status == 1) on e.BankBranchId equals b.Id
										  select new ContractEmployeeDto {
											  Id = newContract.EmployeeId,
											  Name = e.Name,
											  DateOfBirth = e.DateOfBirth,
											  Cccd = e.Cccd,
											  Gender = e.Gender,
											  Email = e.Email != null ? e.Email : "",
											  PhoneNumber = e.PhoneNumber != null ? e.PhoneNumber : "",
											  BankNumber = e.BankNumber,
											  Department = d.Name,
											  JobTitle = j.JobTitleName,
											  Bank = b.BranchName
										  }).FirstOrDefaultAsync();

					var payroll = await _dbContext.Payrolls.Where(x => x.Id == newContract.PayrollId && x.Status == 1)
						.Select(x => new ContractPayrollDto {
							Id = x.Id,
							BasicSalary = x.BasicSalary,
							SalaryCoefficient = x.SalaryCoefficient,
							Notes = x.Notes
						}).FirstOrDefaultAsync();

					var totalSalary = payroll != null ? payroll.BasicSalary * (decimal)payroll.SalaryCoefficient : 0;

					var contractAllowances = await (from p in _dbContext.Payrolls.Where(x => x.Id == newContract.PayrollId && x.Status == 1)
													join pa in _dbContext.PayrollAllowances on p.Id equals pa.PayrollId
													join a in _dbContext.Allowances.Where(x => x.Status == 1) on pa.AllowanceId equals a.Id into temp
													from t in temp.DefaultIfEmpty()
													select new PayrollAllowanceResponseDto {
														Id = t.Id,
														Name = t.AllowanceName,
														Value = t.Value,
														Unit = t.Unit,
													}).ToListAsync();

					var contractBenefits = await (from p in _dbContext.Payrolls.Where(x => x.Id == newContract.PayrollId && x.Status == 1)
												  join pb in _dbContext.PayrollBenefits on p.Id equals pb.PayrollId
												  join b in _dbContext.Benefits.Where(x => x.Status == 1) on pb.BenefitId equals b.Id into temp
												  from t in temp.DefaultIfEmpty()
												  select new PayrollBenefitResponseDto {
													  Id = t.Id,
													  Name = t.BenefitName,
													  Amount = t.Amount,
													  Description = t.Description,
												  }).ToListAsync();

					if (contractAllowances != null && contractAllowances.Count > 0) {
						foreach (var item in contractAllowances) {
							// if Allowance is not OT
							if (_dbContext.Allowances.Where(x => x.Id == item.Id && x.Status == 1).Select(x => x.Unit).FirstOrDefault() == "Month") {
								totalSalary += await _dbContext.Allowances.Where(x => x.Id == item.Id && x.Status == 1)
																		  .Select(x => x.Value).FirstOrDefaultAsync();
							}
						}
					}

					if (contractBenefits != null && contractBenefits.Count > 0) {
						foreach (var item in contractBenefits) {
							totalSalary += await _dbContext.Benefits.Where(x => x.Id == item.Id && x.Status == 1)
																	.Select(x => x.Amount).FirstOrDefaultAsync();
						}
					}

					payroll!.Bonus = new BonusDto {
						Allowances = contractAllowances ?? new List<PayrollAllowanceResponseDto>(),
						Benefits = contractBenefits ?? new List<PayrollBenefitResponseDto>(),
					};

					payroll.TotalSalary = totalSalary;

					var result = _mapper.Map<ContractResponseDto>(newContract);

					result.ContractType = contractType!;
					result.Employee = employee!;
					result.Payroll = payroll;

					return new ApiResponse<ContractResponseDto> {
						Data = result,
						IsSuccess = true,
						Message = "Contract created successully"
					};
				}

				return new ApiResponse<ContractResponseDto>() {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<ContractResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<ContractResponseDto>> DeleteAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Contracts.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<ContractResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				_dbContext.Contracts.Remove(dataFromDb);
				await _dbContext.SaveChangesAsync();

				return new ApiResponse<ContractResponseDto> {
					IsSuccess = true,
					Message = "Deleted succesully"
				};
			} catch (Exception ex) {
				return new ApiResponse<ContractResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<ContractResponseDto>> GetContractByEmployeeIdAsync(Guid id) {
			try {
				var checkEmployee = await _dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (checkEmployee == null) {
					return new ApiResponse<ContractResponseDto> {
						IsSuccess = false,
						Message = "Employee not found"
					};
				}

				var contractFromDb = await _dbContext.Contracts.Where(x => x.EmployeeId == id && x.IsDeleted == 0).FirstOrDefaultAsync();

				if (contractFromDb == null) {
					return new ApiResponse<ContractResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				var contractType = await _dbContext.ContractTypes.Where(x => x.Id == contractFromDb.ContractTypeId && x.Status == 1)
					.Select(x => new ContractTypeDto {
						Id = x.Id,
						Name = x.ContractTypeName
					}).FirstOrDefaultAsync();

				var employee = (from e in _dbContext.Employees.Where(x => x.Id == contractFromDb.EmployeeId)
								join d in _dbContext.Departments.Where(x => x.Status == 1) on e.DepartmentId equals d.Id
								join j in _dbContext.JobTitles.Where(x => x.Status == 1) on e.JobTitleId equals j.Id
								join b in _dbContext.BankBranches.Where(x => x.Status == 1) on e.BankBranchId equals b.Id
								select new ContractEmployeeDto {
									Id = contractFromDb.EmployeeId,
									Name = e.Name,
									DateOfBirth = e.DateOfBirth,
									Cccd = e.Cccd,
									Gender = e.Gender,
									Email = e.Email != null ? e.Email : "",
									PhoneNumber = e.PhoneNumber != null ? e.PhoneNumber : "",
									BankNumber = e.BankNumber,
									Department = d.Name,
									JobTitle = j.JobTitleName,
									Bank = b.BranchName
								}).FirstOrDefault();

				var payroll = await _dbContext.Payrolls.Where(x => x.Id == contractFromDb.PayrollId && x.Status == 1)
					.Select(x => new ContractPayrollDto {
						Id = x.Id,
						BasicSalary = x.BasicSalary,
						SalaryCoefficient = x.SalaryCoefficient,
						Notes = x.Notes
					}).FirstOrDefaultAsync();

				var totalSalary = payroll != null ? payroll.BasicSalary * (decimal)payroll.SalaryCoefficient : 0;

				var contractAllowances = (from p in _dbContext.Payrolls.Where(x => x.Id == contractFromDb.PayrollId && x.Status == 1)
										  join pa in _dbContext.PayrollAllowances on p.Id equals pa.PayrollId
										  join a in _dbContext.Allowances.Where(x => x.Status == 1) on pa.AllowanceId equals a.Id into temp
										  from t in temp.DefaultIfEmpty()
										  select new PayrollAllowanceResponseDto {
											  Id = t.Id,
											  Name = t.AllowanceName,
											  Value = t.Value,
											  Unit = t.Unit,
										  }).ToList();

				var contractBenefits = (from p in _dbContext.Payrolls.Where(x => x.Id == contractFromDb.PayrollId && x.Status == 1)
										join pb in _dbContext.PayrollBenefits on p.Id equals pb.PayrollId
										join b in _dbContext.Benefits.Where(x => x.Status == 1) on pb.BenefitId equals b.Id into temp
										from t in temp.DefaultIfEmpty()
										select new PayrollBenefitResponseDto {
											Id = t.Id,
											Name = t.BenefitName,
											Amount = t.Amount,
											Description = t.Description,
										}).ToList();

				if (contractAllowances != null && contractAllowances.Count > 0) {
					foreach (var item in contractAllowances) {
						// if Allowance is not OT
						if (_dbContext.Allowances.Where(x => x.Id == item.Id && x.Status == 1).Select(x => x.Unit).FirstOrDefault() == "Month") {
							totalSalary += await _dbContext.Allowances.Where(x => x.Id == item.Id && x.Status == 1)
																	  .Select(x => x.Value).FirstOrDefaultAsync();
						}
					}
				}

				if (contractBenefits != null && contractBenefits.Count > 0) {
					foreach (var item in contractBenefits) {
						totalSalary += await _dbContext.Benefits.Where(x => x.Id == item.Id && x.Status == 1)
																.Select(x => x.Amount).FirstOrDefaultAsync();
					}
				}

				payroll!.Bonus = new BonusDto {
					Allowances = contractAllowances ?? new List<PayrollAllowanceResponseDto>(),
					Benefits = contractBenefits ?? new List<PayrollBenefitResponseDto>(),
				};

				payroll.TotalSalary = totalSalary;

				var result = _mapper.Map<ContractResponseDto>(contractFromDb);

				result.ContractType = contractType!;
				result.Employee = employee!;
				result.Payroll = payroll;

				return new ApiResponse<ContractResponseDto> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<ContractResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<ContractResponseDto>> GetContractByIdAsync(Guid id) {
			try {
				var contractFromDb = await _dbContext.Contracts.Where(x => x.Id == id && x.IsDeleted == 0).FirstOrDefaultAsync();

				if (contractFromDb == null) {
					return new ApiResponse<ContractResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				var contractType = await _dbContext.ContractTypes.Where(x => x.Id == contractFromDb.ContractTypeId && x.Status == 1)
					.Select(x => new ContractTypeDto {
						Id = x.Id,
						Name = x.ContractTypeName
					}).FirstOrDefaultAsync();

				var employee = await (from e in _dbContext.Employees.Where(x => x.Id == contractFromDb.EmployeeId)
									  join d in _dbContext.Departments.Where(x => x.Status == 1) on e.DepartmentId equals d.Id
									  join j in _dbContext.JobTitles.Where(x => x.Status == 1) on e.JobTitleId equals j.Id
									  join b in _dbContext.BankBranches.Where(x => x.Status == 1) on e.BankBranchId equals b.Id
									  select new ContractEmployeeDto {
										  Id = contractFromDb.EmployeeId,
										  Name = e.Name,
										  DateOfBirth = e.DateOfBirth,
										  Cccd = e.Cccd,
										  Gender = e.Gender,
										  Email = e.Email != null ? e.Email : "",
										  PhoneNumber = e.PhoneNumber != null ? e.PhoneNumber : "",
										  BankNumber = e.BankNumber,
										  Department = d.Name,
										  JobTitle = j.JobTitleName,
										  Bank = b.BranchName
									  }).FirstOrDefaultAsync();

				var payroll = await _dbContext.Payrolls.Where(x => x.Id == contractFromDb.PayrollId && x.Status == 1)
					.Select(x => new ContractPayrollDto {
						Id = x.Id,
						BasicSalary = x.BasicSalary,
						SalaryCoefficient = x.SalaryCoefficient,
						Notes = x.Notes
					}).FirstOrDefaultAsync();

				var totalSalary = payroll != null ? payroll.BasicSalary * (decimal)payroll.SalaryCoefficient : 0;

				var contractAllowances = await (from p in _dbContext.Payrolls.Where(x => x.Id == contractFromDb.PayrollId && x.Status == 1)
												join pa in _dbContext.PayrollAllowances on p.Id equals pa.PayrollId
												join a in _dbContext.Allowances.Where(x => x.Status == 1) on pa.AllowanceId equals a.Id into temp
												from t in temp.DefaultIfEmpty()
												select new PayrollAllowanceResponseDto {
													Id = t.Id,
													Name = t.AllowanceName,
													Value = t.Value,
													Unit = t.Unit,
												}).ToListAsync();

				var contractBenefits = await (from p in _dbContext.Payrolls.Where(x => x.Id == contractFromDb.PayrollId && x.Status == 1)
											  join pb in _dbContext.PayrollBenefits on p.Id equals pb.PayrollId
											  join b in _dbContext.Benefits.Where(x => x.Status == 1) on pb.BenefitId equals b.Id into temp
											  from t in temp.DefaultIfEmpty()
											  select new PayrollBenefitResponseDto {
												  Id = t.Id,
												  Name = t.BenefitName,
												  Amount = t.Amount,
												  Description = t.Description,
											  }).ToListAsync();

				if (contractAllowances != null && contractAllowances.Count > 0) {
					foreach (var item in contractAllowances) {
						// if Allowance is not OT
						if (_dbContext.Allowances.Where(x => x.Id == item.Id && x.Status == 1).Select(x => x.Unit).FirstOrDefault() == "Month") {
							totalSalary += await _dbContext.Allowances.Where(x => x.Id == item.Id && x.Status == 1)
																	  .Select(x => x.Value).FirstOrDefaultAsync();
						}
					}
				}

				if (contractBenefits != null && contractBenefits.Count > 0) {
					foreach (var item in contractBenefits) {
						totalSalary += await _dbContext.Benefits.Where(x => x.Id == item.Id && x.Status == 1)
																.Select(x => x.Amount).FirstOrDefaultAsync();
					}
				}

				payroll!.Bonus = new BonusDto {
					Allowances = contractAllowances ?? new List<PayrollAllowanceResponseDto>(),
					Benefits = contractBenefits ?? new List<PayrollBenefitResponseDto>(),
				};

				payroll.TotalSalary = totalSalary;

				var result = _mapper.Map<ContractResponseDto>(contractFromDb);

				result.ContractType = contractType!;
				result.Employee = employee!;
				result.Payroll = payroll;

				return new ApiResponse<ContractResponseDto> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<ContractResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PagedResult<ContractResponseDto>>> GetContractsAsync(PagingRequestBase request) {
			try {
				if (request != null) {
					var data = await _dbContext.Contracts.Where(x => x.IsDeleted == 0)
								.Skip((request.PageNumber - 1) * request.PageSize)
								.Take(request.PageSize).ToListAsync();

					int totalRecord = await _dbContext.Contracts.Where(x => x.IsDeleted == 0).CountAsync();
					var result = new List<ContractResponseDto>();

					foreach (var contract in data) {
						var contractType = await _dbContext.ContractTypes.Where(x => x.Id == contract.ContractTypeId && x.Status == 1)
							.Select(x => new ContractTypeDto {
								Id = x.Id,
								Name = x.ContractTypeName
							}).FirstOrDefaultAsync();

						var employee = await (from e in _dbContext.Employees.Where(x => x.Id == contract.EmployeeId)
											  join d in _dbContext.Departments.Where(x => x.Status == 1) on e.DepartmentId equals d.Id
											  join j in _dbContext.JobTitles.Where(x => x.Status == 1) on e.JobTitleId equals j.Id
											  join b in _dbContext.BankBranches.Where(x => x.Status == 1) on e.BankBranchId equals b.Id
											  select new ContractEmployeeDto {
												  Id = contract.EmployeeId,
												  Name = e.Name,
												  DateOfBirth = e.DateOfBirth,
												  Cccd = e.Cccd,
												  Gender = e.Gender,
												  Email = e.Email != null ? e.Email : "",
												  PhoneNumber = e.PhoneNumber != null ? e.PhoneNumber : "",
												  BankNumber = e.BankNumber,
												  Department = d.Name,
												  JobTitle = j.JobTitleName,
												  Bank = b.BranchName
											  }).FirstOrDefaultAsync();

						var payroll = await _dbContext.Payrolls.Where(x => x.Id == contract.PayrollId && x.Status == 1)
							.Select(x => new ContractPayrollDto {
								Id = x.Id,
								BasicSalary = x.BasicSalary,
								SalaryCoefficient = x.SalaryCoefficient,
								Notes = x.Notes
							}).FirstOrDefaultAsync();

						var totalSalary = payroll != null ? payroll.BasicSalary * (decimal)payroll.SalaryCoefficient : 0;

						var contractAllowances = await (from p in _dbContext.Payrolls.Where(x => x.Id == contract.PayrollId && x.Status == 1)
														join pa in _dbContext.PayrollAllowances on p.Id equals pa.PayrollId
														join a in _dbContext.Allowances.Where(x => x.Status == 1) on pa.AllowanceId equals a.Id into temp
														from t in temp.DefaultIfEmpty()
														select new PayrollAllowanceResponseDto {
															Id = t.Id,
															Name = t.AllowanceName,
															Value = t.Value,
															Unit = t.Unit,
														}).ToListAsync();

						var contractBenefits = await (from p in _dbContext.Payrolls.Where(x => x.Id == contract.PayrollId && x.Status == 1)
													  join pb in _dbContext.PayrollBenefits on p.Id equals pb.PayrollId
													  join b in _dbContext.Benefits.Where(x => x.Status == 1) on pb.BenefitId equals b.Id into temp
													  from t in temp.DefaultIfEmpty()
													  select new PayrollBenefitResponseDto {
														  Id = t.Id,
														  Name = t.BenefitName,
														  Amount = t.Amount,
														  Description = t.Description,
													  }).ToListAsync();

						if (contractAllowances != null && contractAllowances.Count > 0) {
							foreach (var item in contractAllowances) {
								// if Allowance is not OT
								if (_dbContext.Allowances.Where(x => x.Id == item.Id && x.Status == 1).Select(x => x.Unit).FirstOrDefault() == "Month") {
									totalSalary += await _dbContext.Allowances.Where(x => x.Id == item.Id && x.Status == 1)
																			  .Select(x => x.Value).FirstOrDefaultAsync();
								}
							}
						}

						if (contractBenefits != null && contractBenefits.Count > 0) {
							foreach (var item in contractBenefits) {
								totalSalary += await _dbContext.Benefits.Where(x => x.Id == item.Id && x.Status == 1)
																		.Select(x => x.Amount).FirstOrDefaultAsync();
							}
						}

						payroll!.Bonus = new BonusDto {
							Allowances = contractAllowances ?? new List<PayrollAllowanceResponseDto>(),
							Benefits = contractBenefits ?? new List<PayrollBenefitResponseDto>(),
						};

						payroll.TotalSalary = totalSalary;

						var contractDto = _mapper.Map<ContractResponseDto>(contract);

						contractDto.ContractType = contractType!;
						contractDto.Employee = employee!;
						contractDto.Payroll = payroll;

						result.Add(contractDto);
					}

					return new ApiResponse<PagedResult<ContractResponseDto>> {
						IsSuccess = true,
						Data = new PagedResult<ContractResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
					};
				}

				return new ApiResponse<PagedResult<ContractResponseDto>> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception) {

				throw;
			}
		}

		public async Task<ApiResponse<SoftDeleteContractResponseDto>> SoftDeleteAsync(Guid id) {
			try {
				var contractFromDb = await _dbContext.Contracts.Where(x => x.Id == id && x.IsDeleted == 0).FirstOrDefaultAsync();

				if(contractFromDb == null) {
					return new ApiResponse<SoftDeleteContractResponseDto> {
						IsSuccess = false,
						Message = "Contract not found"
					};
				}

				contractFromDb.IsDeleted = 1;
				contractFromDb.UpdatedAt = DateTime.Now;
				await _dbContext.SaveChangesAsync();

				var result = new SoftDeleteContractResponseDto {
					Id = contractFromDb.Id,
					IsDeleted = contractFromDb.IsDeleted,
					CreatedAt = contractFromDb.CreatedAt,
					UpdatedAt = contractFromDb.UpdatedAt
				};

				return new ApiResponse<SoftDeleteContractResponseDto> {
					Data = result,
					IsSuccess = true,
					Message = "Deleted successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<SoftDeleteContractResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public Task<ApiResponse<ContractResponseDto>> UpdateContractAsync(UpdateContractRequestDto request) {
			throw new NotImplementedException();
		}

		public Task<IActionResult> UploadFile(Guid id, IFormFile file) {
			throw new NotImplementedException();
		}

		public Task<IActionResult> DownloadFile(Guid id) {
			throw new NotImplementedException();
		}

		public Task<ApiResponse<PagedResult<ContractResponseDto>>> Filter(FilterContractRequestDto request) {
			throw new NotImplementedException();
		}
	}
}
