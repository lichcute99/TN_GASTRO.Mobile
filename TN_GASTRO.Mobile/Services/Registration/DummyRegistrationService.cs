namespace TN_GASTRO.Mobile.Services.Registration;

public class DummyRegistrationService : IRegistrationService
{
    public Task<bool> ValidateAsync(string ip, string password)
        => Task.FromResult(ip == "10.0.0.1" && password == "reg@123"); // TODO: thay bằng API thật
}
