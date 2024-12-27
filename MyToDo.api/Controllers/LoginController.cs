using Microsoft.AspNetCore.Mvc;
using MyToDo.api.Context;
using MyToDo.api.Parameters;
using MyToDo.api.Service;
using MyToDo.Api;
using MyToDo.Shared.Dtos;

namespace MyToDo.api.Controllers
{


    /// <summary>
    /// 登录控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {

        private readonly ILoginService loginService;

        public LoginController(ILoginService loginService)
        {

            this.loginService = loginService;
        }

        [HttpGet]
        public async Task<ApiResponse> Login(string account, string passWord) => await loginService.LoginAsync(account, passWord);

        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto parameter) => await loginService.Register(parameter);




    }
}
