using System;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Infraestructure.Common;
using Sat.Recruitment.Infraestructure.IRepository;

namespace Sat.Recruitment.Infraestructure.Repository
{
	public class UserRepository : ERepository<User>, IUserRepository
	{
		public UserRepository(IQueryableUnitOfWork querybleUnitOfWork) : base(querybleUnitOfWork)
		{
		}
	}
}

