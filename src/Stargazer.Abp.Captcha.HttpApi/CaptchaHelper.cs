using Microsoft.AspNetCore.Http;
using Volo.Abp.Security.Encryption;

namespace Stargazer.Abp.Captcha.HttpApi;
public class CaptchaHelper : ICaptchaHelper
{
    private readonly IStringEncryptionService _stringEncryptionService;
    public CaptchaHelper(IStringEncryptionService stringEncryptionService)
    {
        _stringEncryptionService = stringEncryptionService;
    }

    private const string captchaKey = "captcha";
    public bool IsVerified(HttpRequest request, HttpResponse response, string code)
    {
        string captchaEncrypt = request.Cookies[captchaKey] ?? "";
        if (code.IsNullOrWhiteSpace()
            || captchaEncrypt.IsNullOrWhiteSpace()
            || _stringEncryptionService.Decrypt(captchaEncrypt) != code)
        {
            return false;
        }

        if (!captchaEncrypt.IsNullOrWhiteSpace())
        {
            response.Cookies.Delete(captchaKey);
        }
        return true;
    }

    public void SetValue(HttpRequest request, HttpResponse response, string code)
    {
        string captcha = request.Cookies[captchaKey] ?? "";
        if (!captcha.IsNullOrWhiteSpace())
        {
            response.Cookies.Delete(captchaKey);
        }
        response.Cookies.Append(captchaKey, _stringEncryptionService.Encrypt(code), new CookieOptions()
        {
            Expires = DateTimeOffset.Now.AddMinutes(5),
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Domain = request.Path
        });
    }
}