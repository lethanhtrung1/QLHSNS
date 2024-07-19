using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Constants;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.HealthCareRequestDto;
using QLHSNS.DTOs.Response;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class HealthCareService : IHealthCareService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public HealthCareService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<HealthCare>> CreateHealthCareAsync(CreateHealthCareRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.HealthCares.Where(x => x.Name.ToLower() == request.Name.ToLower())
																 .FirstOrDefaultAsync();

					if (dataFromDb != null) {
						return new ApiResponse<HealthCare> {
							IsSuccess = false,
							Message = "Health Care already exist"
						};
					}
					var data = _mapper.Map<HealthCare>(request);
					await _dbContext.HealthCares.AddAsync(data);
					await _dbContext.SaveChangesAsync();
					return new ApiResponse<HealthCare> {
						IsSuccess = true,
						Data = data,
					};
				}
				return new ApiResponse<HealthCare> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<HealthCare> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<bool> DeleteHealthCareAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.HealthCares.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (dataFromDb == null) return false;

				_dbContext.HealthCares.Remove(dataFromDb);
				await _dbContext.SaveChangesAsync();

				return true;
			} catch (Exception) {
				throw;
			}
		}

		public async Task<ApiResponse<HealthCare>> DisableHealthCareAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.HealthCares.Where(x => x.Id == id && x.Status == 1)
													.FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<HealthCare> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				dataFromDb.Status = 0;
				dataFromDb.UpdatedAt = DateTime.Now;

				await _dbContext.SaveChangesAsync();

				var result = await _dbContext.HealthCares.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();

				return new ApiResponse<HealthCare> {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<HealthCare> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<HealthCare>> EnableHealthCareAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.HealthCares.Where(x => x.Id == id && x.Status == 0)
													.FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<HealthCare> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				dataFromDb.Status = 1;
				dataFromDb.UpdatedAt = DateTime.Now;

				await _dbContext.SaveChangesAsync();

				var result = await _dbContext.HealthCares.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();

				return new ApiResponse<HealthCare> {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<HealthCare> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<HealthCare>>> GetAllHealthCaresAsync() {
			try {
				var data = await _dbContext.HealthCares.Where(x => x.Status == 1).ToListAsync();

				if(data == null || data.Count == 0) {
					return new ApiResponse<List<HealthCare>> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND
					};
				}

				return new ApiResponse<List<HealthCare>> {
					IsSuccess = true,
					Data = data,
				};
			} catch (Exception ex) {
				return new ApiResponse<List<HealthCare>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<HealthCare>> GetHealthCareByIdAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.HealthCares.Where(x => x.Id == id && x.Status == 1)
											.FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<HealthCare> {
						IsSuccess = false,
						Message = "Not fount"
					};
				}

				return new ApiResponse<HealthCare> {
					Data = dataFromDb,
					IsSuccess = true,
				};
			} catch (Exception ex) {
				return new ApiResponse<HealthCare> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PagedResult<HealthCare>>> GetHealthCaresAsync(PagingRequestBase request) {
			try {
				var data = await _dbContext.HealthCares.Where(x => x.Status == 1)
									.Skip((request.PageNumber - 1) * request.PageSize)
									.Take(request.PageSize).ToListAsync();

				if (data == null || data.Count == 0) {
					return new ApiResponse<PagedResult<HealthCare>> {
						IsSuccess = false,
						Message = "Not fount"
					};
				}

				int totalRecord = await _dbContext.HealthCares.Where(x => x.Status == 1).CountAsync();

				return new ApiResponse<PagedResult<HealthCare>> {
					IsSuccess = true,
					Data = new PagedResult<HealthCare>(data, totalRecord, request.PageNumber, request.PageNumber),
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<HealthCare>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<HealthCare>> UpdateHealthCareAsync(UpdateHealthCareRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.HealthCares.Where(x => x.Name.ToLower() == request.Name.ToLower())
																 .FirstOrDefaultAsync();

					if (dataFromDb == null) {
						return new ApiResponse<HealthCare> {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					dataFromDb.Name = request.Name;
					dataFromDb.Address = request.Address;
					dataFromDb.PhoneNumber = request.PhoneNumber;
					dataFromDb.Email = request.Email;
					dataFromDb.Status = request.Status;
					dataFromDb.UpdatedAt = DateTime.Now;

					await _dbContext.SaveChangesAsync();

					return new ApiResponse<HealthCare> {
						IsSuccess = true,
						Data = dataFromDb,
					};
				}
				return new ApiResponse<HealthCare> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<HealthCare> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
