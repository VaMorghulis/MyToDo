using MyToDo.api.Parameters;

namespace MyToDo.api.Service
{
    public interface IBaseService<T>
    {

        Task<ApiResponse> GetAllAsync(QueryParameter query);

        Task<ApiResponse> GetSingleAsync(int id);

        Task<ApiResponse> AddAsync(T model);
        Task<ApiResponse> updateAsync(T model);
        Task<ApiResponse> DeleteAsync(int id);


    }
}
