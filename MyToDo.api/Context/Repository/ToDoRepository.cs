using Microsoft.EntityFrameworkCore;
using MyToDo.Api;

namespace MyToDo.api.Context.Repository
{


    /// <summary>
    /// ToDo仓储
    /// </summary>
    public class ToDoRepository : Repository<ToDo>, IRepository<ToDo>
    {
        public ToDoRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
