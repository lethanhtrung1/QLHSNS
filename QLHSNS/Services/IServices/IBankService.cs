using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Bank;

namespace QLHSNS.Services.IServices {
	public interface IBankService {
		Task<ApiResponse<PagedResult<BankResponseDto>>> GetBanksAsync(PagingRequestBase request);
		Task<ApiResponse<BankResponseDto>> GetBankByIdAsync(Guid id);
		Task<ApiResponse<List<BankResponseDto>>> GetAllAsync();
		Task<ApiResponse<List<BankResponseDto>>> FilterBank(int status);
		Task<ApiResponse<BankResponseDto>> EnableAsync(Guid id);
		Task<ApiResponse<BankResponseDto>> DisableAsync(Guid id);
	}
}
