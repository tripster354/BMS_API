using BMS_API.Services;
using BMS_API.Services.Interface;
using BMS_API.Services.Interface.Partner;
using BMS_API.Services.Interface.User;
using BMS_API.Services.Partner;
using BMS_API.Services.User;
using BudgetManagement.Models;
using BudgetManagement.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace BudgetManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            var contentRoot = env.WebRootPath;
        }
        const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IWebHostEnvironment rootPath { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Db context to the container.


            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BMSContext>(options =>
            options.UseSqlServer(
            Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();


            // Add services to the container.
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<lCountryServices, CountryServices>();
            services.AddScoped<IStateServices, StateServices>();
            services.AddScoped<ICityServices, CityServices>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IInterestService, InterestService>();
            services.AddScoped<IFAQService, FAQService>();
            services.AddScoped<ITicketIssueTypeService, TicketIssueTypeService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IPageMenuService, PageMenuService>();
            services.AddScoped<ItblPartnerService, tblPartnerService>();
            services.AddScoped<ItblUserService, tblUserService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<ISubAdminService, SubAdminService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<lBankServices, BankServices>();
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFeedService, FeedService>();
            services.AddScoped<IPostsService, PostService>();

         //   services.AddMvcCore().AddApiExplorer();

           services.AddSwaggerGen();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v2", new OpenApiInfo
            //    {
            //        Version = "v2",
            //        Title = "Implement Swagger UI",
            //        Description = "A simple example to Implement Swagger UI",
            //    });
            //});
             
           /*  services.AddCors(options =>
             {
                 options.AddPolicy(name: MyAllowSpecificOrigins,
                     policy =>
                     {
                         policy.WithOrigins("*", "http://localhost:4200", "http://localhost:61372", "http://localhost:61492", "https://bms.fusioninformatics.net", "https://bms-pwa-vendor.fusioninformatics.net", "https://bms-pwa.fusioninformatics.net");
                         //policy.WithOrigins("*");
                         policy.AllowAnyMethod();
                         policy.AllowAnyHeader();
                         policy.AllowCredentials();
                     });
             });*/
            /*
            */
            //services.Configure<IISServerOptions>(options =>
            //{
            //    options.MaxRequestBodySize = 2147483648;
            //});

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = string.Empty;
            });
            app.UseStaticFiles(); // Serve static files
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
                RequestPath = "/Uploads",
                EnableDirectoryBrowsing = true
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
