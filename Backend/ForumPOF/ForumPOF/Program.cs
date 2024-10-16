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
using ForumPOF.Middlewares;
using ForumPOF.ModelBinders;
using Infrastructure;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.Repository;
using Persistance.Repository.Interfaces;
using System.Text.Json.Serialization;

namespace ForumPOF;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddCors();

        //builder.Services.AddControllers();

        builder.Services.AddControllers(options =>
        {
            options.ModelBinderProviders.Insert(0, new UlidEntityBinderProvider());
        }).AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ITopicRepository, TopicRepository>();
        builder.Services.AddScoped<ITagRepository, TagRepository>();
        builder.Services.AddScoped<IPostRepository, PostRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IJwtProvider, JwtProvider>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        builder.Services.AddScoped<UsersService>();
        builder.Services.AddScoped<CategoriesService>();
        builder.Services.AddScoped<TopicsService>();
        builder.Services.AddScoped<TagsService>();
        builder.Services.AddScoped<PostsService>();
        builder.Services.AddScoped<CommentsService>();

        builder.Services.AddSingleton(() =>  //Добавляем конфиг
        {
            var config = new TypeAdapterConfig();

            new RegisterMapper().Register(config);

            return config;
        });

        builder.Services.AddScoped<IValidator<LoginUserRequest>, LoginUserValidator>();
        builder.Services.AddScoped<IValidator<RegisterUserRequest>, RegisterUserValidator>();
        builder.Services.AddScoped<IValidator<ReestablishUserRequest>, ReestablishUserValidator>();
        builder.Services.AddScoped<IValidator<UserUpdateRequest>, UserUpdateValidator>();
        builder.Services.AddScoped<IValidator<CategoryCreateRequest>, CategoryCreateValidator>();
        builder.Services.AddScoped<IValidator<CategoryUpdateRequest>, CategoryUpdateValidator>();
        builder.Services.AddScoped<IValidator<CommentCreateRequest>, CommentCreateValidator>();
        builder.Services.AddScoped<IValidator<CommentUpdateRequest>, CommentUpdateValidator>();
        builder.Services.AddScoped<IValidator<TagCreateRequest>, TagCreateValidator>();
        builder.Services.AddScoped<IValidator<TagUpdateRequest>, TagUpdateValidator>();
        builder.Services.AddScoped<IValidator<PostCreateRequest>, PostCreateValidator>();
        builder.Services.AddScoped<IValidator<PostUpdateRequest>, PostUpdateValidator>();
        builder.Services.AddScoped<IValidator<TopicCreateRequest>, TopicCreateValidator>();
        builder.Services.AddScoped<IValidator<TopicUpdateRequest>, TopicUpdateValidator>();

        builder.Services.AddFluentValidationAutoValidation();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddApiAuthentication(builder.Configuration);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ForumContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
