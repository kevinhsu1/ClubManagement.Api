namespace ClubManagement.Api.Models
{
    public class ActiveMemberDto
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MembershipType { get; set; }
        public string HomeClub { get; set; }
        public DateTime JoinDate { get; set; }
    }

}
