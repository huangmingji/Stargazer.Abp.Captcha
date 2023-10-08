using Hei.Captcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stargazer.Abp.Captcha.HttpApi;
using Volo.Abp.AspNetCore.Mvc;

namespace Stargazer.Abp.CMS.Web.Controllers
{
    [Route("security")]
    public class SecurityController : AbpController
    {
        private readonly SecurityCodeHelper _securityCodeHelper;
        private readonly ICaptchaHelper _captchaHelper;
        private readonly ILogger<SecurityController> _logger;

        public SecurityController(ILogger<SecurityController> logger,
            ICaptchaHelper captchaHelper,
            SecurityCodeHelper securityCodeHelper)
        {
            _logger = logger;
            _securityCodeHelper = securityCodeHelper;
            _captchaHelper = captchaHelper;
        }

        [HttpGet("captcha")]
        public IActionResult Captcha()
        {
            try
            {
                var code = _securityCodeHelper.GetRandomCnText(4);
                var imgbyte = _securityCodeHelper.GetGifBubbleCodeByte(code);
                _captchaHelper.SetValue(HttpContext, code: code);
                return File(imgbyte, "image/gif");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}

