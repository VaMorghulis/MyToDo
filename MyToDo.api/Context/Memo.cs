namespace MyToDo.api.Context
{
    public class Memo: BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

    }
}
