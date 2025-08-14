using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoustafaTasks.Domain;

namespace MoustafaTasks.Infrastructure.FluentAPIConfigurations;

public class DeviceConfig : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable($"{nameof(Device)}s");
        builder.HasKey( x => x.Id );
        builder.Property(x => x.NameEnglish).IsRequired().HasColumnType("nvarchar(50)");
        builder.Property(x => x.DeviceType).IsRequired().HasConversion<string>();
        builder.Property(x => x.IsActived).IsRequired().HasConversion<bool>();
        builder.Property(x => x.IsRemoved).IsRequired().HasConversion<bool>();
        builder.HasData(new List<Device> { 
          new Device(1, "Device1", Domain.Enums.DeviceType.Camera, true, false),
          new Device(2, "Device2", Domain.Enums.DeviceType.Another, false,false),
          new Device(3, "Device3", Domain.Enums.DeviceType.Camera, false,true)

        });
    }
}
