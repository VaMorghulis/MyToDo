using AutoMapper;
using MyToDo.api.Context;
using MyToDo.api.Parameters;
using MyToDo.Api;
using MyToDo.Shared.Dtos;
using System.Reflection.Metadata;


namespace MyToDo.api.Service
{

    /// <summary>
    /// 待办事项的实现
    /// </summary>
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public ToDoService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }


        public async Task<ApiResponse> AddAsync(ToDoDto model)
        {
            try
            {
                var todo = mapper.Map<ToDo>(model);
                await work.GetRepository<ToDo>().InsertAsync(todo);

                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);

                return new ApiResponse(false, "添加数据失败");
            }
            catch (Exception ex)
            {

                return new ApiResponse(false, ex.Message);
            }

        }



        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {

                var repository = work.GetRepository<ToDo>();
                var todo = await work.GetRepository<ToDo>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(todo);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, "删除成功");
                return new ApiResponse(false, "删除失败");

            }
            catch (Exception ex)
            {

                return new ApiResponse(false, ex.Message);
            }
        }


        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {

                var repository = work.GetRepository<ToDo>();
                var todos = await repository.GetPagedListAsync(
                    predicate: x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search), pageIndex: parameter.PageIndex, pageSize: parameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.CreateDate));

                return new ApiResponse(true, todos);


            }
            catch (Exception ex)
            {

                return new ApiResponse(false, ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(Shared.Parameters.ToDoParameter query)
        {

            var repository = work.GetRepository<ToDo>();
            var todos = await repository.GetPagedListAsync(
            predicate: x => (string.IsNullOrWhiteSpace(query.Search) ? true : x.Title.Contains(query.Search)) && (query.Status == null ? true : x.Status.Equals(query.Status)), pageIndex: query.PageIndex, pageSize: query.PageSize,
            orderBy: source => source.OrderByDescending(t => t.CreateDate));
            return new ApiResponse(true, todos);
        }
        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todo = await work.GetRepository<ToDo>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, todo);

            }
            catch (Exception ex)
            {

                return new ApiResponse(false, ex.Message);
            }
        }

        public async Task<ApiResponse> updateAsync(ToDoDto model)
        {
            try
            {
                var dbTodo = mapper.Map<ToDo>(model);
                var repository = work.GetRepository<ToDo>();
                var todo = await work.GetRepository<ToDo>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbTodo.Id));

                todo.Title = dbTodo.Title;
                todo.Content = dbTodo.Content;
                todo.Status = dbTodo.Status;
                todo.UpdateDate = DateTime.Now;

                repository.Update(todo);
                if (await work.SaveChangesAsync() > 0)

                    return new ApiResponse(true, todo);

                return new ApiResponse(false, "更新数据异常");

            }
            catch (Exception ex)
            {

                return new ApiResponse(false, ex.Message);
            }
        }


    }
}
