using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Constants;
using QLHSNS.Data;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Payroll;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Payroll;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class PayrollService : IPayrollService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public PayrollService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ApiResponse<PayrollResponseDto>> CreateAsync(CreatePayrollRequestDto request) {
			try {
				if (request != null) {
					var newPayroll = _mapper.Map<Payroll>(request);

					var totalSalary = request.BasicSalary * (decimal)request.SalaryCoefficient;


					// Add new PayrollAllowace
					if (request.AllowaceIds != null && request.AllowaceIds.Count > 0) {
						foreach (var item in request.AllowaceIds!) {
							// if Allowace.Status == 1
							if (_dbContext.Allowances.Where(x => x.Id == item).Select(x => x.Status).FirstOrDefault() == 1) {
								var payrollAllowace = new PayrollAllowaceDto() {
									PayrollId = newPayroll.Id,
									AllowaceId = item
								};
								var newPayrollAllowace = _mapper.Map<PayrollAllowance>(payrollAllowace);
								await _dbContext.PayrollAllowances.AddAsync(newPayrollAllowace);

								// if Allowance is not OT
								if (_dbContext.Allowances.Where(x => x.Id == item && x.Status == 1).Select(x => x.Unit).FirstOrDefault() == "Month") {
									totalSalary += await _dbContext.Allowances.Where(x => x.Id == item && x.Status == 1)
																			.Select(x => x.Value).FirstOrDefaultAsync();
								}
							}
						}
					}

					// Add new PayrollBenefit
					if (request.BennefitIds != null && request.BennefitIds.Count > 0) {
						foreach (var item in request.BennefitIds!) {
							// if Benefit.Status == 1
							if (_dbContext.Benefits.Where(x => x.Id == item).Select(x => x.Status).FirstOrDefault() == 1) {
								var payrollBenefit = new PayrollBenefitDto() {
									PayrollId = newPayroll.Id,
									BenefitId = item
								};
								var newPayrollBenefit = _mapper.Map<PayrollBenefit>(payrollBenefit);
								await _dbContext.PayrollBenefits.AddAsync(newPayrollBenefit);

								totalSalary += await _dbContext.Benefits.Where(x => x.Id == item && x.Status == 1)
																		.Select(x => x.Amount).FirstOrDefaultAsync();
							}
						}
					}

					await _dbContext.Payrolls.AddAsync(newPayroll);
					await _dbContext.SaveChangesAsync();

					var result = _mapper.Map<PayrollResponseDto>(newPayroll);

					var queryAllowance = await _dbContext.PayrollAllowances.Include(x => x.Allowance)
												.Where(x => x.PayrollId == newPayroll.Id && x.Allowance.Status == 1)
												.Select(x => new PayrollAllowanceResponseDto {
													Id = x.AllowanceId,
													Name = x.Allowance.AllowanceName,
													Value = x.Allowance.Value,
													Unit = x.Allowance.Unit
												})
												.ToListAsync();

					var queryBenefit = await _dbContext.PayrollBenefits.Include(x => x.Benefit)
													.Where(x => x.PayrollId == newPayroll.Id && x.Benefit.Status == 1)
													.Select(x => new PayrollBenefitResponseDto {
														Id = x.BenefitId,
														Name = x.Benefit.BenefitName,
														Description = x.Benefit.Description,
														Amount = x.Benefit.Amount,
													}).ToListAsync();

					result.TotalSalary = totalSalary;
					result.Bonus = new BonusDto {
						Allowances = queryAllowance,
						Benefits = queryBenefit
					};

					return new ApiResponse<PayrollResponseDto> {
						Data = result,
						IsSuccess = true,
						Message = "Create new Payroll successfully"
					};
				}
				return new ApiResponse<PayrollResponseDto> {
					IsSuccess = true,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<PayrollResponseDto> {
					IsSuccess = true,
					Message = ex.Message
				};
			}
		}

		public async Task<bool> DeleteAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Payrolls.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return false;
				}

				var payrollAllowancesToDelete = await _dbContext.PayrollAllowances.Where(x => x.PayrollId == id).ToListAsync();
				var payrollBanefitsToDelete = await _dbContext.PayrollBenefits.Where(x => x.PayrollId == id).ToListAsync();

				_dbContext.Payrolls.Remove(dataFromDb);

				foreach (var item in payrollAllowancesToDelete) {
					_dbContext.PayrollAllowances.Remove(item);
				}

				foreach (var item in payrollBanefitsToDelete) {
					_dbContext.PayrollBenefits.Remove(item);
				}

				await _dbContext.SaveChangesAsync();

				return true;
			} catch (Exception) {

				throw;
			}
		}

		public async Task<ApiResponse<PayrollResponseDto>> DisableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Payrolls.Where(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<PayrollResponseDto> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND
					};
				}

				dataFromDb.Status = 1;
				dataFromDb.UpdatedAt = DateTime.Now;

				await _dbContext.SaveChangesAsync();

				var queryAllowance = await (from pa in _dbContext.PayrollAllowances.Where(x => x.AllowanceId == id)
											join a in _dbContext.Allowances.Where(x => x.Status == 1) on pa.AllowanceId equals a.Id
											select new PayrollAllowanceResponseDto {
												Id = a.Id,
												Name = a.AllowanceName,
												Value = a.Value,
												Unit = a.Unit,
											}).ToListAsync();

				var queryBenefit = await (from pb in _dbContext.PayrollBenefits.Where(x => x.BenefitId == id)
										  join b in _dbContext.Benefits.Where(x => x.Status == 1) on pb.BenefitId equals b.Id
										  select new PayrollBenefitResponseDto {
											  Id = b.Id,
											  Name = b.BenefitName,
											  Amount = b.Amount,
											  Description = b.Description
										  }).ToListAsync();

				var totalSalary = dataFromDb.BasicSalary * (decimal)dataFromDb.SalaryCoefficient;
				totalSalary += GetTotalSalary(queryAllowance, queryBenefit);

				var result = _mapper.Map<PayrollResponseDto>(dataFromDb);

				result.Bonus = new BonusDto {
					Allowances = queryAllowance,
					Benefits = queryBenefit
				};

				result.TotalSalary = totalSalary;

				return new ApiResponse<PayrollResponseDto> {
					Data = result,
					IsSuccess = true,
				};
			} catch (Exception ex) {
				return new ApiResponse<PayrollResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PayrollResponseDto>> EnableAsync(Guid id) {
			try {
				var dataFromDb = await _dbContext.Payrolls.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();

				if (dataFromDb == null) {
					return new ApiResponse<PayrollResponseDto> {
						IsSuccess = false,
						Message = Message.DATA_NOT_FOUND
					};
				}

				dataFromDb.Status = 0;
				dataFromDb.UpdatedAt = DateTime.Now;

				await _dbContext.SaveChangesAsync();

				var queryAllowance = await (from pa in _dbContext.PayrollAllowances.Where(x => x.AllowanceId == id)
											join a in _dbContext.Allowances.Where(x => x.Status == 1) on pa.AllowanceId equals a.Id
											select new PayrollAllowanceResponseDto {
												Id = a.Id,
												Name = a.AllowanceName,
												Value = a.Value,
												Unit = a.Unit,
											}).ToListAsync();

				var queryBenefit = await (from pb in _dbContext.PayrollBenefits.Where(x => x.BenefitId == id)
										  join b in _dbContext.Benefits.Where(x => x.Status == 1) on pb.BenefitId equals b.Id
										  select new PayrollBenefitResponseDto {
											  Id = b.Id,
											  Name = b.BenefitName,
											  Amount = b.Amount,
											  Description = b.Description
										  }).ToListAsync();

				var totalSalary = dataFromDb.BasicSalary * (decimal)dataFromDb.SalaryCoefficient;
				totalSalary += GetTotalSalary(queryAllowance, queryBenefit);

				var result = _mapper.Map<PayrollResponseDto>(dataFromDb);

				result.Bonus = new BonusDto {
					Allowances = queryAllowance,
					Benefits = queryBenefit
				};

				result.TotalSalary = totalSalary;

				return new ApiResponse<PayrollResponseDto> {
					Data = result,
					IsSuccess = true,
				};
			} catch (Exception ex) {
				return new ApiResponse<PayrollResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PayrollResponseDto>> GetByIdAsync(Guid id) {
			try {
				var dataFromDb = _dbContext.Payrolls.Where(x => x.Id == id).FirstOrDefault();

				if (dataFromDb == null) {
					return new ApiResponse<PayrollResponseDto> {
						IsSuccess = false,
						Message = "Data not found"
					};
				}

				var queryAllowance = await _dbContext.PayrollAllowances.Include(x => x.Allowance)
												.Where(x => x.PayrollId == id && x.Allowance.Status == 1)
												.Select(x => new PayrollAllowanceResponseDto {
													Id = x.AllowanceId,
													Name = x.Allowance.AllowanceName,
													Value = x.Allowance.Value,
													Unit = x.Allowance.Unit
												})
												.ToListAsync();

				var queryBenefit = await _dbContext.PayrollBenefits.Include(x => x.Benefit)
												.Where(x => x.PayrollId == id && x.Benefit.Status == 1)
												.Select(x => new PayrollBenefitResponseDto {
													Id = x.BenefitId,
													Name = x.Benefit.BenefitName,
													Description = x.Benefit.Description,
													Amount = x.Benefit.Amount,
												}).ToListAsync();

				var totalSalary = dataFromDb.BasicSalary * (decimal)dataFromDb.SalaryCoefficient;
				totalSalary += GetTotalSalary(queryAllowance, queryBenefit);

				var result = _mapper.Map<PayrollResponseDto>(dataFromDb);

				result.Bonus = new BonusDto {
					Allowances = queryAllowance,
					Benefits = queryBenefit
				};

				result.TotalSalary = totalSalary;

				return new ApiResponse<PayrollResponseDto> {
					IsSuccess = true,
					Data = result
				};
			} catch (Exception ex) {
				return new ApiResponse<PayrollResponseDto> {
					IsSuccess = true,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PagedResult<PayrollResponseDto>>> GetPayrollsAsync(PagingRequestBase request) {
			try {
				if (request != null) {
					var payrollList = await _dbContext.Payrolls
						.Skip((request.PageNumber - 1) * request.PageSize)
						.Take(request.PageSize).ToListAsync();

					if (payrollList == null || payrollList.Count == 0) {
						return new ApiResponse<PagedResult<PayrollResponseDto>> {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					int totalRecord = await _dbContext.Payrolls.CountAsync();

					var result = new List<PayrollResponseDto>();

					foreach (var item in payrollList) {
						var queryAllowance = await _dbContext.PayrollAllowances.Include(x => x.Allowance)
												.Where(x => x.PayrollId == item.Id && x.Allowance.Status == 1)
												.Select(x => new PayrollAllowanceResponseDto {
													Id = x.AllowanceId,
													Name = x.Allowance.AllowanceName,
													Value = x.Allowance.Value,
													Unit = x.Allowance.Unit
												})
												.ToListAsync();

						var queryBenefit = await _dbContext.PayrollBenefits.Include(x => x.Benefit)
														.Where(x => x.PayrollId == item.Id && x.Benefit.Status == 1)
														.Select(x => new PayrollBenefitResponseDto {
															Id = x.BenefitId,
															Name = x.Benefit.BenefitName,
															Description = x.Benefit.Description,
															Amount = x.Benefit.Amount,
														}).ToListAsync();

						var bonus = new BonusDto {
							Allowances = queryAllowance,
							Benefits = queryBenefit
						};

						// Caculator Total salary
						var totalSalary = item.BasicSalary * (decimal)item.SalaryCoefficient;
						totalSalary += GetTotalSalary(queryAllowance, queryBenefit);

						var temp = _mapper.Map<PayrollResponseDto>(item);

						temp.Bonus = bonus;
						temp.TotalSalary = totalSalary;

						result.Add(temp);
					}

					return new ApiResponse<PagedResult<PayrollResponseDto>> {
						Data = new PagedResult<PayrollResponseDto>(result, totalRecord, request.PageNumber, request.PageSize),
						IsSuccess = true,
					};
				}
				return new ApiResponse<PagedResult<PayrollResponseDto>> {
					IsSuccess = true,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<PagedResult<PayrollResponseDto>> {
					IsSuccess = true,
					Message = ex.Message
				};
			}
		}

		public async Task<ApiResponse<PayrollResponseDto>> UpdateAsync(UpdatePayrollRequestDto request) {
			try {
				if (request != null) {
					var dataFromDb = await _dbContext.Payrolls.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

					if (dataFromDb == null) {
						return new ApiResponse<PayrollResponseDto> {
							IsSuccess = false,
							Message = "Not found"
						};
					}

					dataFromDb.BasicSalary = request.BasicSalary;
					dataFromDb.SalaryCoefficient = request.SalaryCoefficient;
					dataFromDb.Notes = request.Notes;
					dataFromDb.UpdatedAt = DateTime.Now;

					var newTotalSalary = request.BasicSalary * (decimal)request.SalaryCoefficient;

					// Update Payroll Allowance
					if (request.AllowaceIds != null && request.AllowaceIds.Count > 0) {

						var currentPayrollAllowanceIds = await _dbContext.PayrollAllowances.Select(x => x.AllowanceId).ToListAsync();
						var allowanceToRemove = currentPayrollAllowanceIds.Except(request.AllowaceIds).ToList();
						var allowanceToAdd = request.AllowaceIds.Except(currentPayrollAllowanceIds).ToList();

						foreach (var item in allowanceToRemove) {
							var payrollAllowance = await _dbContext.PayrollAllowances.FirstOrDefaultAsync(x => x.AllowanceId == item);
							if (payrollAllowance != null) {
								_dbContext.PayrollAllowances.Remove(payrollAllowance);
							}
						}

						foreach (var item in allowanceToAdd) {
							var newPayrollAllowanceDto = new PayrollAllowaceDto {
								PayrollId = request.Id,
								AllowaceId = item
							};
							var newPayrollAllowance = _mapper.Map<PayrollAllowance>(newPayrollAllowanceDto);
							await _dbContext.PayrollAllowances.AddAsync(newPayrollAllowance);

							// if Allowance is not OT
							if (_dbContext.Allowances.Where(x => x.Id == item && x.Status == 1).Select(x => x.Unit).FirstOrDefault() != "Hour") {
								newTotalSalary += await _dbContext.Allowances.Where(x => x.Id == item && x.Status == 1)
																			.Select(x => x.Value).FirstOrDefaultAsync();
							}
						}
					}


					// Update Payroll Benefit
					if (request.BennefitIds != null && request.BennefitIds.Count > 0) {
						var currentParollBenefitIds = await _dbContext.PayrollBenefits.Select(x => x.BenefitId).ToListAsync();
						var benefitToAdd = request.BennefitIds.Except(currentParollBenefitIds).ToList();
						var benefitToRemove = currentParollBenefitIds.Except(request.BennefitIds).ToList();

						foreach (var item in benefitToRemove) {
							var payrollBenefit = await _dbContext.PayrollBenefits.FirstOrDefaultAsync(x => x.BenefitId == item);
							if (payrollBenefit != null) {
								_dbContext.PayrollBenefits.Remove(payrollBenefit);
							}
						}

						foreach (var item in benefitToAdd) {
							var newPayrollBenefitDto = new PayrollBenefitDto {
								PayrollId = request.Id,
								BenefitId = item
							};
							var newPayrollBenefit = _mapper.Map<PayrollBenefit>(newPayrollBenefitDto);
							await _dbContext.PayrollBenefits.AddAsync(newPayrollBenefit);

							newTotalSalary += await _dbContext.Benefits.Where(x => x.Id == item && x.Status == 1)
																	.Select(x => x.Amount).FirstOrDefaultAsync();
						}
					}

					await _dbContext.SaveChangesAsync();

					var result = _mapper.Map<PayrollResponseDto>(dataFromDb);

					var queryAllowance = await (from pa in _dbContext.PayrollAllowances.Where(x => x.PayrollId == dataFromDb.Id)
												join a in _dbContext.Allowances.Where(x => x.Status == 1) on pa.AllowanceId equals a.Id into temp
												from t in temp.DefaultIfEmpty()
												select new PayrollAllowanceResponseDto {
													Id = t.Id,
													Name = t.AllowanceName,
													Value = t.Value,
													Unit = t.Unit,
												}).ToListAsync();

					var queryBenefit = await (from pb in _dbContext.PayrollBenefits.Where(x => x.PayrollId == dataFromDb.Id)
											  join b in _dbContext.Benefits.Where(x => x.Status == 1) on pb.BenefitId equals b.Id into temp
											  from t in temp.DefaultIfEmpty()
											  select new PayrollBenefitResponseDto {
												  Id = t.Id,
												  Name = t.BenefitName,
												  Amount = t.Amount,
												  Description = t.Description,
											  }).ToListAsync();

					result.TotalSalary = newTotalSalary;
					result.Bonus = new BonusDto {
						Allowances = queryAllowance,
						Benefits = queryBenefit
					};

					return new ApiResponse<PayrollResponseDto> {
						Data = result,
						IsSuccess = true,
						Message = "Updated successfully"
					};
				}

				return new ApiResponse<PayrollResponseDto> {
					IsSuccess = false,
					Message = "Invalid payload"
				};
			} catch (Exception ex) {
				return new ApiResponse<PayrollResponseDto> {
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}


		private decimal GetTotalSalary(List<PayrollAllowanceResponseDto> AllowanceIds, List<PayrollBenefitResponseDto> BenefitsIds) {
			decimal totalSalary = 0;

			foreach (var item in AllowanceIds) {
				if (item.Unit == "Month") {
					totalSalary += item.Value;
				}
			}

			foreach (var item in BenefitsIds) {
				totalSalary += item.Amount;
			}

			return totalSalary;
		}
	}
}
