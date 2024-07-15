using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Bank;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class BankService : IBankService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public BankService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<BankResponseDto>> DisableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Banks.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<BankResponseDto>() {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				dataFromDb.Status = 0;

				var bankBranches = await _dbContext.BankBranches.Where(x => x.BankId == id && x.Status == 1).ToListAsync();

				foreach (var item in bankBranches) {
					item.Status = 0;
				}

				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.Banks.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();
				var result = _mapper.Map<BankResponseDto>(query);

				return new ApiResponse<BankResponseDto>() {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<BankResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<BankResponseDto>> EnableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Banks.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<BankResponseDto>() {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				dataFromDb.Status = 1;

				var bankBranches = await _dbContext.BankBranches.Where(x => x.BankId == id && x.Status == 0).ToListAsync();

				foreach (var item in bankBranches) {
					item.Status = 1;
				}

				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.Banks.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();
				var result = _mapper.Map<BankResponseDto>(query);

				return new ApiResponse<BankResponseDto>() {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<BankResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<BankResponseDto>>> GetAllAsync() {
			try {
				var data = await _dbContext.Banks.Where(x => x.Status == 1).ToListAsync();

				if (data == null || data.Count == 0) {
					return new ApiResponse<List<BankResponseDto>>() {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				var result = _mapper.Map<List<BankResponseDto>>(data);

				return new ApiResponse<List<BankResponseDto>>() {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<List<BankResponseDto>>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<BankResponseDto>> GetBankByIdAsync(Guid id) {
			try {
				var data = await _dbContext.Banks.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();

				if (data == null) {
					return new ApiResponse<BankResponseDto>() {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				var result = _mapper.Map<BankResponseDto>(data);

				return new ApiResponse<BankResponseDto>() {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<BankResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PagedResult<BankResponseDto>>> GetBanksAsync(PagingRequestBase request) {
			try {
				if (request != null) {
					var data = await _dbContext.Banks.Where(x => x.Status == 1)
									.Skip((request.PageNumber - 1) * request.PageSize)
									.Take(request.PageSize).ToListAsync();

					if (data == null || data.Count == 0) {
						return new ApiResponse<PagedResult<BankResponseDto>>() {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					int totalRecord = await _dbContext.Locations.CountAsync();
					var result = _mapper.Map<List<BankResponseDto>>(data);

					return new ApiResponse<PagedResult<BankResponseDto>>() {
						Data = new PagedResult<BankResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
						IsSuccess = true,
					};
				}
				return new ApiResponse<PagedResult<BankResponseDto>> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<BankResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
