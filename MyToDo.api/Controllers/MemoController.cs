using Microsoft.AspNetCore.Mvc;
using MyToDo.api.Context;
using MyToDo.api.Parameters;
using MyToDo.api.Service;
using MyToDo.Api;
using MyToDo.Shared.Dtos;

namespace MyToDo.api.Controllers
{


    /// <summary>
    /// 备忘录控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MemoController : ControllerBase
    {
        private readonly IMemoService memoService;

        public MemoController(IMemoService memoService)
        {
         
            this.memoService = memoService;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int id)=> await memoService.GetSingleAsync(id);

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameter parameter) => await memoService.GetAllAsync(parameter);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] MemoDto model) => await memoService.AddAsync(model);

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] MemoDto model) => await memoService.updateAsync(model);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => await memoService.DeleteAsync(id);


    }
}
