using MyToDo.Api;

namespace MyToDo.api.Context.Repository
{

    /// <summary>
    /// ToDo仓储
    /// </summary>
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
