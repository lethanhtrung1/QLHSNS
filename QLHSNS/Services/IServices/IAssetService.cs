using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Asset;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Asset;

namespace QLHSNS.Services.IServices {
	public interface IAssetService {
		Task<ApiResponse<AssetResponseDto>> GetAssetByIdAsync(Guid id);
		Task<ApiResponse<PagedResult<AssetResponseDto>>> GetAssetsAsync(PagingRequestBase request);
		Task<bool> DeleteAssetAsync(Guid id);
		Task<ApiResponse<string>> CreateAssetAsync(CreateAssetRequestDto request);
		Task<ApiResponse<AssetResponseDto>> UpdateAssetAsync(UpdateAssetRequestDto request);
		Task<ApiResponse<AssetResponseDto>> EnableAssetAsync(Guid id);
		Task<ApiResponse<AssetResponseDto>> DisableAssetAsync(Guid id);
		Task<ApiResponse<List<AssetResponseDto>>> GetAllAssetsAsync();
	}
}
