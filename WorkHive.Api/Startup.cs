using Microsoft.EntityFrameworkCore;
using WorkHive.Api.Infrastructure;
using WorkHive.Data.Models;
using WorkHive.Data.Services;

namespace WorkHive.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new TrimStringConverter());
                });

            services.AddDbContext<WorkHiveDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserService, UserService>();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
