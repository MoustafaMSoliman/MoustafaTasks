using MoustafaTasks.Domain.Enums;

namespace MoustafaTasks.Domain;

public class Device
{
    public int Id { get; set; }
    public string NameEnglish { get; set; }
    public string? NameArabic { get; set; }
    public string? Description { get; set; }
    public DeviceType DeviceType { get; set; }
    public bool IsActived { get; set; }
    public bool IsRemoved { get; set; }
}
