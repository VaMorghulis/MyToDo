using Microsoft.AspNetCore.Mvc;
using MyToDo.api.Context;
using MyToDo.api.Parameters;
using MyToDo.api.Service;
using MyToDo.Api;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.api.Controllers
{


    /// <summary>
    /// 待办事项控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService toDoService;

        public ToDoController(IToDoService toDoService)
        {
         
            this.toDoService = toDoService;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int id)=> await toDoService.GetSingleAsync(id);

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] ToDoParameter parameter) => await toDoService.GetAllAsync(parameter);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] ToDoDto model) => await toDoService.AddAsync(model);

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] ToDoDto model) => await toDoService.updateAsync(model);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => await toDoService.DeleteAsync(id);


    }
}
