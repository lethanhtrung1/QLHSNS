using QLHSNS.DTOs.Request.OverTime;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.OverTime;

namespace QLHSNS.Services.IServices {
	public interface IOverTimeService {
		Task<ApiResponse<OverTimeResponse>> AddOverTime(CreateOverTimeRequestDto request);
		Task<ApiResponse<List<OverTimeResponse>>> GetOverTimesByEmployeeId(Guid employeeId);
		Task<ApiResponse<OverTimeMonthReponse>> GetOverTimeMonthReponses(Guid employeeId, int month, int year);
	}
}
