using Microsoft.AspNetCore.Mvc;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class PayrollBenefitController : ControllerBase {
		private readonly IPayrollBenefitService _service;

		public PayrollBenefitController(IPayrollBenefitService service) {
			_service = service;
		}

		[HttpPost("AddPayrollBenefits")]
		public async Task<IActionResult> AddPayrollBenefits(Guid payrollId, List<Guid> benefitIds) {
			var result = await _service.AddPayrollBenefits(payrollId, benefitIds);
			return result ? Ok() : BadRequest();
		}

		[HttpDelete("RemovePayrollBenefits")]
		public async Task<IActionResult> RemovePayrollBenefits(Guid payrollId, List<Guid> benefitIds) {
			var result = await _service.RemovePayrollBenefits(payrollId, benefitIds);
			return result ? Ok() : BadRequest();
		}
}
