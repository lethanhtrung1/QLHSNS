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

		public async Task<ApiResponse<EmployeeFamilyResponseDto>> CreateAsync(CreateEmployeeFamilyRequestDto request) {
			try {
				if (request != null) {
					var checkEmployeeFamily = await _dbContext.EmployeeFamilies
						.Where(x => x.EmployeeId == request.EmployeeId && x.Name == request.Name).ToListAsync();

					if (checkEmployeeFamily != null) {
						return new ApiResponse<EmployeeFamilyResponseDto> {
							IsSuccess = false,
							Message = "Employee Family already exist"
						};
					}

					var newEmployeeFamily = new EmployeeFamily {
						Id = Guid.NewGuid(),
						EmployeeId = request.EmployeeId,
						Name = request.Name,
						DateOfBirth = request.DateOfBirth,
						PhoneNumber = request.PhoneNumber,
						Relationship = request.Relationship,
						Occupation = request.Occupation,
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

		public Task<ApiResponse<EmployeeFamilyResponseDto>> CreateRangeAsync(List<CreateEmployeeFamilyRequestDto> request) {
			throw new NotImplementedException();
		}

		public async Task<ApiResponse<EmployeeFamilyResponseDto>> DeleteAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.EmployeeFamilies.Where(x => x.Id == id).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<EmployeeFamilyResponseDto> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND,
					};
				}

				_dbContext.EmployeeFamilies.Remove(dataFromDb);
				await _dbContext.SaveChangesAsync();

				return new ApiResponse<EmployeeFamilyResponseDto> {
					IsSuccess = true,
					Message = Message.DELETED_SUCCESS
				};
			} catch (Exception ex) {
				return new ApiResponse<EmployeeFamilyResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<List<EmployeeFamilyResponseDto>>> GetByEmployeeIdAsync(Guid id) {
			try {
				var data = await _dbContext.EmployeeFamilies.Where(x => x.EmployeeId == id).ToListAsync();

				if (data == null || data.Count == 0) {
					return new ApiResponse<List<EmployeeFamilyResponseDto>> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND
					};
				}

				var result = _mapper.Map<List<EmployeeFamilyResponseDto>>(data).ToList();

				return new ApiResponse<List<EmployeeFamilyResponseDto>> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<List<EmployeeFamilyResponseDto>> {
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

					if (!string.IsNullOrWhiteSpace(request.Name))
						dataFromDb.Name = request.Name;

					if (request.DateOfBirth != default(DateTime))
						dataFromDb.DateOfBirth = request.DateOfBirth;

					if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
						dataFromDb.PhoneNumber = request.PhoneNumber;

					if (!string.IsNullOrWhiteSpace(request.Relationship))
						dataFromDb.Relationship = request.Relationship;

					if (!string.IsNullOrWhiteSpace(request.Occupation))
						dataFromDb.Occupation = request.Occupation;

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

		public Task<ApiResponse<EmployeeFamilyResponseDto>> UpdateRangeAsync(List<UpdateEmployeeFamilyRequestDto> request) {
			throw new NotImplementedException();
		}
	}
}
