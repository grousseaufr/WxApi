using System.Collections.Generic;
using System.Linq;
using WxApi.Dtos.User;

namespace WxApi.Services
{
    public class UserService : IUserService
    {
        private List<User> users;

        public UserService()
        {
            users = new List<User>
            {
                new User
                {
                    Name = "Grégory Rousseau",
                    Token = "727f998f-1c6f-4fef-9619-973af5ffcccd"
                }
            };
        }

        public User Get()
        {
            return users.First();
        }
    }
}
