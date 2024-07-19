using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Reward;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Reward;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class RewardController : ControllerBase {
		private readonly IRewardService _service;

		public RewardController(IRewardService service) {
			_service = service;
		}

		[HttpGet("GetRewardEmployee/{id:guid}")]
		public async Task<ApiResponse<RewardResponseDto>> GetRewardEmployee(Guid id, int month, int year) {
			return await _service.GetRewardEmployeeAsync(id, month, year);
		}

		[HttpPost("GetRewardList")]
		public async Task<ApiResponse<PagedResult<RewardResponseDto>>> GetRewardList(GetRewadPagingRequestDto request) {
			return await _service.GetRewardListByMonthAsync(request);
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<IActionResult> DeleteReward(Guid id) {
			var result = await _service.DeleteAsync(id);
			return result ? Ok() : BadRequest();
		}

		[HttpPost("Add")]
		public async Task<ApiResponse<RewardResponseDto>> AddNewReward(CreateRewardRequestDto request) {
			return await _service.AddRewardAsync(request);
		}

		[HttpPut("Update")]
		public async Task<ApiResponse<RewardResponseDto>> UpdateReward(UpdateRewardRequestDto request) {
			return await _service.UpdaeRewardAsync(request);
		}

		[HttpPut("Received")]
		public async Task<IActionResult> ConfirmReceived(Guid id) {
			var result = await _service.ConfirmReceivedAsync(id);
			return result ? Ok() : BadRequest();
		}
	}
}
