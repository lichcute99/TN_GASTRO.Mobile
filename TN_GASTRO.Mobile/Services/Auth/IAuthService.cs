namespace TN_GASTRO.Mobile.Services.Auth;

public interface IAuthService
{
    // kiểm tra password theo device (sau này gọi API thật)
    Task<bool> ValidateAsync(string deviceName, string numericPassword);
}
