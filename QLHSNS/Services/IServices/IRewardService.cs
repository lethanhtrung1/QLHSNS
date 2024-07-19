using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Reward;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Reward;

namespace QLHSNS.Services.IServices {
	public interface IRewardService {
		Task<bool> DeleteAsync(Guid id);
		Task<ApiResponse<RewardResponseDto>> GetRewardEmployeeAsync(Guid employeeId, int month, int year);
		Task<ApiResponse<RewardResponseDto>> AddRewardAsync(CreateRewardRequestDto request);
		Task<ApiResponse<RewardResponseDto>> UpdaeRewardAsync(UpdateRewardRequestDto request);
		Task<ApiResponse<PagedResult<RewardResponseDto>>> GetRewardListByMonthAsync(GetRewadPagingRequestDto request);
		Task<bool> ConfirmReceivedAsync(Guid id);
	}
}
