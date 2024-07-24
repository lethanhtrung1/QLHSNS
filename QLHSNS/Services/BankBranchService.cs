using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.BankBranch;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class BankBranchService : IBankBranchService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public BankBranchService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<BankBranchResponseDto>> DisableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.BankBranches.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();
				if (dataFromDb == null) {
					return new ApiResponse<BankBranchResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}
				dataFromDb.Status = 0;
				dataFromDb.UpdatedAt = DateTime.Now;

				var query = await _dbContext.BankBranches.Where(x => x.Id == id && x.Status == 0)
														 .Select(x => x.Bank).FirstOrDefaultAsync();

				var result = _mapper.Map<BankBranchResponseDto>(query);

				return new ApiResponse<BankBranchResponseDto> {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<BankBranchResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<BankBranchResponseDto>> EnableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.BankBranches.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();
				if (dataFromDb == null) {
					return new ApiResponse<BankBranchResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}
				dataFromDb.Status = 1;
				dataFromDb.UpdatedAt = DateTime.Now;

				var query = await _dbContext.BankBranches.Where(x => x.Id == id && x.Status == 1)
														 .Select(x => x.Bank).FirstOrDefaultAsync();

				var result = _mapper.Map<BankBranchResponseDto>(query);

				return new ApiResponse<BankBranchResponseDto> {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<BankBranchResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<BankBranchDto>>> GetAllAsync(int status) {
			try {
				var data = new List<BankBranch>();
				if (status == 0 || status == 1)
					data = await _dbContext.BankBranches.Include(x => x.Bank)
						.Where(x => x.Status == 1 && x.Bank.Status == 1).ToListAsync();
				else
					data = await _dbContext.BankBranches.Include(x => x.Bank).Where(x => x.Bank.Status == 1).ToListAsync();

				if (data == null || data.Count == 0) {
					return new ApiResponse<List<BankBranchDto>> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				var result = _mapper.Map<List<BankBranchDto>>(data);

				return new ApiResponse<List<BankBranchDto>> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<List<BankBranchDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<BankBranchResponseDto>> GetBankBranchByIdAsync(Guid id) {
			try {
				var data = await _dbContext.BankBranches.Where(x => x.Id == id && x.Status == 1)
														.Include(x => x.Bank).FirstOrDefaultAsync();

				if (data == null) {
					return new ApiResponse<BankBranchResponseDto>() {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				var result = _mapper.Map<BankBranchResponseDto>(data);

				return new ApiResponse<BankBranchResponseDto>() {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<BankBranchResponseDto>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PagedResult<BankBranchResponseDto>>> GetBankBranchesAsync(PagingRequestBase request) {
			try {
				if (request != null) {
					var data = await _dbContext.BankBranches.Where(x => x.Status == 1)
									.Include(x => x.Bank)
									.Skip((request.PageNumber - 1) * request.PageSize)
									.Take(request.PageSize).ToListAsync();

					if (data == null || data.Count == 0) {
						return new ApiResponse<PagedResult<BankBranchResponseDto>>() {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					int totalRecord = await _dbContext.BankBranches.CountAsync();
					var result = _mapper.Map<List<BankBranchResponseDto>>(data);

					return new ApiResponse<PagedResult<BankBranchResponseDto>>() {
						Data = new PagedResult<BankBranchResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
						IsSuccess = true,
					};
				}
				return new ApiResponse<PagedResult<BankBranchResponseDto>> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<BankBranchResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<BankBranchDto>>> GetByBankIdAsync(Guid BankId, int status) {
			try {
				var checkBank = await _dbContext.Banks.Where(x => x.Id == BankId && x.Status == status).FirstOrDefaultAsync();
				if (checkBank == null) {
					return new ApiResponse<List<BankBranchDto>> {
						IsSuccess = false,
						Message = "Bank is not active"
					};
				}
				var data = new List<BankBranch>();
				if (status == 0 || status == 1)
					data = await _dbContext.BankBranches.Where(x => x.BankId == BankId && x.Status == status).Include(x => x.Bank).ToListAsync();
				else
					data = await _dbContext.BankBranches.Where(x => x.BankId == BankId).Include(x => x.Bank).ToListAsync();

				if (data == null || data.Count == 0) {
					return new ApiResponse<List<BankBranchDto>> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				var result = _mapper.Map<List<BankBranchDto>>(data);

				return new ApiResponse<List<BankBranchDto>> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<List<BankBranchDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
