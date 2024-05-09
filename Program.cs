using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SoftworkMessanger.Services.Authentification;
using SoftworkMessanger.Services.Authentification.Jwt;
using SoftworkMessanger.Services.Repositories.Chats;
using SoftworkMessanger.Services.Repositories.Messages;
using SoftworkMessanger.Services.Repositories.Users;
using SoftworkMessanger.Utilites;

namespace SoftworkMessanger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region WebApplicationBuilder configuration

            var builder = WebApplication.CreateBuilder(args);

            #region DI container configuration

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    SecurityKey securityKey = new JwtOptions().SecurityKey;

                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = securityKey,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            #region Custom services

            #region Transient

            builder.Services.AddTransient<SqlServerConnector>();
            builder.Services.AddTransient<IJwtGenerator, JwtGenerator>();

            builder.Services.AddTransient<IJwtDecoder, JwtDecoder>();

            #endregion

            #region Scoped

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IAuthentificationService, AuthentificationService>();

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();
            builder.Services.AddScoped<IChatsRepository, ChatsRepository>();

            #endregion

            #endregion

            #endregion

            #endregion

            #region WebApplication configuration

            var app = builder.Build();

            #region Middleware configuration

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            });

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            #endregion

            app.Run();

            #endregion
        }
    }
}
