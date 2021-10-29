using AcademyProject.Models;
using AcademyProject.Repositories;
using AcademyProject.Repositories.Implements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AcademyProject.Services;
using AcademyProject.Services.Implements;

namespace AcademyProject
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
            // CORS Configure
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(Configuration["CORSConfig:AllowedHosts"])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<AcademyProjectContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IBlogCommentRepository, BlogCommentRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPictureRepository, PictureRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IRequirementRepository, RequirementRepository>();
            services.AddScoped<ITrackRepository, TrackRepository>();
            services.AddScoped<ITrackStepRepository, TrackStepRepository>();
            services.AddScoped<IWillLearnRepository, WillLearnRepository>();

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IBlogCommentService, BlogCommentService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IRequirementService, RequirementService>();
            services.AddScoped<ITrackService, TrackService>();
            services.AddScoped<ITrackStepService, TrackStepService>();
            services.AddScoped<IWillLearnService, WillLearnService>();

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers();

            // Configure Authentication
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWTConfig:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWTConfig:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTConfig:AccessTokenSecret"]))
                };
            });

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AcademyProject", Version = "v1" });
                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AcademyProject v1"));
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
