namespace Zvezdichka.Web.Models
{
    public class UserWithRolesModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string[] Roles { get; set; }
    }
}