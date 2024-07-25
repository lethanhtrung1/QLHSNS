using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Constants;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Department;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Department;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class DepartmentService : IDepartmentService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public DepartmentService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<List<DepartmentJobTitleResponseDto>>> AddDepartmentJobTitleAsync(DepartmentJobTitleRequestDto request) {
			try {
				if (request != null) {
					if (request.JobTitleIds != null && request.JobTitleIds.Count > 0) {
						foreach (var item in request.JobTitleIds) {
							var dataDto = new CreateDepartmentJobTitleDto {
								DepartmentId = request.DepartmentId,
								JobTitleId = item
							};
							var data = _mapper.Map<DepartmentJobTitle>(dataDto);
							await _dbContext.DepartmentJobTitles.AddAsync(data);
						}
						await _dbContext.SaveChangesAsync();

						var result = await _dbContext.DepartmentJobTitles.Include(x => x.JobTitle)
												.Where(x => x.DepartmentId == request.DepartmentId && x.JobTitle.Status == 1)
												.Select(x => new DepartmentJobTitleResponseDto {
													Id = x.Id,
													Name = x.JobTitle.JobTitleName,
												})
												.ToListAsync();

						return new ApiResponse<List<DepartmentJobTitleResponseDto>> {
							Data = result,
							IsSuccess = true,
							Message = "Added successfully"
						};
					}
				}
				return new ApiResponse<List<DepartmentJobTitleResponseDto>> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<List<DepartmentJobTitleResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<DepartmentResponseDto>> CreateDepartmentAsync(CreateDepartmentRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.Departments
						.Where(x => x.Name.ToLower() == request.Name.ToLower()).FirstOrDefaultAsync();

					if (dataFromDb != null) {
						return new ApiResponse<DepartmentResponseDto> {
							IsSuccess = false,
							Message = "Department already exist"
						};
					}

					var newDepartment = _mapper.Map<Department>(request);
					await _dbContext.Departments.AddAsync(newDepartment);

					if (request.JobTitleIds != null && request.JobTitleIds.Count > 0) {
						foreach (var item in request.JobTitleIds) {
							var dataDto = new CreateDepartmentJobTitleDto {
								DepartmentId = newDepartment.Id,
								JobTitleId = item
							};
							var data = _mapper.Map<DepartmentJobTitle>(dataDto);
							await _dbContext.DepartmentJobTitles.AddAsync(data);
						}
					}

					await _dbContext.SaveChangesAsync();

					var departments = await _dbContext.Departments.Where(x => x.Id == newDepartment.Id && x.Status == 1).FirstOrDefaultAsync();
					var departmentJobTitles = await _dbContext.DepartmentJobTitles.Include(x => x.JobTitle)
												.Where(x => x.DepartmentId == newDepartment.Id && x.JobTitle.Status == 1)
												.Select(x => new DepartmentJobTitleResponseDto {
													Id = x.Id,
													Name = x.JobTitle.JobTitleName,
												})
												.ToListAsync();

					var result = _mapper.Map<DepartmentResponseDto>(departments);
					result.JobTitles = departmentJobTitles;

					return new ApiResponse<DepartmentResponseDto>() {
						Data = result,
						IsSuccess = true,
						Message = "Created successfully"
					};
				}
				return new ApiResponse<DepartmentResponseDto> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<DepartmentResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<bool> DeleteDepartmentAsync(Guid id) {
			var dataFromDb = await _dbContext.Departments.Where(x => x.Id == id).FirstOrDefaultAsync();

			if (dataFromDb == null) return false;

			var departmentJobTitle = await _dbContext.DepartmentJobTitles.Where(x => x.DepartmentId == id).ToListAsync();

			foreach (var item in departmentJobTitle) {
				_dbContext.DepartmentJobTitles.Remove(item);
			}

			_dbContext.Departments.Remove(dataFromDb);

			await _dbContext.SaveChangesAsync();

			return true;
		}

		public async Task<ApiResponse<DepartmentResponseDto>> DisableDepartmentAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Departments.Where(x => x.Status == 1 && x.Id == id).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<DepartmentResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				dataFromDb.Status = 0;
				dataFromDb.UpdatedAt = DateTime.Now;
				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.Departments.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();
				var result = _mapper.Map<DepartmentResponseDto>(query);

				return new ApiResponse<DepartmentResponseDto>() {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<DepartmentResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<DepartmentResponseDto>> EnableDepartmentAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Departments.Where(x => x.Id == id && x.Status == 0)
															 .FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<DepartmentResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				dataFromDb.Status = 1;
				dataFromDb.UpdatedAt = DateTime.Now;
				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.Departments.Where(x => x.Id == id && x.Status == 1)
														.FirstOrDefaultAsync();
				var result = _mapper.Map<DepartmentResponseDto>(query);

				return new ApiResponse<DepartmentResponseDto>() {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<DepartmentResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<DepartmentBaseResponseDto>>> GetAllAsync(int status) {
			try {
				var data = new List<Department>();
				if (status == FilterStatus.Active || status == FilterStatus.NonActive) {
					data = await _dbContext.Departments.Where(x => x.Status == status).ToListAsync();
				} else if (status == FilterStatus.All) {
					data = await _dbContext.Departments.ToListAsync();
				} else {
					return new ApiResponse<List<DepartmentBaseResponseDto>> {
						IsSuccess = false,
						Message = Message.INVALID_PAYLOAD
					};
				}

				if (data == null || data.Count == 0) {
					return new ApiResponse<List<DepartmentBaseResponseDto>> {
						IsSuccess = false,
						Message = "No data"
					};
				}

				var result = _mapper.Map<List<DepartmentBaseResponseDto>>(data);

				return new ApiResponse<List<DepartmentBaseResponseDto>> {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<List<DepartmentBaseResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<DepartmentResponseDto>> GetByIdAsync(Guid id) {
			try {
				var data = await _dbContext.Departments.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (data == null) {
					return new ApiResponse<DepartmentResponseDto>() {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				var departmentJobTitles = await _dbContext.DepartmentJobTitles.Include(x => x.JobTitle)
												.Where(x => x.DepartmentId == id && x.JobTitle.Status == 1)
												.Select(x => new DepartmentJobTitleResponseDto {
													Id = x.JobTitleId,
													Name = x.JobTitle.JobTitleName,
												})
												.ToListAsync();

				var result = _mapper.Map<DepartmentResponseDto>(data);
				result.JobTitles = departmentJobTitles;

				return new ApiResponse<DepartmentResponseDto>() {
					Data = result,
					IsSuccess = true,
				};
			} catch (Exception ex) {
				return new ApiResponse<DepartmentResponseDto>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<PagedResult<DepartmentResponseDto>>> GetDepartmentsAsync(PagingRequestBase request) {
			try {
				if (request != null) {
					var data = await _dbContext.Departments
									.Skip((request.PageNumber - 1) * request.PageSize)
									.Take(request.PageSize).ToListAsync();

					if (data == null || data.Count == 0) {
						return new ApiResponse<PagedResult<DepartmentResponseDto>>() {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					int totalRecord = await _dbContext.Departments.CountAsync();
					//var result = _mapper.Map<List<DepartmentResponseDto>>(data);

					var result = new List<DepartmentResponseDto>();

					foreach (var item in data) {
						var departmentJobTitles = await _dbContext.DepartmentJobTitles.Include(x => x.JobTitle)
												.Where(x => x.DepartmentId == item.Id && x.JobTitle.Status == 1)
												.Select(x => new DepartmentJobTitleResponseDto {
													Id = x.JobTitleId,
													Name = x.JobTitle.JobTitleName,
												})
												.ToListAsync();

						var departmentDto = _mapper.Map<DepartmentResponseDto>(item);
						departmentDto.JobTitles = departmentJobTitles;
						result.Add(departmentDto);
					}

					return new ApiResponse<PagedResult<DepartmentResponseDto>>() {
						Data = new PagedResult<DepartmentResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
						IsSuccess = true,
					};
				}
				return new ApiResponse<PagedResult<DepartmentResponseDto>>() {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<DepartmentResponseDto>>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<DepartmentResponseDto>> UpdateDepartmentAsync(UpdateDepartmentRequestDto request) {
			try {
				if (request != null) {
					var departments = await _dbContext.Departments.Include(x => x.DepartmentJobTitles)
										.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

					if (departments == null) {
						return new ApiResponse<DepartmentResponseDto> {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					departments.Name = request.Name;
					departments.UpdatedAt = DateTime.Now;

					if (request.JobTitleIds != null && request.JobTitleIds.Count > 0) {
						var currentJobTitleIds = departments.DepartmentJobTitles.Select(x => x.JobTitleId).ToList();
						var jobTitlesToRemove = currentJobTitleIds.Except(request.JobTitleIds).ToList();
						var jobTitlesToAdd = request.JobTitleIds.Except(currentJobTitleIds).ToList();

						foreach (var item in jobTitlesToRemove) {
							var departmentJobTitle = departments.DepartmentJobTitles.FirstOrDefault(x => x.JobTitleId == item);
							if (departmentJobTitle != null) {
								_dbContext.DepartmentJobTitles.Remove(departmentJobTitle);
							}
						}

						foreach (var item in jobTitlesToAdd) {
							var newDepartmentJobTitleDto = new CreateDepartmentJobTitleDto {
								DepartmentId = request.Id,
								JobTitleId = item
							};
							var newDepartmentJobTitle = _mapper.Map<DepartmentJobTitle>(newDepartmentJobTitleDto);
							await _dbContext.DepartmentJobTitles.AddAsync(newDepartmentJobTitle);
						}
					}

					await _dbContext.SaveChangesAsync();

					var departmentFromDb = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == request.Id);
					var departmentJobTitles = await _dbContext.DepartmentJobTitles.Include(x => x.JobTitle)
												.Where(x => x.DepartmentId == request.Id && x.JobTitle.Status == 1)
												.Select(x => new DepartmentJobTitleResponseDto {
													Id = x.Id,
													Name = x.JobTitle.JobTitleName,
												})
												.ToListAsync();

					var result = _mapper.Map<DepartmentResponseDto>(departmentFromDb);
					result.JobTitles = departmentJobTitles;

					return new ApiResponse<DepartmentResponseDto>() {
						Data = result,
						IsSuccess = true,
						Message = "Updated successfully"
					};
				}

				return new ApiResponse<DepartmentResponseDto> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<DepartmentResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<DepartmentJobTitleResponseDto>>> UpdateDepartmentJobTitleAsync(DepartmentJobTitleRequestDto request) {
			try {
				if (request != null) {
					var departmentJobTitles = _dbContext.DepartmentJobTitles
												.Where(x => x.DepartmentId == request.DepartmentId);

					var currentJobTitleIds = departmentJobTitles.Select(x => x.JobTitleId).ToList();

					var jobTitlesToRemove = currentJobTitleIds.Except(request.JobTitleIds).ToList();
					var jobTitlesToAdd = request.JobTitleIds.Except(currentJobTitleIds).ToList();

					foreach (var item in jobTitlesToRemove) {
						var departmentJobTitle = departmentJobTitles.FirstOrDefault(x => x.JobTitleId == item);
						if (departmentJobTitle != null) {
							_dbContext.DepartmentJobTitles.Remove(departmentJobTitle);
						}
					}

					foreach (var item in jobTitlesToAdd) {
						var newDepartmentJobTitleDto = new CreateDepartmentJobTitleDto {
							DepartmentId = request.DepartmentId,
							JobTitleId = item
						};
						var newDepartmentJobTitle = _mapper.Map<DepartmentJobTitle>(newDepartmentJobTitleDto);
						await _dbContext.DepartmentJobTitles.AddAsync(newDepartmentJobTitle);
					}

					await _dbContext.SaveChangesAsync();

					var query = _dbContext.DepartmentJobTitles.Where(x => x.Id == request.DepartmentId);
					var jobTitles = _dbContext.JobTitles.Where(x => x.Status == 1);
					var result = await (from q in query
										join j in jobTitles on q.JobTitleId equals j.Id into temp
										from t in temp.DefaultIfEmpty()
										select new DepartmentJobTitleResponseDto {
											Id = t.Id,
											Name = t.JobTitleName
										}).ToListAsync();

					return new ApiResponse<List<DepartmentJobTitleResponseDto>> {
						Data = result,
						IsSuccess = true,
						Message = "Updated successfully"
					};
				}

				return new ApiResponse<List<DepartmentJobTitleResponseDto>> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<List<DepartmentJobTitleResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
