using AutoMapper;
using MyToDo.api.Context;
using MyToDo.Api;
using MyToDo.Shared.Dtos;

namespace MyToDo.api.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public LoginService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }
        public async Task<ApiResponse> LoginAsync(string Account, string password)
        {
            try
            {
                var model = await work.GetRepository<User>().GetFirstOrDefaultAsync(predicate: x => (x.Account.Equals(Account)) &&
                   (x.PassWord.Equals(password)));

                if (model == null)
                    return new ApiResponse("账号或密码错误");
                return new ApiResponse(true, model);
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, "登录异常");
            }
        }

        public async Task<ApiResponse> Register(UserDto user)
        {
            try
            {
                var model = mapper.Map<User>(user);

                var repository = work.GetRepository<User>();

                var userModel = await repository.GetFirstOrDefaultAsync(predicate: x => x.Account.Equals(model.Account));

                if (userModel != null)
                    return new ApiResponse(false, $"当前账户:{model.Account}已经存在");


                model.CreateDate = DateTime.Now;
                await repository.InsertAsync(model);

                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);

                return new ApiResponse(false, "注册失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, "注册异常");
            }
        }
    }
}
