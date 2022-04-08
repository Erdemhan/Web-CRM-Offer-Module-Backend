using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using crmweb.Common.Auxiliary;
using crmweb.Common.Extensions;
using crmweb.Models.UserModels;
using crmweb.Services;

namespace crmweb.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AuthService AuthService;
        private readonly UserService UserService;

        public UserController(AuthService AuthService, UserService UserService)
        {
            this.AuthService = AuthService;
            this.UserService = UserService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery query)
        {
            if (!ModelState.IsValid)
                return Json(Result<LoginInfo>.PrepareFailure(""));
            User.GetSubject<int>();
            Result<LoginInfo> vResult = await AuthService.AuthForLogIn(query);
            return Ok(vResult);
        }

        [HttpGet]
        [Route("infobyid/{Id}")]
        public async Task<Result<List<UserInfo>>> GetUserbyId(int Id)
        {
            return await UserService.GetUserbyId(Id);
        }

        [HttpGet]
        [Route("list")]
        public async Task<Result<List<UserInfo>>> GetList()
        {
            return await UserService.GetUserList();
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestInfo user)
        {
            if (!ModelState.IsValid)
                return Json(Result.PrepareFailure(""));

            return Ok(await UserService.CreateUser(user)) ;
        }


        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> PutUser([FromBody] UserUpdateInfo user)
        {
            return Ok(await UserService.PutUser(user));
        }

        [HttpGet]
        [Route("active/{Id}")]
        public async Task<Result<UserInfo>> Activate(int Id)
        {
            return await UserService.ActivateUser(Id);
        }

        [HttpGet]
        [Route("role/{Id}")]
        public async Task<Result<UserInfo>> changeUserRole(int Id)
        {
            return await UserService.ChangeUserRole(Id);
        }


        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            return Ok(await UserService.DeleteUser(Id));
        }


    }
}
