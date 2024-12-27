using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyToDo.api.Context;
using MyToDo.api.Context.Repository;
using MyToDo.api.Extension;
using MyToDo.api.Service;
using MyToDo.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<MyToDoContext>(options => {

    var connectionString = builder.Configuration.GetConnectionString("ToDoConnection");

    options.UseSqlite(connectionString);

}).AddUnitOfWork<MyToDoContext>()
.AddCustomRepository<ToDo,ToDoRepository>()
.AddCustomRepository<Memo, MemoRepository>()
.AddCustomRepository<User, UserRepository>();

builder.Services.AddTransient<IToDoService, ToDoService>();
builder.Services.AddTransient<IMemoService, MemoService>();
builder.Services.AddTransient<ILoginService, LoginService>();


//ÅäÖÃÌí¼ÓAutoMapper
var autoMapperConfig = new MapperConfiguration(config =>
{
    config.AddProfile(new AutoMapperProFile());
});


builder.Services.AddSingleton(autoMapperConfig.CreateMapper());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


