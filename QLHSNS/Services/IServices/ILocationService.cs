using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Location;
using QLHSNS.DTOs.Response;
using QLHSNS.Model;

namespace QLHSNS.Services.IServices {
	public interface ILocationService {
		Task<ApiResponse<PagedResult<Location>>> GetLocationsPagingAsync(PagingRequestBase request);
		Task<ApiResponse<Location>> GetLocationByIdAsync(Guid id);
		Task<ApiResponse<List<Location>>> GetAllLocationsAsync(LocationRequestDto request);
	}
}
