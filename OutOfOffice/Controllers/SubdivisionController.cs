using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.BLL.Services.IService;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.Subdivision;

namespace OutOfOffice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubdivisionController : ControllerBase
    {
        private readonly ISubdivisionService<SubdivisionDto, SubdivisionViewModel> _subdivisionService;

        public SubdivisionController(ISubdivisionService<SubdivisionDto, SubdivisionViewModel> subdivisionService)
        {
            _subdivisionService = subdivisionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddSubdivision([FromBody] SubdivisionDto subdivisionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subdivisionId = await _subdivisionService.AddAsync(subdivisionDto);
            return CreatedAtAction(nameof(GetSubdivisionById), new { id = subdivisionId }, subdivisionId);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubdivisions()
        {
            var subdivisions = await _subdivisionService.GelAllAsync();
            return Ok(subdivisions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubdivisionById(int id)
        {
            var subdivisions = await _subdivisionService.GelAllAsync();
            var subdivision = subdivisions.FirstOrDefault(s => s.Id == id);
            if (subdivision == null)
            {
                return NotFound();
            }

            return Ok(subdivision);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubdivision(int id, [FromBody] SubdivisionDto subdivisionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _subdivisionService.UpdateAsync(id, subdivisionDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
