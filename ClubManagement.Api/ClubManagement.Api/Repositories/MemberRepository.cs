using ClubManagement.Api.Models;
using Microsoft.Data.SqlClient;

namespace ClubManagement.Api.Repositories
{


public class MemberRepository : IMemberRepository
    {
        private readonly string _connectionString;

        public MemberRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ActiveMemberDto>> GetActiveMembersAsync(
            string sortColumn,
            int? pageNumber,
            int? pageSize)
        {
            var results = new List<ActiveMemberDto>();

            var sql = @"
SELECT
    m.MemberId,
    m.FirstName,
    m.LastName,
    m.Email,
    ms.MembershipType,
    c.ClubName AS HomeClub,
    m.JoinDate
FROM Members m
INNER JOIN Memberships ms ON m.MemberId = ms.MemberId
INNER JOIN Clubs c ON m.ClubId = c.ClubId
WHERE ms.IsActive = 1
AND c.IsActive = 1
";

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                sql += @"
ORDER BY
    CASE WHEN @sortColumn = 'FirstName' THEN m.FirstName END,
    CASE WHEN @sortColumn = 'LastName' THEN m.LastName END,
    CASE WHEN @sortColumn = 'Email' THEN m.Email END,
    CASE WHEN @sortColumn = 'MembershipType' THEN ms.MembershipType END,
    CASE WHEN @sortColumn = 'HomeClub' THEN c.ClubName END,
    CASE WHEN @sortColumn = 'JoinDate' THEN m.JoinDate END
OFFSET @Offset ROWS
FETCH NEXT @pageSize ROWS ONLY;
";
            }
            else
            {
                sql += @"
ORDER BY m.LastName, m.FirstName;
";
            }


            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@SortColumn", sortColumn);

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                cmd.Parameters.AddWithValue("@Offset", (pageNumber.Value - 1) * pageSize.Value);
                cmd.Parameters.AddWithValue("@pageSize", pageSize.Value);
            }


            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new ActiveMemberDto
                {
                    MemberId = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    MembershipType = reader.GetString(4),
                    HomeClub = reader.GetString(5),
                    JoinDate = reader.GetDateTime(6)
                });
            }

            return results;
        }
    }

}
