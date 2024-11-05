using Application.DTOs.Categories;
using Application.DTOs.Comments;
using Application.DTOs.Posts;
using Application.DTOs.Tags;
using Application.DTOs.Topics;
using Application.DTOs.Users;
using Application.Interfaces.Auth;
using Application.Mappings;
using Application.Services;
using Application.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using ForumPOF.Extensions;
using ForumPOF.Filters;
using ForumPOF.Middlewares;
using ForumPOF.ModelBinders;
using Infrastructure;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.Repository;
using Persistance.Repository.Interfaces;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ForumPOF;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddCors();

        builder.Services
            .AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new UlidEntityBinderProvider());
            })
            .AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        var assemblyApplication = Assembly.Load("Application");
        var assemblyPersistance = Assembly.Load("Persistance");
        var assemblyInfrastructure = Assembly.Load("Infrastructure");
        var assemblyPresentation = Assembly.Load("ForumPOF");

        builder.Services.AddValidatorsFromAssembly(assemblyApplication);
        builder.Services.AddFluentValidationAutoValidation();

        builder.Services.Scan(scan => scan
            .FromAssemblies(assemblyApplication, assemblyPersistance, assemblyInfrastructure, assemblyPresentation)
            .AddClasses(classes => classes.InNamespaces("Persistance.Repository"))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.InNamespaces("Application.Services"))
                .AsSelf()
                .WithScopedLifetime()
            .AddClasses(classes => classes.InNamespaces("Infrastructure"))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
             .AddClasses(classes => classes.InNamespaces("ForumPOF.Filters"))
                 .AsSelf()
                 .WithScopedLifetime()    
            );

        //builder.Services.AddScoped<IUserRepository, UserRepository>();
        //builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        //builder.Services.AddScoped<ITopicRepository, TopicRepository>();
        //builder.Services.AddScoped<ITagRepository, TagRepository>();
        //builder.Services.AddScoped<IPostRepository, PostRepository>();
        //builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        //builder.Services.AddScoped<IJwtProvider, JwtProvider>();
        //builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

        //builder.Services.AddScoped<UsersService>();
        //builder.Services.AddScoped<CategoriesService>();
        //builder.Services.AddScoped<TopicsService>();
        //builder.Services.AddScoped<TagsService>();
        //builder.Services.AddScoped<PostsService>();
        //builder.Services.AddScoped<CommentsService>();

        var mapsterConfig = new MapsterConfig();
        builder.Services.AddSingleton<MapsterConfig>();

        builder.Services.AddApiAuthentication(builder.Configuration);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ForumContext>(options =>
        {
            options
            .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            .EnableSensitiveDataLogging();

        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(options => options
            .WithOrigins(new[] {"http://localhost:3000"})
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
        );

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        //app.UseMiddleware<JwtMiddleware>();
        app.UseMiddleware<LoggerMiddleware>();

        app.UseHttpsRedirection();

        app.UseCookiePolicy(new CookiePolicyOptions()
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always,
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
