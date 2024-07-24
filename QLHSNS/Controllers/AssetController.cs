using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Asset;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Asset;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AssetController : ControllerBase {
		private readonly IAssetService _service;

		public AssetController(IAssetService service) {
			_service = service;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<AssetResponseDto>> GetById(Guid id) {
			return await _service.GetAssetByIdAsync(id);
		}

		[HttpPost("GetList")]
		public async Task<ApiResponse<PagedResult<AssetResponseDto>>> GetList(PagingRequestBase request) {
			return await _service.GetAssetsAsync(request);
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<IActionResult> Delete(Guid id) {
			var result = await _service.DeleteAssetAsync(id);
			return result ? Ok() : BadRequest();
		}

		[HttpPost("Create")]
		public async Task<ApiResponse<string>> Create(CreateAssetRequestDto request) {
			return await _service.CreateAssetAsync(request);
		}

		[HttpPut("Update")]
		public async Task<ApiResponse<AssetResponseDto>> Update(UpdateAssetRequestDto request) {
			return await _service.UpdateAssetAsync(request);
		}

		[HttpPut("Enable/{id:Guid}")]
		public async Task<ApiResponse<AssetResponseDto>> Enable(Guid id) {
			return await _service.EnableAssetAsync(id);
		}

		[HttpPut("Disable/{id:Guid}")]
		public async Task<ApiResponse<AssetResponseDto>> Disable(Guid id) {
			return await _service.DisableAssetAsync(id);
		}

		[HttpGet("GetAll/{status:int}")]
		public async Task<ApiResponse<List<AssetResponseDto>>> GetAll(int status) {
			return await _service.GetAllAssetsAsync(status);
		}
	}
}
