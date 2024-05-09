using System.Text;
using FeedService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtKey = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();