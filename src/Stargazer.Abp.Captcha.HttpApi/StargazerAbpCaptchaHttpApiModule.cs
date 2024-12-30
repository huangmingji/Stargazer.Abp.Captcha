using System.Text;
using Hei.Captcha;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Security.Encryption;
using Stargazer.Common.Extend;

namespace Stargazer.Abp.Captcha.HttpApi;

[DependsOn(
    typeof(AbpSecurityModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class StargazerAbpCaptchaHttpApiModule : AbpModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHeiCaptcha();
        IConfiguration? configuration = context.Services.BuildServiceProvider().GetService<IConfiguration>();
        string strongPassPhrase = configuration?.GetSection("App:AbpSecurity:PassPhrase").Value ?? "LxMH1v8lyuVsOF3B";
        string salt = configuration?.GetSection("App:AbpSecurity:Salt").Value ?? "p%TCDNuW";
        string initVectorBytes = configuration?.GetSection("App:AbpSecurity:InitVectorBytes").Value ?? "5uhQorj0IO7eMTBR";
        int keySize = configuration?.GetSection("App:AbpSecurity:KeySize").Value.ToInt() ?? 256;
        Configure<AbpStringEncryptionOptions>(opts =>
        {
            opts.DefaultPassPhrase = strongPassPhrase;
            opts.DefaultSalt = Encoding.UTF8.GetBytes(salt);
            opts.InitVectorBytes = Encoding.UTF8.GetBytes(initVectorBytes);
            opts.Keysize = keySize;
        });

        context.Services.AddTransient<ICaptchaHelper, CaptchaHelper>();
    }

}
