using Infraestructure.Errors;
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
            string filters = $" Where UserCode='{newUser.UserCode}'";
            var usersFound = this._userRepository.GetByFilters(filters);
            if (usersFound.Any())
            {
                throw new UserException($"An user with user code = {newUser.UserCode} already exists.");
            }
            else
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
            
        }

        

        public User Login(string userCode, string pass)
        {
            User userfound = null;

            string filters = $" Where UserCode='{userCode}' AND Pass='{pass}'";
            var usersFound = this._userRepository.GetByFilters(filters);

            if(usersFound is not null)
            {
                if (usersFound.Count() == 0)
                    throw new UserException("User not found, or invalid password.");
                else if (usersFound.Count() > 1)
                    throw new UserException("More than one user found.");
                else
                {

                    UserEntity userEntity = (UserEntity)usersFound.FirstOrDefault();

                    userfound = new User()
                    {
                        Id = userEntity.Id,
                        UserCode = userEntity.UserCode,
                        UserLastName = userEntity.UserLastName,
                        UserName = userEntity.UserName,
                    };
                }
            }
            

            return userfound;
        }
    }
}
