using be.Models;
using be.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace be.Controllers
{
    [Route("api/reception")]
    [ApiController]
    public class ReceptionController : ControllerBase
    {
        private readonly IReceptionService _receptionService;

        public ReceptionController(IReceptionService receptionService)
        {
            _receptionService = receptionService;
        }

        [HttpGet("all")]
        public ActionResult GetAllReceptions()
        {
            var result = _receptionService.GetAllReceptions();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        public ActionResult GetManagers(int id)
        {
            var result = _receptionService.GetReception(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        [HttpPut("update")]
        public ActionResult UpdateReception(User reception)
        {
            User foundManger = _receptionService.GetReception(reception.UserId);
            if (foundManger.FullName != reception.FullName)
            {
                reception.Email = _receptionService.GenerateEmail(reception.FullName);
            }
            _receptionService.UpdateReception(reception);
            return Ok();
        }

        [HttpPost]
        public ActionResult CreateReception(User reception)
        {
            reception.Email = _receptionService.GenerateEmail(reception.FullName);

            _receptionService.AddReception(reception);
            return Ok();
        }

        [HttpPut("updateStatus")]
        public ActionResult UpdateStatusManager(UserStatus userStatus)
        {
            _receptionService.UpdateStatusReception(userStatus);
            return Ok();
        }
    }
}
