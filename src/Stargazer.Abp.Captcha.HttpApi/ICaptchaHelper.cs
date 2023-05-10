using Microsoft.AspNetCore.Http;

namespace Stargazer.Abp.Captcha.HttpApi;

public interface ICaptchaHelper
{

    void SetValue(HttpRequest request, HttpResponse response, string code);

    bool IsVerified(HttpRequest request, HttpResponse response, string code);

}