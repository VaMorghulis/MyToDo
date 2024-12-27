using Microsoft.EntityFrameworkCore;
using MyToDo.Api;

namespace MyToDo.api.Context.Repository
{
    /// <summary>
    /// ToDo仓储
    /// </summary>
    public class MemoRepository : Repository<Memo>, IRepository<Memo>
    {
        public MemoRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
