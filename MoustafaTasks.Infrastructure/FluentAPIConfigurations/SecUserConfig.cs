using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoustafaTasks.Domain;

namespace MoustafaTasks.Infrastructure.FluentAPIConfigurations;

public class SecUserConfig : IEntityTypeConfiguration<SecUser>
{
    public void Configure(EntityTypeBuilder<SecUser> builder)
    {
        builder.ToTable("SecUsers");
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserId).IsRequired().HasColumnType("int");
        builder.Property(x => x.UserTypeId).IsRequired().HasConversion<string>();
        builder.Property(x => x.IsActived).IsRequired().HasConversion<bool>();
        builder.Property(x => x.IsRemoved).IsRequired().HasConversion<bool>();
        builder.HasData(new List<SecUser> {
            new SecUser (1, "username1", Domain.Enums.UserTypeId.Person, true, false),
            new SecUser (2, "username2", Domain.Enums.UserTypeId.ServiceAccount, false, false),
            new SecUser (3, "username3", Domain.Enums.UserTypeId.Person, false, true)
    });
    }
}
