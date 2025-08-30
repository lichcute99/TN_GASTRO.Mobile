namespace TN_GASTRO.Mobile.Services.Registration;

public interface IRegistrationService
{
    // Trả true nếu cặp (ip, password) hợp lệ theo DB 
    Task<bool> ValidateAsync(string ip, string password);
}
