using ClubManagement.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ClubManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _repo;

        public MembersController(IMemberRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveMembers(
    string sortColumn = "LastName",
    int? pageNumber = null,
    int? pageSize = null)
        {
            // Validate sort column
            var validSortColumns = new[]
            {
        "FirstName", "LastName", "Email",
        "MembershipType", "HomeClub", "JoinDate"
    };

            if (!validSortColumns.Contains(sortColumn))
                return BadRequest($"Invalid sort column: {sortColumn}");

            // Validate pagination
            if (pageNumber.HasValue && pageNumber <= 0)
                return BadRequest("pageNumber must be greater than 0.");

            if (pageSize.HasValue && pageSize <= 0)
                return BadRequest("pageSize must be greater than 0.");

            // If only one pagination value is provided, reject it
            if (pageNumber.HasValue ^ pageSize.HasValue)
                return BadRequest("Both pageNumber and pageSize must be provided together.");

            var members = await _repo.GetActiveMembersAsync(sortColumn, pageNumber, pageSize);
            return Ok(members);
        }

    }
}
