using Microsoft.AspNetCore.Mvc;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class PayrollAllowanceController : ControllerBase {
		private readonly IPayrollAllowanceService _service;

		public PayrollAllowanceController(IPayrollAllowanceService service) {
			_service = service;
		}

		[HttpPost("AddPayrollAllowances")]
		public async Task<IActionResult> AddPayrollAllowances(Guid payrollId, List<Guid> allowanceIds) {
			var result = await _service.AddPayrollAllowances(payrollId, allowanceIds);
			return result ? Ok() : BadRequest();
		}

		[HttpDelete("RemovePayrollAllowances")]
		public async Task<IActionResult> RemovePayrollAllowances(Guid payrollId, List<Guid> allowanceIds) {
			var result = await _service.RemovePayrollAllowances(payrollId, allowanceIds);
			return result ? Ok() : BadRequest();
		}
	}
}
