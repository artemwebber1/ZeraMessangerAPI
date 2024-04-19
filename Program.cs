
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

            #region Custom services

            #region Scoped

            builder.Services.AddScoped<SqlServerConnector>();

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();

            #endregion

            #endregion

            #endregion

            #endregion

            #region WebApplication configuration

            var app = builder.Build();

            #region Middleware configuration

            // Конвейер обработки запроса
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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
