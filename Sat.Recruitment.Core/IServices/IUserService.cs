using System;
using Sat.Recruitment.Domain.Entities;

namespace Sat.Recruitment.Core.IServices
{
	public interface IUserService : IService<User>
	{
        Task<bool> ValidateUserAsync(User user);
    }
}

