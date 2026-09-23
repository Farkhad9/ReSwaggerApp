using Microsoft.AspNetCore.Identity;

namespace ReApiSwagger.Models
{
    public class AppUser: IdentityUser
    {
        public string NickName { get; set; }
    }
}
