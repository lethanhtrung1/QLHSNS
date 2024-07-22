using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Constants;
using QLHSNS.Data;
using QLHSNS.DTOs.Request.EmployeeFamily;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.EmployeeFamily;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class EmployeeFamilyService : IEmployeeFamilyService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public EmployeeFamilyService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<EmployeeFamilyDetailResponseDto>> AddEmployeeFamilyDetail(AddEmployeeFamilyDetailRequestDto request) {
			try {
				if (request != null) {
					var checkEmployeeFamily = await _dbContext.EmployeeFamilies.Where(x => x.Id == request.EmployeeFamilyId).FirstOrDefaultAsync();

					if(checkEmployeeFamily == null) {
						return new ApiResponse<EmployeeFamilyDetailResponseDto> {
							IsSuccess = false,
							Message = "Employee family not found"
						};
					}

					var newFamilyDetail = _mapper.Map<EmployeeFamilyDetail>(request);

					await _dbContext.EmployeeFamilyDetails.AddAsync(newFamilyDetail);
					await _dbContext.SaveChangesAsync();

					var query = await _dbContext.EmployeeFamilyDetails.FirstOrDefaultAsync(x => x.Id == newFamilyDetail.Id);
					var result = _mapper.Map<EmployeeFamilyDetailResponseDto>(query);

					return new ApiResponse<EmployeeFamilyDetailResponseDto> {
						IsSuccess = true,
						Data = result,
						Message = "Add new Employee Family successfully"
					};
				}

				return new ApiResponse<EmployeeFamilyDetailResponseDto> {
					IsSuccess = false,
					Message = Message.INVALID_PAYLOAD
				};
			} catch (Exception ex) {
				return new ApiResponse<EmployeeFamilyDetailResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<EmployeeFamilyResponseDto>> CreateAsync(CreateEmployeeFamilyRequestDto request) {
			try {
				if (request != null) {
					var checkEmployeeFamily = await _dbContext.EmployeeFamilies
						.Where(x => x.EmployeeId == request.EmployeeId).ToListAsync();

					if (checkEmployeeFamily != null) {
						return new ApiResponse<EmployeeFamilyResponseDto> {
							IsSuccess = false,
							Message = "Employee Family already exist"
						};
					}

					var newEmployeeFamily = new EmployeeFamily {
						Id = Guid.NewGuid(),
						EmployeeId = request.EmployeeId,
						Deduction = request.Deduction,
						EffectiveDate = request.EffectiveDate,
					};

					await _dbContext.EmployeeFamilies.AddAsync(newEmployeeFamily);
					await _dbContext.SaveChangesAsync();

					return new ApiResponse<EmployeeFamilyResponseDto> {
						IsSuccess = true,
					};
				}

				return new ApiResponse<EmployeeFamilyResponseDto> {
					IsSuccess = false,
					Message = Message.INVALID_PAYLOAD
				};
			} catch (Exception ex) {
				return new ApiResponse<EmployeeFamilyResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<bool> DeleteAsync(Guid id) {
			try {
				var employeeFamily = await _dbContext.EmployeeFamilies.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (employeeFamily == null) return false;

				var employeeFamilyDetails = await _dbContext.EmployeeFamilyDetails.Where(x => x.EmployeeFamilyId == id).ToListAsync();

				_dbContext.EmployeeFamilyDetails.RemoveRange(employeeFamilyDetails);
				_dbContext.EmployeeFamilies.Remove(employeeFamily);
				await _dbContext.SaveChangesAsync();

				return true;
			} catch (Exception) {
				throw;
			}
		}

		public async Task<ApiResponse<GetEmployeeFamilyWithDetailResponseDto>> GetByEmployeeIdAsync(Guid id) {
			try {
				var data = await _dbContext.EmployeeFamilies.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();

				if (data == null) {
					return new ApiResponse<GetEmployeeFamilyWithDetailResponseDto> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND
					};
				}

				var result = _mapper.Map<GetEmployeeFamilyWithDetailResponseDto>(data);

				var query = await _dbContext.EmployeeFamilyDetails.Where(x => x.EmployeeFamilyId == data.Id).ToListAsync();
				var familyDetails = _mapper.Map<List<EmployeeFamilyDetailResponseDto>>(query);

				result.FamilyDetails = familyDetails;

				return new ApiResponse<GetEmployeeFamilyWithDetailResponseDto> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<GetEmployeeFamilyWithDetailResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<EmployeeFamilyResponseDto>> UpdateAsync(UpdateEmployeeFamilyRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.EmployeeFamilies.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

					if (dataFromDb == null) {
						return new ApiResponse<EmployeeFamilyResponseDto> {
							IsSuccess = false,
							Message = Message.DATA_NOT_FOUND
						};
					}

					if (request.EffectiveDate != default(DateTime))
						dataFromDb.EffectiveDate = request.EffectiveDate;

					if (request.Deduction == 0 || request.Deduction == 1)
						dataFromDb.Deduction = request.Deduction;

					if (request.EmployeeId.HasValue)
						dataFromDb.EmployeeId = request.EmployeeId.Value;

					await _dbContext.SaveChangesAsync();

					var query = await _dbContext.EmployeeFamilies.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
					var result = _mapper.Map<EmployeeFamilyResponseDto>(query);

					return new ApiResponse<EmployeeFamilyResponseDto> {
						Data = result,
						IsSuccess = true,
						Message = Message.UPDATED_SUCCESS
					};
				}

				return new ApiResponse<EmployeeFamilyResponseDto> {
					IsSuccess = false,
					Message = Message.INVALID_PAYLOAD
				};
			} catch (Exception ex) {
				return new ApiResponse<EmployeeFamilyResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}
	}
}
