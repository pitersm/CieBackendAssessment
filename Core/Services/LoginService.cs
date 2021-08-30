using Core.Model;
using Core.Repository;
using System.Threading.Tasks;

namespace Core.Services
{
    public class LoginService
    {
        private readonly IRepository<User> _repository;

        public LoginService()
        {
        }

        public LoginService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<User> Login(string eMail)
        {
            var user = await _repository.GetByStringProperty("EMail", eMail);
            if (user == null) 
            {
                user = await _repository.Save(new User() { EMail = eMail });
            }

            return user;
        }
    }
}
