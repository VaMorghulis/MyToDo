
using MyToDo.Common.Models;
using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    internal class ToDoService : BaseService<ToDoDto>, IToDoService
    {
        private readonly HttpRestClient client;

        public ToDoService(HttpRestClient client):base(client,"ToDo")
        {
            this.client=client;
           
        }

        public async Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/ToDo/GetAll?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}"+
                $"&status={parameter.Status}";

            if (!string.IsNullOrEmpty(parameter.Search))
            {
                // Only add the Search parameter if it has a value.
                request.Route = $"api/ToDo/GetAll?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" + $"&Search={parameter.Search}"+
                $"&status={parameter.Status}";
            }
            return await client.ExecuteAsync<PagedList<ToDoDto>>(request);
        }
    }
}
