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
    public bool IsVerified(HttpContext httpContext, string code)
    {
        string captchaEncrypt = httpContext.Request.Cookies[captchaKey] ?? "";
        if (code.IsNullOrWhiteSpace()
            || captchaEncrypt.IsNullOrWhiteSpace()
            || _stringEncryptionService.Decrypt(captchaEncrypt) != code)
        {
            return false;
        }

        if (!captchaEncrypt.IsNullOrWhiteSpace())
        {
            httpContext.Response.Cookies.Delete(captchaKey);
        }
        return true;
    }

    public void SetValue(HttpContext httpContext, string code)
    {
        string captcha = httpContext.Request.Cookies[captchaKey] ?? "";
        if (!captcha.IsNullOrWhiteSpace())
        {
            httpContext.Response.Cookies.Delete(captchaKey);
        }
        httpContext.Response.Cookies.Append(captchaKey, _stringEncryptionService.Encrypt(code), new CookieOptions()
        {
            Expires = DateTimeOffset.Now.AddMinutes(5),
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Domain = httpContext.Request.Path
        });
    }
}