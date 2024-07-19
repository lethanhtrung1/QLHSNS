using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Asset;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Asset;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class AssetService : IAssetService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public AssetService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<string>> CreateAssetAsync(CreateAssetRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.Assets.Where(x => x.Name.ToLower() == request.Name.ToLower()).FirstOrDefaultAsync();

					if (dataFromDb != null) {
						return new ApiResponse<string> {
							IsSuccess = false,
							Message = "Asset already exist"
						};
					}
					var data = _mapper.Map<Asset>(request);
					await _dbContext.Assets.AddAsync(data);
					await _dbContext.SaveChangesAsync();

					return new ApiResponse<string> {
						IsSuccess = true,
						Data = data.Id.ToString()
					};
				}
				return new ApiResponse<string> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<string> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<bool> DeleteAssetAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Assets.Where(x => x.Id == id).FirstOrDefaultAsync();
				if (dataFromDb == null) return false;

				_dbContext.Assets.Remove(dataFromDb);
				await _dbContext.SaveChangesAsync();

				return true;
			} catch (Exception) {
				throw;
			}
		}

		public async Task<ApiResponse<AssetResponseDto>> DisableAssetAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Assets.FirstOrDefaultAsync(x => x.Id == id && x.Status == 1);

				if (dataFromDb == null) {
					return new ApiResponse<AssetResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				dataFromDb.Status = 0;

				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.Assets.FirstOrDefaultAsync(x => x.Id == id && x.Status == 0);
				var result = _mapper.Map<AssetResponseDto>(query);

				return new ApiResponse<AssetResponseDto> {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<AssetResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<AssetResponseDto>> EnableAssetAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Assets.FirstOrDefaultAsync(x => x.Id == id && x.Status == 0);
				if (dataFromDb == null) {
					return new ApiResponse<AssetResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				dataFromDb.Status = 1;

				await _dbContext.SaveChangesAsync();

				var query = await _dbContext.Assets.FirstOrDefaultAsync(x => x.Id == id && x.Status == 1);
				var result = _mapper.Map<AssetResponseDto>(query);

				return new ApiResponse<AssetResponseDto> {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<AssetResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<AssetResponseDto>> GetAssetByIdAsync(Guid id) {
			try {
				var data = await _dbContext.Assets.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();
				if (data == null) {
					return new ApiResponse<AssetResponseDto> {
						IsSuccess = false,
						Message = "Not found"
					};
				}
				var result = _mapper.Map<AssetResponseDto>(data);
				return new ApiResponse<AssetResponseDto> {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<AssetResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PagedResult<AssetResponseDto>>> GetAssetsAsync(PagingRequestBase request) {
			try {
				if (request != null) {
					var dataList = await _dbContext.Assets.Where(x => x.Status == 1)
										.Skip((request.PageNumber - 1) * request.PageSize)
										.Take(request.PageSize).ToListAsync();

					if (dataList == null || dataList.Count == 0) {
						return new ApiResponse<PagedResult<AssetResponseDto>> {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					var result = _mapper.Map<List<AssetResponseDto>>(dataList);
					var totalRecord = await _dbContext.Assets.CountAsync();

					return new ApiResponse<PagedResult<AssetResponseDto>> {
						IsSuccess = true,
						Data = new PagedResult<AssetResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
					};
				}

				return new ApiResponse<PagedResult<AssetResponseDto>> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<AssetResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<AssetResponseDto>>> GetAllAssetsAsync() {
			try {
				var data = await _dbContext.Assets.Where(x => x.Status == 1).ToListAsync();

				if (data == null || data.Count == 0) {
					return new ApiResponse<List<AssetResponseDto>> {
						IsSuccess = false,
						Message = "No data"
					};
				}

				var result = _mapper.Map<List<AssetResponseDto>>(data);

				return new ApiResponse<List<AssetResponseDto>> {
					IsSuccess = true,
					Data = result,
				};
			} catch (Exception ex) {
				return new ApiResponse<List<AssetResponseDto>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<AssetResponseDto>> UpdateAssetAsync(UpdateAssetRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.Assets.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
					if (dataFromDb == null) {
						return new ApiResponse<AssetResponseDto> {
							IsSuccess = false,
							Message = "Not found"
						};
					}
					dataFromDb.Name = request.Name;
					dataFromDb.Description = request.Description;
					dataFromDb.Price = request.Price;
					dataFromDb.Status = request.Status;

					await _dbContext.SaveChangesAsync();

					var query = await _dbContext.Assets.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
					var result = _mapper.Map<AssetResponseDto>(query);

					return new ApiResponse<AssetResponseDto> {
						IsSuccess = true,
						Data = result,
					};
				}

				return new ApiResponse<AssetResponseDto> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<AssetResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
