using MoustafaTasks.Domain.Enums;

namespace MoustafaTasks.Domain;

public class Device
{
    public Device(int id, string nameEnglish, DeviceType deviceType, bool isActived, bool isRemoved)
    {
        Id = id;
        NameEnglish = nameEnglish;
        DeviceType = deviceType;
        IsActived = isActived;
        IsRemoved = isRemoved;
    }

    public int Id { get; set; }
    public string NameEnglish { get; set; }
    public DeviceType DeviceType { get; set; }
    public bool IsActived { get; set; }
    public bool IsRemoved { get; set; }
}
