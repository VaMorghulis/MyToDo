namespace MyToDo.api.Context
{
    public class ToDo: BaseEntity
    {
        public string  Title { get; set; }=string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
