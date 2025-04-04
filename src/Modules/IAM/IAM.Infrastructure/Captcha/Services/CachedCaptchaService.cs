using Common.Application.Caching;
using Common.Application.Options;
using Common.Domain.ResultMonad;
using IAM.Application.Captcha.Services;
using Microsoft.Extensions.Options;

namespace IAM.Infrastructure.Captcha.Services;
public class CachedCaptchaService(
    ICaptchaService decoree,
    ICacheService cacheService,
    IOptions<OtpOptions> otpOptionsProvider
    ) : ICaptchaService
{
    private readonly int _cacheCaptchaForMinutes = otpOptionsProvider.Value.ExpirationInMinutes;
    public Task<Result> ValidateAsync(string captchaToken, CancellationToken cancellationToken)
        => cacheService.GetOrCreateAsync(
            key: CacheKey(captchaToken),
            factory: async ct => await decoree.ValidateAsync(captchaToken, ct),
            absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(_cacheCaptchaForMinutes),
            cancellationToken: cancellationToken);

    public string GetClientKey() => decoree.GetClientKey();
    private static string CacheKey(string captchaToken) => $"captcha:{captchaToken}";
}
