
using Core.Model;
using Core.Repository;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CieBackendAssessment.Controllers
{
    public class UserController : BaseController
    {
        private readonly IRepository<User> _repository;
        private readonly LoginService _loginService;

        public UserController(IRepository<User> repository, LoginService loginService)
        {
            _repository = repository;
            _loginService = loginService;
        }

        /// <summary>
        /// Logs a given user in
        /// </summary>
        /// <param name="eMail" example="piter.machado@x-team.com">The user's email address</param>
        [HttpGet("{eMail}")]
        public async Task<ActionResult<User>> Login(string eMail)
        {
            var value = await _loginService.Login(eMail);

            return Ok(value);
        }

        /// <summary>
        /// Gets a specific user by its unique id
        /// </summary>
        /// <param name="id" example="1">The user id</param>
        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult<User>> Get(long id)
        {
            var value = await _repository.Get(id);

            if (value != null)
            {
                return Ok(value);
            }
            else
            {
                return NotFound("There is no user that matches the id you informed. Please try again with another Id parameter.");
            }
        }
    }
}
