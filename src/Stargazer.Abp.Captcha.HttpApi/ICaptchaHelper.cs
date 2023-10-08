using Microsoft.AspNetCore.Http;

namespace Stargazer.Abp.Captcha.HttpApi;

public interface ICaptchaHelper
{

    void SetValue(HttpContext httpContext, string code);

    bool IsVerified(HttpContext httpContext, string code);

}