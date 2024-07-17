using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Constants;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Employee;
using QLHSNS.DTOs.Request.EmployeeRequestDto;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Employee;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class EmployeeService : IEmployeeService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public EmployeeService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<EmployeeAssetResponseDto>> CreateEmployeeAssetsAsync(EmployeeAssetRequestDto request) {
			try {
				var employeeFromDb = await _dbContext.Employees.Where(x => x.Id == request.EmployeeId).FirstOrDefaultAsync();
				if (employeeFromDb != null) {
					foreach (var assetId in request.AssetIds) {
						var newEmployeeAssetDto = new CreateEmployeeAssetDto {
							EmployeeId = request.EmployeeId,
							AssetId = assetId
						};
						var newEmployeeAsset = _mapper.Map<EmployeeAsset>(newEmployeeAssetDto);
						await _dbContext.EmployeeAssets.AddAsync(newEmployeeAsset);
					}

					await _dbContext.SaveChangesAsync();

					var emplyeeAssets = _dbContext.EmployeeAssets.Where(x => x.EmployeeId == request.EmployeeId);
					var assets = _dbContext.Assets.Where(x => x.Status == 1);

					var query = await (from ea in emplyeeAssets
									   join a in assets on ea.AssetId equals a.Id into temp
									   from t in temp.DefaultIfEmpty()
									   select new EmployeeAssetDto {
										   Id = t.Id,
										   Name = t.Name,
										   Description = t.Description!,
										   Price = t.Price,
										   Status = t.Status,
										   PurchaseDate = t.PurchaseDate
									   }).ToListAsync();

					var result = new EmployeeAssetResponseDto {
						Id = request.EmployeeId,
						Name = employeeFromDb.Name,
						Assets = query,
					};

					return new ApiResponse<EmployeeAssetResponseDto> {
						Data = result,
						IsSuccess = true,
						Message = "Created successfully"
					};
				}
				return new ApiResponse<EmployeeAssetResponseDto> {
					IsSuccess = false,
					Message = "Employee not found"
				};
			} catch (Exception ex) {
				return new ApiResponse<EmployeeAssetResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<EmployeeResponseDto>> CreateNewEmployeeAsync(CreateEmployeeRequestDto request) {
			try {
				if (request != null) {
					var checkEmployeeFromDb = await _dbContext.Employees.Where(x => x.Cccd == request.Cccd).FirstOrDefaultAsync();

					if (checkEmployeeFromDb != null) {
						return new ApiResponse<EmployeeResponseDto> {
							IsSuccess = false,
							Message = "Employee already exist"
						};
					}

					var newEmployee = _mapper.Map<Employee>(request);

					await _dbContext.Employees.AddAsync(newEmployee);
					await _dbContext.SaveChangesAsync();

					if (newEmployee != null) {
						var queryBankBranch = await (from bb in _dbContext.BankBranches.Where(x => x.Id == newEmployee.BankBranchId && x.Status == 1)
													 join b in _dbContext.Banks.Where(x => x.Status == 1) on bb.BankId equals b.Id
													 select new EmployeeBankBranchDto {
														 Id = bb.Id,
														 BranchName = bb.BranchName,
														 Address = bb.Address,
														 PhoneNumber = bb.PhoneNumber,
														 Bank = new EmployeeBankDto {
															 Id = b.Id,
															 BankName = b.BankName,
														 }
													 }).FirstAsync();

						var queryAddress = await _dbContext.Locations.Where(x => x.Id == newEmployee.LocationId)
																	.Select(x => new EmployeeCVDto {
																		Id = x.Id,
																		Country = x.Country,
																		District = x.District,
																		Province = x.Province,
																		Ward = x.Ward,
																	}).FirstAsync();

						var queryHealthCare = await _dbContext.HealthCares.Where(x => x.Id == newEmployee.HealthCareId && x.Status == 1)
																			.Select(x => new EmployeeHealthCareDto {
																				Id = x.Id,
																				Name = x.Name,
																				Address = x.Address,
																				Email = x.Email,
																				PhoneNumber = x.PhoneNumber,
																			}).FirstAsync();

						var queryDepartment = await _dbContext.Departments.Where(x => x.Id == newEmployee.DepartmentId && x.Status == 1)
																			.Select(x => new EmployeeDepartmentDto {
																				Id = x.Id,
																				Name = x.Name,
																			}).FirstAsync();

						var queryJobTitle = await _dbContext.JobTitles.Where(x => x.Id == newEmployee.JobTitleId && x.Status == 1)
																		.Select(x => new EmployeeJobTitleDto {
																			Id = x.Id,
																			Name = x.JobTitleName
																		}).FirstAsync();

						var result = _mapper.Map<EmployeeResponseDto>(newEmployee);

						result.JobTitle = queryJobTitle;
						result.BankBranch = queryBankBranch;
						result.Address = queryAddress;
						result.HealthCare = queryHealthCare;
						result.Department = queryDepartment;

						return new ApiResponse<EmployeeResponseDto> {
							Data = result,
							IsSuccess = true,
							Message = "Created successfully"
						};
					}
				}
				return new ApiResponse<EmployeeResponseDto> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<EmployeeResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<EmployeeResponseDto>> DeleteEmployeeAsync(Guid id) {
			try {
				var employeeFromDb = await _dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (employeeFromDb == null) {
					return new ApiResponse<EmployeeResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				var employeeAssetsToRemove = await _dbContext.EmployeeAssets.Where(x => x.EmployeeId == id).ToListAsync();
				var employeeFamilyToRemove = await _dbContext.EmployeeFamilies.Where(x => x.EmployeeId == id).ToListAsync();

				foreach (var item in employeeAssetsToRemove) {
					_dbContext.EmployeeAssets.Remove(item);
				}

				foreach (var item in employeeFamilyToRemove) {
					_dbContext.EmployeeFamilies.Remove(item);
				}

				_dbContext.Employees.Remove(employeeFromDb);

				return new ApiResponse<EmployeeResponseDto> {
					IsSuccess = true,
					Message = "Deleted successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<EmployeeResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<EmployeeAssetResponseDto>> GetAssetByEmployeeId(Guid id) {
			try {
				var employeeFromDb = await _dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (employeeFromDb != null) {
					var employeeAssets = _dbContext.EmployeeAssets.Where(x => x.EmployeeId == id);
					var assets = _dbContext.Assets.Where(x => x.Status == 1);

					var query = await (from ea in employeeAssets
									   join a in assets on ea.AssetId equals a.Id into temp
									   from t in temp.DefaultIfEmpty()
									   select new EmployeeAssetDto {
										   Id = t.Id,
										   Name = t.Name,
										   Description = t.Description!,
										   Price = t.Price,
										   Status = t.Status,
										   PurchaseDate = t.PurchaseDate
									   }).ToListAsync();

					var result = new EmployeeAssetResponseDto {
						Id = id,
						Name = employeeFromDb.Name,
						Assets = query,
					};

					return new ApiResponse<EmployeeAssetResponseDto> {
						Data = result,
						IsSuccess = true
					};
				}
				return new ApiResponse<EmployeeAssetResponseDto> {
					IsSuccess = false,
					Message = "Employee not found"
				};
			} catch (Exception ex) {
				return new ApiResponse<EmployeeAssetResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByIdAsync(Guid id) {
			try {
				var employeeFromDb = await _dbContext.Employees.Where(x => x.Id == id)
							//.Include(x => x.BankBranch).ThenInclude(x => x.Bank)
							//.Include(x => x.Department).Include(x => x.JobTitle)
							//.Include(x => x.HealthCare).Include(x => x.Location)
							.FirstOrDefaultAsync();

				if (employeeFromDb == null) {
					return new ApiResponse<EmployeeResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				var queryBankBranch = await (from bb in _dbContext.BankBranches.Where(x => x.Id == employeeFromDb.BankBranchId && x.Status == 1)
											 join b in _dbContext.Banks.Where(x => x.Status == 1) on bb.BankId equals b.Id
											 select new EmployeeBankBranchDto {
												 Id = bb.Id,
												 BranchName = bb.BranchName,
												 Address = bb.Address,
												 PhoneNumber = bb.PhoneNumber,
												 Bank = new EmployeeBankDto {
													 Id = b.Id,
													 BankName = b.BankName,
												 }
											 }).FirstAsync();

				var queryAddress = await (from ad in _dbContext.Locations.Where(x => x.Id == employeeFromDb.LocationId)
										  select new EmployeeCVDto {
											  Id = ad.Id,
											  Country = ad.Country,
											  District = ad.District,
											  Province = ad.Province,
											  Ward = ad.Ward,
										  }).FirstAsync();

				var queryHealthCare = await (from hc in _dbContext.HealthCares.Where(x => x.Id == employeeFromDb.HealthCareId && x.Status == 1)
											 select new EmployeeHealthCareDto {
												 Id = hc.Id,
												 Name = hc.Name,
												 Address = hc.Address,
												 Email = hc.Email,
												 PhoneNumber = hc.PhoneNumber,
											 }).FirstAsync();

				var queryDepartment = await (from d in _dbContext.Departments.Where(x => x.Id == employeeFromDb.DepartmentId && x.Status == 1)
											 select new EmployeeDepartmentDto {
												 Id = d.Id,
												 Name = d.Name,
											 }).FirstAsync();

				var queryJobTitle = await (from jt in _dbContext.JobTitles.Where(x => x.Id == employeeFromDb.JobTitleId && x.Status == 1)
										   select new EmployeeJobTitleDto {
											   Id = jt.Id,
											   Name = jt.JobTitleName,
										   }).FirstAsync();

				var result = _mapper.Map<EmployeeResponseDto>(employeeFromDb);

				result.BankBranch = queryBankBranch;
				result.Address = queryAddress;
				result.HealthCare = queryHealthCare;
				result.Department = queryDepartment;
				result.JobTitle = queryJobTitle;

				return new ApiResponse<EmployeeResponseDto> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<EmployeeResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PagedResult<EmployeeResponseDto>>> GetEmployeesAsync(PagingRequestBase request) {
			try {
				if (request != null) {
					var employees = await _dbContext.Employees.Skip((request.PageNumber - 1) * request.PageSize)
															.Take(request.PageSize).ToListAsync();

					if (employees == null || employees.Count == 0) {
						return new ApiResponse<PagedResult<EmployeeResponseDto>> {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					int totalRecord = await _dbContext.Employees.CountAsync();

					var result = new List<EmployeeResponseDto>();

					foreach (var employee in employees) {
						var queryBankBranch = await (from bb in _dbContext.BankBranches.Where(x => x.Id == employee.BankBranchId && x.Status == 1)
													 join b in _dbContext.Banks.Where(x => x.Status == 1) on bb.BankId equals b.Id
													 select new EmployeeBankBranchDto {
														 Id = bb.Id,
														 BranchName = bb.BranchName,
														 Address = bb.Address,
														 PhoneNumber = bb.PhoneNumber,
														 Bank = new EmployeeBankDto {
															 Id = b.Id,
															 BankName = b.BankName,
														 }
													 }).FirstAsync();

						var queryAddress = await (from ad in _dbContext.Locations.Where(x => x.Id == employee.LocationId)
												  select new EmployeeCVDto {
													  Id = ad.Id,
													  Country = ad.Country,
													  District = ad.District,
													  Province = ad.Province,
													  Ward = ad.Ward,
												  }).FirstAsync();

						var queryHealthCare = await (from hc in _dbContext.HealthCares.Where(x => x.Id == employee.HealthCareId && x.Status == 1)
													 select new EmployeeHealthCareDto {
														 Id = hc.Id,
														 Name = hc.Name,
														 Address = hc.Address,
														 Email = hc.Email,
														 PhoneNumber = hc.PhoneNumber,
													 }).FirstAsync();

						var queryDepartment = await (from d in _dbContext.Departments.Where(x => x.Id == employee.DepartmentId && x.Status == 1)
													 select new EmployeeDepartmentDto {
														 Id = d.Id,
														 Name = d.Name,
													 }).FirstAsync();

						var queryJobTitle = await (from jt in _dbContext.JobTitles.Where(x => x.Id == employee.JobTitleId && x.Status == 1)
												   select new EmployeeJobTitleDto {
													   Id = jt.Id,
													   Name = jt.JobTitleName,
												   }).FirstAsync();

						var item = _mapper.Map<EmployeeResponseDto>(employee);

						item.BankBranch = queryBankBranch;
						item.Address = queryAddress;
						item.HealthCare = queryHealthCare;
						item.Department = queryDepartment;
						item.JobTitle = queryJobTitle;

						result.Add(item);
					}

					return new ApiResponse<PagedResult<EmployeeResponseDto>> {
						Data = new PagedResult<EmployeeResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
						IsSuccess = true,
					};
				}
				return new ApiResponse<PagedResult<EmployeeResponseDto>> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<EmployeeResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PagedResult<EmployeeResponseDto>>> SearchAllEmployeeAsync(EmployeePagingRequestDto request) {
			try {
				if (request != null) {
					var query = new List<Employee>();
					query = await _dbContext.Employees.Include(x => x.Department)
													  .Include(x => x.JobTitle)
													  .Include(x => x.HealthCare)
													  .Include(x => x.Location)
													  .Include(x => x.BankBranch)
													  .ThenInclude(x => x.Bank).ToListAsync();

					// Filter
					if (!string.IsNullOrWhiteSpace(request.Keyword?.Trim())) {
						query = query.Where(x => x.Name.Trim().ToLower().Contains(request.Keyword.Trim().ToLower())
											|| x.Cccd.Trim().ToLower().Contains(request.Keyword.Trim().ToLower())
											|| x.Email.Trim().ToLower().Contains(request.Keyword.Trim().ToLower())).ToList();
					}
					if (!string.IsNullOrWhiteSpace(request.Cccd)) {
						query = query.Where(x => x.Cccd.Trim().ToLower().Contains(request.Cccd.Trim().ToLower())).ToList();
					}

					if (!string.IsNullOrWhiteSpace(request.Email)) {
						query = query.Where(x => x.Email.Trim().ToLower().Contains(request.Email.Trim().ToLower())).ToList();
					}

					if (!string.IsNullOrWhiteSpace(request.PhoneNumber)) {
						query = query.Where(x => x.PhoneNumber.Trim().ToLower().Contains(request.PhoneNumber.Trim().ToLower())).ToList();
					}

					if (!string.IsNullOrWhiteSpace(request.BankNumber)) {
						query = query.Where(x => x.BankNumber.Trim().ToLower().Contains(request.BankNumber.Trim().ToLower())).ToList();
					}

					if (request.Gender != -1) {
						query = query.Where(x => x.Gender == request.Gender).ToList();
					}

					if (request.DepartmentId.HasValue) {
						query = query.Where(x => x.DepartmentId == request.DepartmentId.Value).ToList();
					}

					if (request.JobTitleId.HasValue) {
						query = query.Where(x => x.JobTitleId == request.JobTitleId.Value).ToList();
					}

					if (request.HealthCareId.HasValue) {
						query = query.Where(x => x.HealthCareId == request.HealthCareId.Value).ToList();
					}

					if (request.BankBranchId.HasValue) {
						query = query.Where(x => x.BankBranchId == request.BankBranchId.Value).ToList();
					}

					if (request.LocationId.HasValue) {
						query = query.Where(x => x.LocationId == request.LocationId.Value).ToList();
					}

					// Sort
					if (!string.IsNullOrWhiteSpace(request.SortField)) {
						// Sort by Job Title
						if (request.SortField == OrderByField.SORT_BY_JOBTITLE) {
							if (request.SortOrder == OrderByField.ASC) {
								query = query.OrderBy(x => x.JobTitle.JobTitleName).ToList();
							}
							if (request.SortOrder == OrderByField.DESC) {
								query = query.OrderByDescending(x => x.JobTitle.JobTitleName).ToList();
							}
						}
						// Sort by Department
						else if (request.SortField == OrderByField.SORT_BY_DEPARTMENT) {
							if (request.SortOrder == OrderByField.ASC) {
								query = query.OrderBy(x => x.Department?.Name).ToList();
							}
							if (request.SortOrder == OrderByField.DESC) {
								query = query.OrderByDescending(x => x.Department?.Name).ToList();
							}
						}
						// Sort by Bank
						else if (request.SortField == OrderByField.SORT_BY_BANK) {
							if (request.SortOrder == OrderByField.ASC) {
								query = query.OrderBy(x => x.BankBranch?.Bank.BankName).ToList();
							}
							if (request.SortOrder == OrderByField.DESC) {
								query = query.OrderByDescending(x => x.BankBranch?.Bank.BankName).ToList();
							}
						}
						// Sort by Employee Name
						else {
							if (request.SortOrder == OrderByField.ASC) {
								query = query.OrderBy(x => x.Name).ToList();
							}
							if (request.SortOrder == OrderByField.DESC) {
								query = query.OrderByDescending(x => x.Name).ToList();
							}
						}
					}

					int totalRecord = query.Count();
					var data = query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
					var result = _mapper.Map<List<EmployeeResponseDto>>(data).ToList();

					return new ApiResponse<PagedResult<EmployeeResponseDto>> {
						Data = new PagedResult<EmployeeResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
						IsSuccess = true,
					};
				}

				return new ApiResponse<PagedResult<EmployeeResponseDto>> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<EmployeeResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<EmployeeResponseDto>> UpdateEmployeeAsyns(UpdateEmployeeRequestDto request) {
			try {
				if (request != null) {
					var employeeFromDb = await _dbContext.Employees.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

					if (employeeFromDb == null) {
						return new ApiResponse<EmployeeResponseDto> {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					employeeFromDb.UpdatedAt = DateTime.Now;
					employeeFromDb.Name = request.Name;
					employeeFromDb.Cccd = request.Cccd;
					employeeFromDb.BankNumber = request.BankNumber;
					employeeFromDb.PhoneNumber = request.PhoneNumber;
					employeeFromDb.DateOfBirth = request.DateOfBirth;
					employeeFromDb.Email = request.Email;
					employeeFromDb.Gender = request.Gender;

					if (request.DepartmentId != null) {
						employeeFromDb.DepartmentId = request.DepartmentId.Value;
					}

					if (request.JobTitleId != null) {
						employeeFromDb.JobTitleId = request.JobTitleId.Value;
					}

					if (request.HealthCareId != null) {
						employeeFromDb.HealthCareId = request.HealthCareId.Value;
					}

					if (request.LocationId != null) {
						employeeFromDb.LocationId = request.LocationId.Value;
					}

					if (request.BankBranchId != null) {
						employeeFromDb.BankBranchId = request.BankBranchId.Value;
					}

					await _dbContext.SaveChangesAsync();

					var result = _mapper.Map<EmployeeResponseDto>(employeeFromDb);

					var queryBankBranch = await (from bb in _dbContext.BankBranches.Where(x => x.Id == employeeFromDb.BankBranchId && x.Status == 1)
												 join b in _dbContext.Banks.Where(x => x.Status == 1) on bb.BankId equals b.Id
												 select new EmployeeBankBranchDto {
													 Id = bb.Id,
													 BranchName = bb.BranchName,
													 Address = bb.Address,
													 PhoneNumber = bb.PhoneNumber,
													 Bank = new EmployeeBankDto {
														 Id = b.Id,
														 BankName = b.BankName,
													 }
												 }).FirstAsync();

					var queryAddress = await (from ad in _dbContext.Locations.Where(x => x.Id == employeeFromDb.LocationId)
											  select new EmployeeCVDto {
												  Id = ad.Id,
												  Country = ad.Country,
												  District = ad.District,
												  Province = ad.Province,
												  Ward = ad.Ward,
											  }).FirstAsync();

					var queryHealthCare = await (from hc in _dbContext.HealthCares.Where(x => x.Id == employeeFromDb.HealthCareId && x.Status == 1)
												 select new EmployeeHealthCareDto {
													 Id = hc.Id,
													 Name = hc.Name,
													 Address = hc.Address,
													 Email = hc.Email,
													 PhoneNumber = hc.PhoneNumber,
												 }).FirstAsync();

					var queryDepartment = await (from d in _dbContext.Departments.Where(x => x.Id == employeeFromDb.DepartmentId && x.Status == 1)
												 select new EmployeeDepartmentDto {
													 Id = d.Id,
													 Name = d.Name,
												 }).FirstAsync();

					var queryJobTitle = await (from jt in _dbContext.JobTitles.Where(x => x.Id == employeeFromDb.JobTitleId && x.Status == 1)
											   select new EmployeeJobTitleDto {
												   Id = jt.Id,
												   Name = jt.JobTitleName,
											   }).FirstAsync();

					result.JobTitle = queryJobTitle;
					result.BankBranch = queryBankBranch;
					result.Address = queryAddress;
					result.HealthCare = queryHealthCare;
					result.Department = queryDepartment;

					return new ApiResponse<EmployeeResponseDto> {
						Data = result,
						IsSuccess = true,
						Message = "Updated successuly",
					};
				}

				return new ApiResponse<EmployeeResponseDto> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<EmployeeResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
