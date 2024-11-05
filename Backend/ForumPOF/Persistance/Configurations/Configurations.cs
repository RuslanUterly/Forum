using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistance.Enums;

namespace Persistance.Configurations;

public abstract class UlidConverter
{
    public ValueConverter<Ulid, string> ulidConverter = new ValueConverter<Ulid, string>(
            v => v.ToString(),       // Как сохранять Ulid в строку
            v => Ulid.Parse(v));     // Как загружать из строки
}

public class UserConfiguration : UlidConverter, IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(c => c.Id)
               .HasConversion(ulidConverter);

        builder.HasIndex(u => u.UserName)
               .IsUnique();

        builder.HasIndex(u => u.Email)
               .IsUnique();

        builder.HasOne(u => u.Role)
               .WithMany()
               .HasForeignKey(u => u.RoleId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(c => c.Id);

        var roles = Enum
            .GetValues<UserRole>()
            .Select(r => new Role
            {
                Id = (int)r,
                Name = r.ToString()
            });

        builder.HasData(roles);
    }
}

public class CategoryConfiguration : UlidConverter, IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasIndex(c => c.Name)
               .IsUnique();

        builder.Property(c => c.Id)
               .HasConversion(ulidConverter);

        builder.Property(c => c.Name)
               .UseCollation("SQL_Latin1_General_CP1_CI_AS");
    }
}

public class CommentConfiguration : UlidConverter, IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(c => c.Id)
               .HasConversion(ulidConverter);

        builder.Property(c => c.PostId)
               .HasConversion(ulidConverter);

        builder.Property(c => c.UserId)
               .HasConversion(ulidConverter);
    }
}

public class PostConfiguration : UlidConverter, IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(c => c.Id)
               .HasConversion(ulidConverter);

        builder.Property(c => c.TopicId)
               .HasConversion(ulidConverter);

        builder.Property(c => c.UserId)
               .HasConversion(ulidConverter);
    }
}
public class TagConfiguration : UlidConverter, IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(c => c.Id)
               .HasConversion(ulidConverter);

        builder.HasIndex(u => u.Title)
               .IsUnique();

        builder.Property(c => c.Title)
               .UseCollation("SQL_Latin1_General_CP1_CI_AS");

    }
}

public class TopicConfiguration : UlidConverter, IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.Property(c => c.Id)
               .HasConversion(ulidConverter);

        builder.Property(c => c.UserId)
               .HasConversion(ulidConverter);

        builder.Property(c => c.CategoryId)
               .HasConversion(ulidConverter);
    }
}

public class TopicTagsConfiguration : UlidConverter, IEntityTypeConfiguration<TopicTag>
{
    public void Configure(EntityTypeBuilder<TopicTag> builder)
    {
        builder.Property(c => c.TopicTagId)
               .HasConversion(ulidConverter);

        builder.Property(c => c.TopicId)
               .HasConversion(ulidConverter);

        builder.Property(c => c.TagId)
               .HasConversion(ulidConverter);

        builder.HasOne(tt => tt.Topic)
               .WithMany(t => t.ThreadTags)
               .HasForeignKey(tt => tt.TopicId);

        builder.HasOne(tt => tt.Tag)
               .WithMany(t => t.TopicTags)
               .HasForeignKey(tt => tt.TagId);
    }
}
