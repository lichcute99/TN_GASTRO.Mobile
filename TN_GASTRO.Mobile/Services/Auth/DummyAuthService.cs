namespace TN_GASTRO.Mobile.Services.Auth;

public class DummyAuthService : IAuthService
{
    public Task<bool> ValidateAsync(string deviceName, string numericPassword)
        => Task.FromResult(!string.IsNullOrWhiteSpace(deviceName) && numericPassword == "123456");
}
