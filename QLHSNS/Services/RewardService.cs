using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Constants;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Reward;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Reward;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class RewardService : IRewardService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public RewardService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<RewardResponseDto>> AddRewardAsync(CreateRewardRequestDto request) {
			try {
				if (request != null) {
					var data = _mapper.Map<Reward>(request);

					await _dbContext.Rewards.AddAsync(data);
					await _dbContext.SaveChangesAsync();

					var result = _mapper.Map<RewardResponseDto>(data);

					return new ApiResponse<RewardResponseDto> {
						IsSuccess = true,
						Data = result,
						Message = Message.CREATED_SUCCESS,
					};
				}

				return new ApiResponse<RewardResponseDto> {
					IsSuccess = false,
					Message = Message.INVALID_PAYLOAD
				};
			} catch (Exception ex) {
				return new ApiResponse<RewardResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<bool> ConfirmReceivedAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Rewards.Where(x => x.Id == id && x.IsReceived == 0).FirstOrDefaultAsync();

				if (dataFromDb == null) return false;

				dataFromDb.IsReceived = 1;
				dataFromDb.UpdatedAt = DateTime.Now;

				return true;
			} catch (Exception) {

				throw;
			}
		}

		public async Task<bool> DeleteAsync(Guid id) {
			var dataTobeDelete = await _dbContext.Rewards.FindAsync(id);

			if (dataTobeDelete == null) return false;

			_dbContext.Rewards.Remove(dataTobeDelete);
			await _dbContext.SaveChangesAsync();

			return true;
		}

		public async Task<ApiResponse<PagedResult<RewardResponseDto>>> GetRewardListByMonthAsync(GetRewadPagingRequestDto request) {
			try {
				if (request != null) {
					var query = await _dbContext.Rewards.ToListAsync();
					if (request.Year != 0) {
						query = query.Where(x => x.Year == request.Year).ToList();
					}

					if (request.Month != 0) {
						query = query.Where(x => x.Month == request.Month).ToList();
					}

					if (request.IsReceived == 0 || request.IsReceived == 1) {
						query = query.Where(x => x.IsReceived == request.IsReceived).ToList();
					}

					if (query.Count == 0) {
						return new ApiResponse<PagedResult<RewardResponseDto>> {
							IsSuccess = false,
							Message = Message.DATA_NOT_FOUND
						};
					}

					int totalRecord = query.Count();
					query = query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

					var result = _mapper.Map<List<RewardResponseDto>>(query);

					return new ApiResponse<PagedResult<RewardResponseDto>> {
						Data = new PagedResult<RewardResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
						IsSuccess = true,
					};
				}

				return new ApiResponse<PagedResult<RewardResponseDto>> {
					IsSuccess = false,
					Message = Message.INVALID_PAYLOAD
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<RewardResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<RewardResponseDto>> GetRewardEmployeeAsync(Guid employeeId, int month, int year) {
			try {
				var dataFromDb = await _dbContext.Rewards
					.Where(x => x.EmployeeId == employeeId && x.Month == month && x.Year == year).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<RewardResponseDto> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND
					};
				}

				var result = _mapper.Map<RewardResponseDto>(dataFromDb);

				return new ApiResponse<RewardResponseDto> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<RewardResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<RewardResponseDto>> UpdaeRewardAsync(UpdateRewardRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.Rewards.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

					if(dataFromDb == null) {
						return new ApiResponse<RewardResponseDto> {
							IsSuccess = false,
							Message = Message.DATA_NOT_FOUND
						};
					}

					dataFromDb.RewardAmount = request.RewardAmount;
					dataFromDb.Description = request.Description;
					dataFromDb.Month = request.Month;
					dataFromDb.Year = request.Year;
					dataFromDb.UpdatedAt = DateTime.Now;

					await _dbContext.SaveChangesAsync();

					var query = await _dbContext.Rewards.FirstOrDefaultAsync(x => x.Id == request.Id);
					var result = _mapper.Map<RewardResponseDto>(query);

					return new ApiResponse<RewardResponseDto> {
						IsSuccess = true,
						Message = Message.UPDATED_SUCCESS,
						Data = result,
					};
				}

				return new ApiResponse<RewardResponseDto> {
					IsSuccess = false,
					Message = Message.INVALID_PAYLOAD
				};
			} catch (Exception ex) {
				return new ApiResponse<RewardResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
