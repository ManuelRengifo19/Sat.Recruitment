using System;
using Sat.Recruitment.Core.Enums;
using Sat.Recruitment.Core.IServices;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Infraestructure.IRepository;

namespace Sat.Recruitment.Core.Services
{
	public class UserService : Service<User>, IUserService
	{
		public UserService(IUserRepository userRepository) : base(userRepository)
		{
		}

        public async Task<bool> ValidateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "The User object cannot be null.");
            }

            if (string.IsNullOrEmpty(user.Name))
            {
                throw new ArgumentException("The Name field cannot be null or empty.", nameof(user.Name));
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("The Email field cannot be null or empty.", nameof(user.Email));
            }

            if (string.IsNullOrEmpty(user.Address))
            {
                throw new ArgumentException("The Address field cannot be null or empty.", nameof(user.Address));
            }

            if (string.IsNullOrEmpty(user.Phone))
            {
                throw new ArgumentException("The Phone field cannot be null or empty.", nameof(user.Phone));
            }

            ValidateTypeOfUser(user);

            return await this.AddAsync(user).ConfigureAwait(false);
        }

        protected void ValidateTypeOfUser(User user)
        {
            if (user.UserType == UserTypeEnum.Normal.ToString())
            {
                if (user.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.12);
                    //If new user is normal and has more than USD100
                    var gif = user.Money * percentage;
                    user.Money = user.Money + gif;
                }
                if (user.Money < 100)
                {
                    if (user.Money > 10)
                    {
                        var percentage = Convert.ToDecimal(0.8);
                        var gif = user.Money * percentage;
                        user.Money = user.Money + gif;
                    }
                }
            }
            if (user.UserType == UserTypeEnum.SuperUser.ToString())
            {
                if (user.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.20);
                    var gif = user.Money * percentage;
                    user.Money = user.Money + gif;
                }
            }
            if (user.UserType == UserTypeEnum.Premuin.ToString())
            {
                if (user.Money > 100)
                {
                    var gif = user.Money * 2;
                    user.Money = user.Money + gif;
                }
            }
        }
    }
}

