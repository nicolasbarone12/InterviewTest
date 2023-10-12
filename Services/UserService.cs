using Repositories;
using Repositories.Interfaces;
using Repositories.ModelsDB;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService:IService
    {
        private IRepository _userRepository;

        public UserService()
        {
           this._userRepository = new UserRepository();
        }

        public bool RegisterUser(User newUser)
        {
            bool userRegistered = true;
            var userEntity = new UserEntity()
            {
                Id = 0,
                UserLastName = newUser.UserLastName,
                UserName = newUser.UserName,
                UserCode = newUser.UserCode,
                Password = newUser.Password
            };

            userRegistered = (this._userRepository.Insert(userEntity) == 1);

            return userRegistered;
        }

        public bool LogIn(User userData)
        {
            bool userFound = false;

            return userFound;
        }
    }
}
