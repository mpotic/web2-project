using AutoMapper;
using Business.Util;
using Data.UnitOfWork;

namespace Business.Services
{
	public class AdminService : IAdminServices
	{
		private IUnitOfWork _unitOfWork;

		private IMapper _mapper;

		private IUserHelper userHelper;

		public AdminService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			userHelper = new UserHelper(_unitOfWork);
		}
	}
}
