using ClubManagement.Api.Models;

namespace ClubManagement.Api.Repositories
{
    public interface IMemberRepository
    {
        Task<IEnumerable<ActiveMemberDto>> GetActiveMembersAsync(
            string sortColumn,
            int? pageNumber,
            int? pageSize);
    }

}
