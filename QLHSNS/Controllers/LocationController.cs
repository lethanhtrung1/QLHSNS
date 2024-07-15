using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Location;
using QLHSNS.DTOs.Response;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class LocationController : ControllerBase {
		private readonly ILocationService _service;

		public LocationController(ILocationService service) {
			_service = service;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<Location>> GetLocationById(Guid id) {
			return await _service.GetLocationByIdAsync(id);
		}

		[HttpPost("GetList")]
		public async Task<ApiResponse<PagedResult<Location>>> GetLocations(PagingRequestBase request) {
			return await _service.GetLocationsPagingAsync(request);
		}

		[HttpPost("GetAll")]
		public async Task<ApiResponse<List<Location>>> GetAllLocations(LocationRequestDto request) {
			return await _service.GetAllLocationsAsync(request);
		}
	}
}
