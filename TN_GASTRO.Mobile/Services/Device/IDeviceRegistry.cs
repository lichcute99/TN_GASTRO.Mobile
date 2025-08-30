namespace TN_GASTRO.Mobile.Services.Device;

public interface IDeviceRegistry
{
    bool IsRegistered();
    string? GetDeviceName();
    string? GetDeviceIp();
    string? GetLanguage();

    void SaveRegistration(string deviceName, string ip);
    void SaveLanguage(string lang); // "vi" | "en" | "de"
}
