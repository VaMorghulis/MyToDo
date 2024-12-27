using AutoMapper;
using MyToDo.api.Context;
using MyToDo.api.Parameters;
using MyToDo.Api;
using MyToDo.Shared.Dtos;

namespace MyToDo.api.Service
{

    /// <summary>
    /// 待办事项的实现
    /// </summary>
    public class MemoService : IMemoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public MemoService(IUnitOfWork work,IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }


        public async Task<ApiResponse> AddAsync(MemoDto model)
        {
            try
            {
                var memo = mapper.Map<Memo>(model);
                await work.GetRepository<Memo>().InsertAsync(memo);

                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, memo);

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

                var repository = work.GetRepository<Memo>();
                var memo = await work.GetRepository<Memo>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(memo);
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

                var repository = work.GetRepository<Memo>();
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



        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<Memo>();
                var memo = await work.GetRepository<Memo>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

                return new ApiResponse(true, memo);

            }
            catch (Exception ex)
            {

                return new ApiResponse(false, ex.Message);
            }
        }

        public async Task<ApiResponse> updateAsync(MemoDto model)
        {
            try
            {
                var dbTodo = mapper.Map<Memo>(model);
                var repository = work.GetRepository<Memo>();
                var todo = await work.GetRepository<Memo>().GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbTodo.Id));

                todo.Title = dbTodo.Title;
                todo.Content = dbTodo.Content;

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
