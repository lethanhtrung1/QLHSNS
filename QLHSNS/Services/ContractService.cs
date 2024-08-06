using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Constants;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Contract;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Contract;
using QLHSNS.DTOs.Response.Payroll;
using QLHSNS.Model;
using QLHSNS.Services.IServices;
using System.IO.Compression;

namespace QLHSNS.Services {
	public class ContractService : IContractService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;

		public ContractService(AppDbContext dbContext, IMapper mapper, IWebHostEnvironment env) {
			_dbContext = dbContext;
			_mapper = mapper;
			_env = env;
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
					var contractType = await _dbContext.ContractTypes.Where(x => x.Id == newContract.ContractTypeId && x.Status == 1).FirstOrDefaultAsync();

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

					var newContractFromDb = await _dbContext.Contracts.Where(x => x.Id == newContract.Id).FirstOrDefaultAsync();
					var result = _mapper.Map<ContractResponseDto>(newContractFromDb);

					result.ContractTypeResponse = _mapper.Map<ContractTypeDto>(contractType);
					result.EmployeeResponse = employee!;
					result.PayrollResponse = payroll;

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

		public async Task<bool> DeleteAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Contracts.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return false;
				}

				_dbContext.Contracts.Remove(dataFromDb);
				await _dbContext.SaveChangesAsync();

				return true;
			} catch (Exception) {
				throw;
			}
		}

		public async Task<ApiResponse<ContractResponseDto>> GetContractByEmployeeIdAsync(Guid id) {
			try {
				var checkEmployee = await _dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (checkEmployee == null) {
					return new ApiResponse<ContractResponseDto> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND
					};
				}

				var contractFromDb = await _dbContext.Contracts.Where(x => x.EmployeeId == id && x.IsDeleted == 0).FirstOrDefaultAsync();

				if (contractFromDb == null) {
					return new ApiResponse<ContractResponseDto> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND
					};
				}

				var result = await GetContractInfo(contractFromDb);

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
						Message = Message.DATA_NOT_FOUND
					};
				}

				var result = await GetContractInfo(contractFromDb);

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
						var contractDto = await GetContractInfo(contract);

						result.Add(contractDto);
					}

					return new ApiResponse<PagedResult<ContractResponseDto>> {
						IsSuccess = true,
						Data = new PagedResult<ContractResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
					};
				}

				return new ApiResponse<PagedResult<ContractResponseDto>> {
					IsSuccess = false,
					Message = Message.INVALID_PAYLOAD
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<ContractResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<SoftDeleteContractResponseDto>> SoftDeleteAsync(Guid id) {
			try {
				var contractFromDb = await _dbContext.Contracts.Where(x => x.Id == id && x.IsDeleted == 0).FirstOrDefaultAsync();

				if (contractFromDb == null) {
					return new ApiResponse<SoftDeleteContractResponseDto> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND
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

		public async Task<ApiResponse<ContractResponseDto>> UpdateContractAsync(UpdateContractRequestDto request) {
			try {
				if (request != null) {
					var contractFromDb = await _dbContext.Contracts.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

					if (contractFromDb == null) {
						return new ApiResponse<ContractResponseDto> {
							IsSuccess = false,
							Message = Message.DATA_NOT_FOUND
						};
					}

					if (request.PayrollId.HasValue) {
						contractFromDb.PayrollId = request.PayrollId.Value;
					}

					if (request.EmployeeId.HasValue) {
						contractFromDb.PayrollId = request.EmployeeId.Value;
					}

					if (request.ContractTypeId.HasValue) {
						contractFromDb.PayrollId = request.ContractTypeId.Value;
					}

					await _dbContext.SaveChangesAsync();

					// Get data after update
					var contractType = await _dbContext.ContractTypes.Where(x => x.Id == contractFromDb.ContractTypeId && x.Status == 1)
																	.FirstOrDefaultAsync();

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

					// Allowance bonus
					if (contractAllowances != null && contractAllowances.Count > 0) {
						foreach (var item in contractAllowances) {
							// if Allowance is not OT
							if (_dbContext.Allowances.Where(x => x.Id == item.Id && x.Status == 1).Select(x => x.Unit).FirstOrDefault() == "Month") {
								totalSalary += await _dbContext.Allowances.Where(x => x.Id == item.Id && x.Status == 1)
																		  .Select(x => x.Value).FirstOrDefaultAsync();
							}
						}
					}

					// Benefit bonus
					if (contractBenefits != null && contractBenefits.Count > 0) {
						foreach (var item in contractBenefits) {
							totalSalary += await _dbContext.Benefits.Where(x => x.Id == item.Id && x.Status == 1)
																	.Select(x => x.Amount).FirstOrDefaultAsync();
						}
					}

					// Total Bonus
					payroll!.Bonus = new BonusDto {
						Allowances = contractAllowances ?? new List<PayrollAllowanceResponseDto>(),
						Benefits = contractBenefits ?? new List<PayrollBenefitResponseDto>(),
					};

					payroll.TotalSalary = totalSalary;

					var result = _mapper.Map<ContractResponseDto>(contractFromDb);

					result.ContractTypeResponse = _mapper.Map<ContractTypeDto>(contractType);
					result.EmployeeResponse = employee!;
					result.PayrollResponse = payroll;

					return new ApiResponse<ContractResponseDto> {
						Data = result,
						IsSuccess = true,
						Message = Message.UPDATED_SUCCESS
					};
				}

				return new ApiResponse<ContractResponseDto> {
					IsSuccess = false,
					Message = Message.INVALID_PAYLOAD
				};
			} catch (Exception ex) {
				return new ApiResponse<ContractResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<string>>> UploadFilesAsync(Guid id, List<IFormFile> files) {
			try {
				var contractFromDb = await _dbContext.Contracts.Where(x => x.Id == id && x.IsDeleted == 0).FirstOrDefaultAsync();

				if (contractFromDb == null) {
					return new ApiResponse<List<string>> {
						IsSuccess = false,
						Message = "Contract not found",
					};
				}

				string wwwroot = _env.WebRootPath;

				foreach (var file in files) {
					var fileName = Path.GetRandomFileName() + DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss") + "_" + file.FileName;

					//var extension = Path.GetExtension(fileName);
					var filePath = Path.Combine(Directory.GetCurrentDirectory(), wwwroot + "\\Upload\\ContractFile");

					if (!Directory.Exists(filePath)) {
						Directory.CreateDirectory(filePath);
					}

					var folder = Path.Combine("\\Upload\\ContractFile", fileName);
					var completePath = Path.Combine(Directory.GetCurrentDirectory(), wwwroot + "\\Upload\\ContractFile", fileName);

					using (var stream = new FileStream(completePath, FileMode.Create)) {
						await file.CopyToAsync(stream);
					}

					var data = new Attachment {
						Id = Guid.NewGuid(),
						ContractId = id,
						FileName = fileName,
						FilePath = folder,
						UploadDate = DateTime.Now
					};

					await _dbContext.Attachments.AddAsync(data);
				}

				await _dbContext.SaveChangesAsync();

				return new ApiResponse<List<string>> {
					IsSuccess = true,
					Message = "Upload successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<List<string>> {
					IsSuccess = true,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<FileResponseDto>> DownloadFile(Guid id) {
			try {
				var filePaths = await _dbContext.Attachments.Where(x => x.ContractId == id).Select(x => x.FilePath).ToListAsync();

				if (filePaths == null || filePaths.Count == 0) {
					return new ApiResponse<FileResponseDto> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND
					};
				}

				var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

				var files = new List<string>();

				foreach (var filePath in filePaths) {
					var file = Directory.GetFiles(Path.Combine(_env.WebRootPath, filePath)).ToList();
					files.AddRange(file);
				}

				using (var memoryStream = new MemoryStream()) {
					using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true)) {
						foreach (var file in files) {
							var theFile = archive.CreateEntry(file);
							using (var streamWrite = new StreamWriter(theFile.Open())) {
								streamWrite.Write(File.ReadAllText(file));
							}
						}
					}

					var result = new FileResponseDto {
						FileType = "application/zip",
						ArchiveData = memoryStream.ToArray(),
						ArchiveName = zipName
					};

					return new ApiResponse<FileResponseDto> {
						IsSuccess = true,
						Data = result,
						Message = "Pass"
					};
				}
			} catch (Exception ex) {
				return new ApiResponse<FileResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<AttachmentResponseDto>>> GetAllFileByContractIdAsync(Guid id) {
			try {
				var data = await _dbContext.Attachments.Where(x => x.ContractId == id).ToListAsync();

				if (data == null || data.Count == 0) {
					return new ApiResponse<List<AttachmentResponseDto>> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND
					};
				}

				var result = _mapper.Map<List<AttachmentResponseDto>>(data);

				var wwwroot = _env.WebRootPath;

				foreach (var item in result) {
					item.FilePath = wwwroot + item.FilePath;
				}

				return new ApiResponse<List<AttachmentResponseDto>> {
					Data = result,
					IsSuccess = true,
				};
			} catch (Exception ex) {
				return new ApiResponse<List<AttachmentResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PagedResult<ContractResponseDto>>> FilterAsync(FilterContractRequestDto request) {
			try {
				if (request != null) {
					var query = await _dbContext.Contracts.Include(x => x.ContractType).Where(x => x.IsDeleted == request.IsDeleted).ToListAsync();

					if (request.ContractTypeId.HasValue) {
						query = query.Where(x => x.ContractTypeId == request.ContractTypeId).ToList();
					}

					if (request.StartDate != null) {
						query = query.Where(x => x.StartDate >= request.StartDate).ToList();
					}

					if (request.EndDate != null) {
						query = query.Where(x => x.EndDate >= request.EndDate).ToList();
					}

					if (!string.IsNullOrWhiteSpace(request.SortField)) {
						if (request.SortField == OrderByField.SORT_BY_CONTRACT_TYPE) {
							if (request.SortOrder == OrderByField.ASC) {
								query = query.OrderBy(x => x.ContractType.ContractTypeName).ToList();
							}
							if (request.SortOrder == OrderByField.DESC) {
								query = query.OrderByDescending(x => x.ContractType.ContractTypeName).ToList();
							}
						} else if (request.SortField == OrderByField.SORT_BY_START_DATE) {
							if (request.SortOrder == OrderByField.ASC) {
								query = query.OrderBy(x => x.StartDate).ToList();
							}
							if (request.SortOrder == OrderByField.DESC) {
								query = query.OrderByDescending(x => x.StartDate).ToList();
							}
						} else {
							if (request.SortOrder == OrderByField.ASC) {
								query = query.OrderBy(x => x.EndDate).ToList();
							}
							if (request.SortOrder == OrderByField.DESC) {
								query = query.OrderByDescending(x => x.EndDate).ToList();
							}
						}
					}

					int totalRecord = query.Count();
					var data = query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

					var result = new List<ContractResponseDto>();

					foreach (var contract in data) {
						var contractDto = await GetContractInfo(contract);

						result.Add(contractDto);
					}

					return new ApiResponse<PagedResult<ContractResponseDto>> {
						Data = new PagedResult<ContractResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
						IsSuccess = true,
					};
				}
				return new ApiResponse<PagedResult<ContractResponseDto>> {
					IsSuccess = false,
					Message = Message.INVALID_PAYLOAD
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<ContractResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		private async Task<ContractResponseDto> GetContractInfo(Contract contract) {
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

			contractDto.ContractTypeResponse = contractType!;
			contractDto.EmployeeResponse = employee!;
			contractDto.PayrollResponse = payroll;

			return contractDto;
		}
	}
}
