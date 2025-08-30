using Windows.Storage;

namespace TN_GASTRO.Mobile.Services.Device;

public class LocalSettingsDeviceRegistry : IDeviceRegistry
{
    private readonly ApplicationDataContainer _ls = ApplicationData.Current.LocalSettings;
    private const string KeyRegistered = "device_registered";
    private const string KeyName = "device_name";
    private const string KeyIp = "device_ip";
    private const string KeyLang = "language";

    public bool IsRegistered() => (_ls.Values[KeyRegistered] as bool?) == true;

    public string? GetDeviceName() => _ls.Values[KeyName] as string;
    public string? GetDeviceIp() => _ls.Values[KeyIp] as string;
    public string? GetLanguage() => _ls.Values[KeyLang] as string;

    public void SaveRegistration(string deviceName, string ip)
    {
        _ls.Values[KeyRegistered] = true;
        _ls.Values[KeyName] = deviceName;
        _ls.Values[KeyIp] = ip;
    }

    public void SaveLanguage(string lang) => _ls.Values[KeyLang] = lang;
}
