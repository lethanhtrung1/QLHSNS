using Microsoft.EntityFrameworkCore;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Location;
using QLHSNS.DTOs.Response;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class LocationService : ILocationService {
		private readonly AppDbContext _dbContext;

		public LocationService(AppDbContext dbContext) {
			_dbContext = dbContext;
		}

		public async Task<ApiResponse<Location>> GetLocationByIdAsync(Guid id) {
			try {
				var data = await _dbContext.Locations.Where(x => x.Id == id).FirstOrDefaultAsync();
				if(data == null) {
					return new ApiResponse<Location>() {
						IsSuccess = false,
						Message = "Not found"
					};
				}
				return new ApiResponse<Location>() {
					IsSuccess = true,
					Data = data,
				};
			} catch (Exception ex) {
				return new ApiResponse<Location>() {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PagedResult<Location>>> GetLocationsPagingAsync(PagingRequestBase request) {
			try {
				if(request != null) {
					var data = await _dbContext.Locations.Skip((request.PageNumber - 1) * request.PageSize)
														 .Take(request.PageSize).ToListAsync();
					if(data == null || data.Count == 0) {
						return new ApiResponse<PagedResult<Location>>() { IsSuccess = false, Message = "Not found" };
					}
					int totalRecord = await _dbContext.Locations.CountAsync();

					return new ApiResponse<PagedResult<Location>>() {
						IsSuccess = true,
						Data = new PagedResult<Location>(data, totalRecord, request.PageNumber, request.PageSize)
					};
				}
				return new ApiResponse<PagedResult<Location>> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<Location>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<Location>>> GetAllLocationsAsync(LocationRequestDto request) {
			try {
				if(request != null) {
					var query = new List<Location>();
					query = await _dbContext.Locations.ToListAsync();

					if(!string.IsNullOrWhiteSpace(request.Keyword!.Trim())) {
						query = query.Where(x => x.Country.Trim().ToLower().Contains(request.Keyword.Trim().ToLower())
											|| x.Province.Trim().ToLower().Contains(request.Keyword.Trim().ToLower())
											|| x.District.Trim().ToLower().Contains(request.Keyword.Trim().ToLower())
											|| x.Ward.Trim().ToLower().Contains(request.Keyword.Trim().ToLower())).ToList();
					}

					if(request.Country != null) {
						query = query.Where(x => x.Country.ToLower() ==  request.Country.ToLower()).ToList();
					}
					if (request.Province != null) {
						query = query.Where(x => x.Province.ToLower() == request.Province.ToLower()).ToList();
					}
					if (request.District != null) {
						query = query.Where(x => x.District.ToLower() == request.District.ToLower()).ToList();
					}
					if (request.Ward != null) {
						query = query.Where(x => x.Ward.ToLower() == request.Ward.ToLower()).ToList();
					}

					return new ApiResponse<List<Location>> {
						IsSuccess = true,
						Data = query
					};
				}
				return new ApiResponse<List<Location>> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<List<Location>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
