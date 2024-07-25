using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Constants;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Allowance;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Allowance;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class AllowanceService : IAllowanceService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public AllowanceService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<AllowanceResponseDto>> EnableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Allowances.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<AllowanceResponseDto>() {
						IsSuccess = false,
						Message = "Not fount"
					};
				}
				dataFromDb.Status = 1;
				dataFromDb.UpdatedAt = DateTime.Now;

				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.Allowances.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();

				var result = _mapper.Map<AllowanceResponseDto>(query);

				return new ApiResponse<AllowanceResponseDto>() {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully",
				};
			} catch (Exception ex) {
				return new ApiResponse<AllowanceResponseDto>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<AllowanceResponseDto>> DisableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Allowances.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<AllowanceResponseDto>() {
						IsSuccess = false,
						Message = "Not fount"
					};
				}

				dataFromDb.Status = 0;
				dataFromDb.UpdatedAt = DateTime.Now;

				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.Allowances.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();

				var result = _mapper.Map<AllowanceResponseDto>(query);

				return new ApiResponse<AllowanceResponseDto>() {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully",
				};
			} catch (Exception ex) {
				return new ApiResponse<AllowanceResponseDto>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<AllowanceResponseDto>> CreateAsync(CreateAllowanceRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.Allowances
						.Where(x => x.AllowanceName.ToLower() == request.AllowanceName.ToLower())
						.FirstOrDefaultAsync();

					if (dataFromDb != null) {
						return new ApiResponse<AllowanceResponseDto>() {
							IsSuccess = false,
							Message = "Constract type already exist"
						};
					}
					var data = _mapper.Map<Allowance>(request);
					await _dbContext.Allowances.AddAsync(data);
					await _dbContext.SaveChangesAsync();

					var query = await _dbContext.Allowances.Where(x => x.Id == data.Id).FirstOrDefaultAsync();
					var result = _mapper.Map<AllowanceResponseDto>(query);

					return new ApiResponse<AllowanceResponseDto>() {
						Data = result,
						IsSuccess = true,
						Message = "Created successfully"
					};
				}
				return new ApiResponse<AllowanceResponseDto>() {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<AllowanceResponseDto>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<bool> DeleteAsync(Guid id) {
			try {
				var data = await _dbContext.Allowances.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (data == null) return false;

				_dbContext.Allowances.Remove(data);
				await _dbContext.SaveChangesAsync();

				return true;
			} catch (Exception) {
				throw;
			}
		}

		public async Task<ApiResponse<PagedResult<AllowanceResponseDto>>> GetPagingAsync(PagingRequestBase request) {
			try {
				var data = await _dbContext.Allowances
						.Skip((request.PageNumber - 1) * request.PageSize)
						.Take(request.PageSize).ToListAsync();

				if (data.Count == 0) {
					return new ApiResponse<PagedResult<AllowanceResponseDto>>() {
						IsSuccess = false,
						Message = "No data"
					};
				}

				int totalResord = await _dbContext.Allowances.CountAsync();
				var result = _mapper.Map<List<AllowanceResponseDto>>(data);

				return new ApiResponse<PagedResult<AllowanceResponseDto>>() {
					Data = new PagedResult<AllowanceResponseDto>(result, totalResord, request.PageNumber, request.PageSize),
					IsSuccess = true,
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<AllowanceResponseDto>>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<AllowanceResponseDto>> GetByIdAsync(Guid id) {
			try {
				var data = await _dbContext.Allowances.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (data == null) {
					return new ApiResponse<AllowanceResponseDto>() {
						IsSuccess = false,
						Message = "Not fount"
					};
				}

				var result = _mapper.Map<AllowanceResponseDto>(data);

				return new ApiResponse<AllowanceResponseDto>() {
					Data = result,
					IsSuccess = true
				};
			} catch (Exception ex) {
				return new ApiResponse<AllowanceResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<AllowanceResponseDto>> UpdateAsync(UpdateAllowanceRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.Allowances.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
					if (dataFromDb == null) {
						return new ApiResponse<AllowanceResponseDto>() {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					dataFromDb.AllowanceName = request.AllowanceName;
					dataFromDb.Value = request.Value;
					dataFromDb.Unit = request.Unit;
					dataFromDb.UpdatedAt = DateTime.Now;

					await _dbContext.SaveChangesAsync();

					var query = await _dbContext.Allowances.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
					var result = _mapper.Map<AllowanceResponseDto>(query);

					return new ApiResponse<AllowanceResponseDto>() {
						Data = result,
						IsSuccess = true,
						Message = "Updated successfully"
					};
				}

				return new ApiResponse<AllowanceResponseDto>() {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<AllowanceResponseDto>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<List<AllowanceResponseDto>>> GetAllAsync(int status) {
			try {
				var data = new List<Allowance>();

				if (status == FilterStatus.Active || status == FilterStatus.NonActive)
					data = await _dbContext.Allowances.Where(x => x.Status == status).ToListAsync();
				else if (status == FilterStatus.All)
					data = await _dbContext.Allowances.ToListAsync();
				else
					return new ApiResponse<List<AllowanceResponseDto>> {
						IsSuccess = false,
						Message = Message.INVALID_PAYLOAD
					};

				if (data == null || data.Count == 0) {
					return new ApiResponse<List<AllowanceResponseDto>> {
						IsSuccess = false,
						Message = "Not data"
					};
				}

				var result = _mapper.Map<List<AllowanceResponseDto>>(data);

				return new ApiResponse<List<AllowanceResponseDto>> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<List<AllowanceResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
