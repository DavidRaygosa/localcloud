using System.Text;
using Application.Contracts;
using Domain;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Security;
using WebAPI.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Persistence.DapperConnection;
using Persistence.DapperConnection.Pagination;
using Application;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options=>options.AddPolicy("AllowAccess_To_API",
            policy => policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            ));

            services.AddDbContext<DataBaseContext>(opt =>{
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            //services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);   

            // DAPPER
            services.Configure<ConnectionConf>(Configuration.GetSection("ConnectionStrings"));
            services.AddOptions();

            //
            services.AddMediatR(typeof(Consulta.Manejador).Assembly);
            services.AddControllers(opt=>{
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            }).AddFluentValidation(cfg=>cfg.RegisterValidatorsFromAssemblyContaining<Application.Users.Register>());
            
            // JWT - ROLES - IDENTITY FRAMEWORK
            var builder = services.AddIdentityCore<User>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddRoles<IdentityRole>(); // ROLES
            identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User,IdentityRole>>();
            identityBuilder.AddEntityFrameworkStores<DataBaseContext>();
            identityBuilder.AddSignInManager<SignInManager<User>>();
            services.TryAddSingleton<ISystemClock, SystemClock>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("My Secret Password Cloud"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt=>{
                opt.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserSession, UserSession>();
            services.AddAutoMapper(typeof(Consulta.Manejador));

            // STORED PROCEDURE
            services.AddTransient<IFactoryConnection, FactoryConnection>();
            services.AddScoped<IPagination, PaginationRepo>(); // INSTANCIAR PAGINACION INTERFACE

            // SWAGGER
            services.AddSwaggerGen(c=>{
                c.SwaggerDoc("v1", new OpenApiInfo{
                    Title = "LocalCloud Services for Maintenance",
                    Version = "v1"
                });
                c.CustomSchemaIds(c=>c.FullName); // GET FULL PATH
            });

            // LOAD INDEX.HTML
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAccess_To_API");

            app.UseMiddleware<ManejadorErrorMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // SWAGGER
            app.UseSwagger();
            app.UseSwaggerUI(c=>{
                c.SwaggerEndpoint("/swagger/v1/swagger.json","LocalCloud v1");
            });

            // LOAD INDEX.HTML
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "Client"; 
            });
        }
    }
}
