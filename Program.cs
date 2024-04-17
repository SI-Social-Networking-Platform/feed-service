using FeedService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient("UserServiceClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["UserService:BaseUrl"]);
});

builder.Services.AddHttpClient("PostServiceClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PostService:BaseUrl"]);
});

builder.Services.AddTransient<IUserServiceClient, UserServiceClient>();
builder.Services.AddTransient<IPostServiceClient, PostServiceClient>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();