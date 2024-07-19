using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.JobTitle;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.JobTitle;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class JobTitlteService : IJobTitleService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public JobTitlteService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<JobTitleResponseDto>> CreateAsync(CreateJobTitleRequestDto jobTitle) {
			try {
				if (jobTitle != null) {
					var jobTitleFromDb = await _dbContext.JobTitles
						.Where(x => x.JobTitleName.ToLower() == jobTitle.JobTitleName.ToLower())
						.FirstOrDefaultAsync();

					if (jobTitleFromDb != null) {
						return new ApiResponse<JobTitleResponseDto>() {
							IsSuccess = false,
							Message = "Job Title already exits",
						};
					}

					var data = _mapper.Map<JobTitle>(jobTitle);
					await _dbContext.JobTitles.AddAsync(data);
					await _dbContext.SaveChangesAsync();

					var query = await _dbContext.JobTitles.Where(x => x.Id == data.Id).FirstOrDefaultAsync();
					var result = _mapper.Map<JobTitleResponseDto>(query);

					return new ApiResponse<JobTitleResponseDto>() {
						Data = result,
						IsSuccess = true,
						Message = "Created successfully",
					};
				}
				return new ApiResponse<JobTitleResponseDto>() {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<JobTitleResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<bool> DeleteAsync(Guid id) {
			try {
				var data = await _dbContext.JobTitles.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (data == null) return false;

				_dbContext.JobTitles.Remove(data);
				await _dbContext.SaveChangesAsync();

				return true;
			} catch (Exception) {
				throw;
			}
		}

		public async Task<ApiResponse<JobTitleResponseDto>> DisableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.JobTitles.FirstOrDefaultAsync(x => x.Id == id && x.Status == 1);

				if (dataFromDb == null) {
					return new ApiResponse<JobTitleResponseDto> {
						IsSuccess = false,
						Message = "Not Found"
					};
				}

				dataFromDb.Status = 0;
				dataFromDb.UpdatedAt = DateTime.Now;

				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.JobTitles.FirstOrDefaultAsync(x => x.Id == id && x.Status == 0);
				var result = _mapper.Map<JobTitleResponseDto>(query);

				return new ApiResponse<JobTitleResponseDto>() {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<JobTitleResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<JobTitleResponseDto>> EnableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.JobTitles.FirstOrDefaultAsync(x => x.Id == id && x.Status == 0);

				if (dataFromDb == null) {
					return new ApiResponse<JobTitleResponseDto> {
						IsSuccess = false,
						Message = "Not Found"
					};
				}

				dataFromDb.Status = 1;
				dataFromDb.UpdatedAt = DateTime.Now;

				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.JobTitles.FirstOrDefaultAsync(x => x.Id == id && x.Status == 1);
				var result = _mapper.Map<JobTitleResponseDto>(query);

				return new ApiResponse<JobTitleResponseDto>() {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<JobTitleResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PagedResult<JobTitleResponseDto>>> GetAllAsync(PagingRequestBase request) {
			try {
				var data = await _dbContext.JobTitles.Where(x=>x.Status == 1)
							.Skip((request.PageNumber - 1) * request.PageSize)
							.Take(request.PageSize).ToListAsync();
				
				if (data.Count == 0) {
					return new ApiResponse<PagedResult<JobTitleResponseDto>> {
						IsSuccess = false,
						Message = "Not Found",
					};
				}

				int totalRecord = await _dbContext.JobTitles.Where(x => x.Status == 1).CountAsync();
				var result = _mapper.Map<List<JobTitleResponseDto>>(data);

				return new ApiResponse<PagedResult<JobTitleResponseDto>> {
					Data = new PagedResult<JobTitleResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
					IsSuccess = true,
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<JobTitleResponseDto>> {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<List<JobTitleResponseDto>>> GetByDepartmentIdAsync(Guid departmentId) {
			try {
				var data = await (from d in _dbContext.DepartmentJobTitles.Where(x => x.Id == departmentId)
								  join j in _dbContext.JobTitles.Where(x => x.Status == 1) on d.JobTitleId equals j.Id
								  select new JobTitleResponseDto {
									  Id = j.Id,
									  JobTitleName = j.JobTitleName,
									  Status = j.Status,
									  CreatedAt = j.CreatedAt,
									  UpdatedAt = j.UpdatedAt,
								  }).ToListAsync();

				if(data == null || data.Count == 0) {
					return new ApiResponse<List<JobTitleResponseDto>> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				return new ApiResponse<List<JobTitleResponseDto>> {
					IsSuccess = true,
					Data = data,
				};
			} catch (Exception ex) {
				return new ApiResponse<List<JobTitleResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<JobTitleResponseDto>> GetByIdAsync(Guid id) {
			try {
				var data = await _dbContext.JobTitles.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();
				if (data == null) {
					return new ApiResponse<JobTitleResponseDto>() {
						IsSuccess = false,
						Message = "Not Found"
					};
				}

				var result = _mapper.Map<JobTitleResponseDto>(data);

				return new ApiResponse<JobTitleResponseDto>() {
					Data = result,
					IsSuccess = true,
				};
			} catch (Exception ex) {
				return new ApiResponse<JobTitleResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<JobTitleResponseDto>> UpdateAsync(UpdateJobTitleRequestDto jobTitle) {
			try {
				if (jobTitle != null) {
					var dataFromDb = await _dbContext.JobTitles.Where(x => x.Id == jobTitle.Id).FirstOrDefaultAsync();

					if (dataFromDb == null) {
						return new ApiResponse<JobTitleResponseDto> {
							IsSuccess = false,
							Message = "Not Found"
						};
					}

					dataFromDb.JobTitleName = jobTitle.JobTitleName;
					dataFromDb.Status = jobTitle.Status;
					dataFromDb.UpdatedAt = DateTime.Now;

					await _dbContext.SaveChangesAsync();

					var query = await _dbContext.JobTitles.Where(x => x.Id == jobTitle.Id).FirstOrDefaultAsync();
					var result = _mapper.Map<JobTitleResponseDto>(query);

					return new ApiResponse<JobTitleResponseDto>() {
						Data = result,
						IsSuccess = true,
						Message = "Updated successfully"
					};
				}
				return new ApiResponse<JobTitleResponseDto>() {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<JobTitleResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
