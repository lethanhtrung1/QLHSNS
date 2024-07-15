using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.ContractType;
using QLHSNS.DTOs.Response;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class ContractTypeService : IContractTypeService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public ContractTypeService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<ContractType>> CreateAsync(CreateContractTypeRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.ContractTypes
						.Where(x => x.ContractTypeName.ToLower() == request.ContractTypeName.ToLower())
						.FirstOrDefaultAsync();

					if (dataFromDb != null) {
						return new ApiResponse<ContractType>() {
							IsSuccess = false,
							Message = "Constract type already exist"
						};
					}

					var data = _mapper.Map<ContractType>(request);

					await _dbContext.ContractTypes.AddAsync(data);
					await _dbContext.SaveChangesAsync();

					return new ApiResponse<ContractType>() {
						Data = data,
						IsSuccess = true,
						Message = "Created successfully"
					};
				}
				return new ApiResponse<ContractType>() {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<ContractType>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<ContractType>> DeleteAsync(Guid id) {
			try {
				var data = await _dbContext.ContractTypes.Where(x => x.Id == id).FirstOrDefaultAsync();
				if (data == null) {
					return new ApiResponse<ContractType>() {
						IsSuccess = false,
						Message = "Not fount"
					};
				}

				_dbContext.ContractTypes.Remove(data);
				await _dbContext.SaveChangesAsync();

				return new ApiResponse<ContractType>() {
					IsSuccess = true,
					Message = "Deleted successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<ContractType>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<ContractType>> DisableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.ContractTypes.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<ContractType>() {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				dataFromDb.Status = 0;
				dataFromDb.UpdatedAt = DateTime.Now;

				await _dbContext.SaveChangesAsync();

				var result = await _dbContext.ContractTypes.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();

				return new ApiResponse<ContractType>() {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<ContractType>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<ContractType>> EnableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.ContractTypes.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();
				if (dataFromDb == null) {
					return new ApiResponse<ContractType>() {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				dataFromDb.Status = 1;
				dataFromDb.UpdatedAt = DateTime.Now;

				await _dbContext.SaveChangesAsync();

				var result = await _dbContext.ContractTypes.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();

				return new ApiResponse<ContractType>() {
					Data = result,
					IsSuccess = true,
					Message = "Updated successfully"
				};
			} catch (Exception ex) {
				return new ApiResponse<ContractType>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<PagedResult<ContractType>>> GetAsync(PagingRequestBase request) {
			try {
				var data = await _dbContext.ContractTypes.Where(x => x.Status == 1)
									.Skip((request.PageNumber - 1) * request.PageSize)
									.Take(request.PageSize).ToListAsync();

				if (data.Count == 0) {
					return new ApiResponse<PagedResult<ContractType>> {
						IsSuccess = false,
						Message = "Not found"
					};
				}

				int totalRecord = await _dbContext.ContractTypes.Where(x => x.Status == 1).CountAsync();

				return new ApiResponse<PagedResult<ContractType>>() {
					Data = new PagedResult<ContractType>(data, totalRecord, request.PageNumber, request.PageSize),
					IsSuccess = true
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<ContractType>>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<ContractType>> GetByIdAsync(Guid id) {
			try {
				var data = await _dbContext.ContractTypes.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();

				if (data == null) {
					return new ApiResponse<ContractType>() {
						IsSuccess = false,
						Message = "Not fount"
					};
				}

				return new ApiResponse<ContractType>() {
					Data = data,
					IsSuccess = true
				};
			} catch (Exception ex) {
				return new ApiResponse<ContractType>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<ApiResponse<ContractType>> UpdateAsync(UpdateContractTypeRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.ContractTypes.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
					if (dataFromDb == null) {
						return new ApiResponse<ContractType>() {
							IsSuccess = false,
							Message = "Not found"
						};
					}
					dataFromDb.ContractTypeName = request.ContractTypeName;
					dataFromDb.Status = request.Status;
					dataFromDb.UpdatedAt = DateTime.Now;

					await _dbContext.SaveChangesAsync();

					var result = await _dbContext.ContractTypes.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

					return new ApiResponse<ContractType>() {
						Data = result,
						IsSuccess = true,
						Message = "Updated successfully"
					};
				}

				return new ApiResponse<ContractType>() {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<ContractType>() {
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}
	}
}
