using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.AbsenceReason;
using OutOfOffice.BLL.Services.IService;

namespace OutOfOffice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbsenceReasonController : ControllerBase
    {
        private readonly IAbsenceReasonService<AbsenceReasonDto, AbsenceReasonViewModel> _absenceReasonService;

        public AbsenceReasonController(IAbsenceReasonService<AbsenceReasonDto, AbsenceReasonViewModel> absenceReasonService)
        {
            _absenceReasonService = absenceReasonService;
        }

        [HttpPost]
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
        public async Task<IActionResult> GetAllAbsenceReasons()
        {
            var absenceReasons = await _absenceReasonService.GetAllAsync();
            return Ok(absenceReasons);
        }

        [HttpGet("{id}")]
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
