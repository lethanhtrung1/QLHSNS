using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Benefit;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Benefit;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class BenefitService : IBenefitService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public BenefitService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<BenefitResponseDto>> EnableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Benefits.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();
				if (dataFromDb == null) {
					return new ApiResponse<BenefitResponseDto>() {
						IsSuccess = false,
						Message = "Not fount"
					};
				}
				dataFromDb.Status = 1;
				dataFromDb.UpdatedAt = DateTime.Now;
				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.Benefits.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();
				var result = _mapper.Map<BenefitResponseDto>(query);

				return new ApiResponse<BenefitResponseDto>() {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully",
				};
			} catch (Exception ex) {
				return new ApiResponse<BenefitResponseDto>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<BenefitResponseDto>> DisableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Benefits.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();
				if (dataFromDb == null) {
					return new ApiResponse<BenefitResponseDto>() {
						IsSuccess = false,
						Message = "Not fount"
					};
				}
				dataFromDb.Status = 0;
				dataFromDb.UpdatedAt = DateTime.Now;

				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.Benefits.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();
				var result = _mapper.Map<BenefitResponseDto>(query);

				return new ApiResponse<BenefitResponseDto>() {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully",
				};
			} catch (Exception ex) {
				return new ApiResponse<BenefitResponseDto>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<BenefitResponseDto>> CreateAsync(CreateBenefitRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.Benefits
						.Where(x => x.BenefitName == request.BenefitName).FirstOrDefaultAsync();
					if (dataFromDb != null) {
						return new ApiResponse<BenefitResponseDto>() {
							IsSuccess = false,
							Message = "Benefit Scheme already exist",
						};
					}

					var data = _mapper.Map<Benefit>(request);
					await _dbContext.Benefits.AddAsync(data);
					await _dbContext.SaveChangesAsync();

					var query = await _dbContext.Benefits.Where(x => x.Id == data.Id).FirstOrDefaultAsync();
					var result = _mapper.Map<BenefitResponseDto>(query);

					return new ApiResponse<BenefitResponseDto>() {
						Data = result,
						IsSuccess = true,
						Message = "Created successfully"
					};
				}
				return new ApiResponse<BenefitResponseDto>() {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<BenefitResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<bool> DeleteAsync(Guid id) {
			try {
				var data = await _dbContext.Benefits.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (data == null) return false;

				_dbContext.Benefits.Remove(data);
				await _dbContext.SaveChangesAsync();

				return true;
			} catch (Exception) {
				throw;
			}
		}

		public async Task<ApiResponse<PagedResult<BenefitResponseDto>>> GetPagingAsync(PagingRequestBase request) {
			try {
				var data = await _dbContext.Benefits
							.Skip((request.PageNumber - 1) * request.PageSize)
							.Take(request.PageSize).ToListAsync();

				if (data.Count == 0) {
					return new ApiResponse<PagedResult<BenefitResponseDto>>() {
						IsSuccess = false,
						Message = "No data"
					};
				}

				int totalRecord = await _dbContext.Benefits.CountAsync();
				var result = _mapper.Map<List<BenefitResponseDto>>(data);

				return new ApiResponse<PagedResult<BenefitResponseDto>>() {
					Data = new PagedResult<BenefitResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
					IsSuccess = true,
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<BenefitResponseDto>>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<BenefitResponseDto>> GetByIdAsync(Guid id) {
			try {
				var data = await _dbContext.Benefits.Where(x => x.Id == id).FirstOrDefaultAsync();
				if (data == null) {
					return new ApiResponse<BenefitResponseDto>() {
						IsSuccess = false,
						Message = "Not fount"
					};
				}

				var result = _mapper.Map<BenefitResponseDto>(data);
				return new ApiResponse<BenefitResponseDto>() {
					Data = result,
					IsSuccess = true
				};
			} catch (Exception ex) {
				return new ApiResponse<BenefitResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<BenefitResponseDto>> UpdateAsync(UpdateBenefitRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.Benefits.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
					if (dataFromDb == null) {
						return new ApiResponse<BenefitResponseDto>() {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					dataFromDb.BenefitName = request.BenefitName;
					dataFromDb.Description = request.Description;
					dataFromDb.Amount = request.Amount;
					dataFromDb.UpdatedAt = DateTime.Now;

					await _dbContext.SaveChangesAsync();

					var query = await _dbContext.Benefits.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
					var result = _mapper.Map<BenefitResponseDto>(query);

					return new ApiResponse<BenefitResponseDto>() {
						Data = result,
						IsSuccess = true,
						Message = "Updated successfully"
					};
				}

				return new ApiResponse<BenefitResponseDto>() {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<BenefitResponseDto>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<List<BenefitResponseDto>>> GetAllAsync(int status) {
			try {
				var data = new List<Benefit>();

				if(status == -1)
					data = await _dbContext.Benefits.ToListAsync();
				else
					data = await _dbContext.Benefits.Where(x => x.Status == status).ToListAsync();

				if (data == null || data.Count == 0) {
					return new ApiResponse<List<BenefitResponseDto>> {
						IsSuccess = false,
						Message = "No data"
					};
				}

				var result = _mapper.Map<List<BenefitResponseDto>>(data);

				return new ApiResponse<List<BenefitResponseDto>> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<List<BenefitResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
