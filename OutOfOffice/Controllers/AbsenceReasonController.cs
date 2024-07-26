using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.AbsenceReason;
using OutOfOffice.BLL.Services.IService;
using Microsoft.AspNetCore.Authorization;

namespace OutOfOffice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AbsenceReasonController : ControllerBase
    {
        private readonly IAbsenceReasonService<AbsenceReasonDto, AbsenceReasonViewModel> _absenceReasonService;

        public AbsenceReasonController(IAbsenceReasonService<AbsenceReasonDto, AbsenceReasonViewModel> absenceReasonService)
        {
            _absenceReasonService = absenceReasonService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAbsenceReason([FromBody] AbsenceReasonDto absenceReasonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var absenceReasonId = await _absenceReasonService.AddAsync(absenceReasonDto);
            return CreatedAtAction(nameof(GetAbsenceReasonById), new { id = absenceReasonId }, absenceReasonId);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAbsenceReasons()
        {
            var absenceReasons = await _absenceReasonService.GetAllAsync();
            return Ok(absenceReasons);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAbsenceReasonById(int id)
        {
            var absenceReasons = await _absenceReasonService.GetAllAsync();
            var absenceReason = absenceReasons.FirstOrDefault(ar => ar.Id == id);
            if (absenceReason == null)
            {
                return NotFound();
            }

            return Ok(absenceReason);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAbsenceReason(int id, [FromBody] AbsenceReasonDto absenceReasonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _absenceReasonService.UpdateAsync(id, absenceReasonDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
