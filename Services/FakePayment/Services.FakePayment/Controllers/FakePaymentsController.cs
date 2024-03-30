using Education.Shared.ControllerBases;
using Education.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomBaseController
    {

        [HttpPost]
        public async Task<IActionResult> ReceivePayment()
        {
            return CreateActionResultInstance<NoContent>(Response<NoContent>.Success(200));
        }
    }
}
