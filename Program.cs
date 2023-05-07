global using Biddo.Services.AuthServices;
global using Biddo.Services.EventServices;
global using Biddo.Services.MailServices;
global using Biddo.Services.ChatServices;
global using Biddo.Services.HelpServices;
global using Biddo.Services.AdminServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Biddo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BiddoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BiddoContext") ?? throw new InvalidOperationException("Connection string 'BiddoContext' not found.")));
            // Add JWT Middleware
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

            // Add CORS
            builder.Services.AddCors(options => options.AddPolicy(name : "NgOrigins", policy =>
            {
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            }));

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IHelpService, HelpService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddHttpContextAccessor();
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

            app.UseCors("NgOrigins");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}