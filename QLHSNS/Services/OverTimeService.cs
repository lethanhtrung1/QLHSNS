using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Constants;
using QLHSNS.Data;
using QLHSNS.DTOs.Request.OverTime;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.OverTime;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class OverTimeService : IOverTimeService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public OverTimeService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<OverTimeResponse>> AddOverTime(CreateOverTimeRequestDto request) {
			try {
				if (request != null) {
					var checkEmployee = await _dbContext.Employees.Where(x => x.Id == request.EmployeeId && x.IsWorking == 1).FirstOrDefaultAsync();

					if (checkEmployee == null) {
						return new ApiResponse<OverTimeResponse> {
							IsSuccess = false,
							Message = "Employee not found"
						};
					}

					var newOverTime = _mapper.Map<OverTime>(request);

					await _dbContext.OverTimes.AddAsync(newOverTime);
					await _dbContext.SaveChangesAsync();

					var overTimeFromDb = await _dbContext.OverTimes.FirstOrDefaultAsync(x => x.Id == newOverTime.Id);

					var result = _mapper.Map<OverTimeResponse>(overTimeFromDb);

					return new ApiResponse<OverTimeResponse> {
						Data = result,
						IsSuccess = true,
						Message = "Added successfully"
					};
				}
				return new ApiResponse<OverTimeResponse> {
					IsSuccess = false,
					Message = Message.INVALID_PAYLOAD
				};
			} catch (Exception ex) {
				return new ApiResponse<OverTimeResponse> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<OverTimeMonthReponse>> GetOverTimeMonthReponses(Guid employeeId, int month, int year) {
			try {
				var checkEmployee = await _dbContext.Employees.Where(x => x.Id == employeeId && x.IsWorking == 1).FirstOrDefaultAsync();

				if (checkEmployee == null) {
					return new ApiResponse<OverTimeMonthReponse> {
						IsSuccess = false,
						Message = "Employee not found"
					};
				}

				var totalHours = await _dbContext.OverTimes
					.Where(x => x.EmployeeId == employeeId && x.OverTimeDate.Month == month)
					.SumAsync(x => x.TotalHour);

				var result = new OverTimeMonthReponse {
					EmployeeId = employeeId,
					Month = month,
					Year = year,
					TotalHour = totalHours
				};

				return new ApiResponse<OverTimeMonthReponse> {
					Data = result,
					IsSuccess = true,
				};
			} catch (Exception ex) {
				return new ApiResponse<OverTimeMonthReponse> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<OverTimeResponse>>> GetOverTimesByEmployeeId(Guid employeeId) {
			try {
				var checkEmployee = await _dbContext.Employees.Where(x => x.Id == employeeId && x.IsWorking == 1).FirstOrDefaultAsync();

				if (checkEmployee == null) {
					return new ApiResponse<List<OverTimeResponse>> {
						IsSuccess = false,
						Message = "Employee not found"
					};
				}

				var data = await _dbContext.OverTimes.Where(x => x.EmployeeId == employeeId).ToListAsync();

				var result = _mapper.Map<List<OverTimeResponse>>(data);

				return new ApiResponse<List<OverTimeResponse>> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<List<OverTimeResponse>> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
