using System.Threading.Tasks;
using Features.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using ResultNet;

namespace Application.Providers;

public class UpdateRequestCultureProvider : IRequestCultureProvider
{
    public async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var cultureService = httpContext.RequestServices.GetRequiredService<ICultureService>();
        var updateService = httpContext.RequestServices.GetRequiredService<IUpdateService>();

        var update = await updateService.GetUpdateFromHttpRequest(httpContext);

        if (update is null)
            return new ProviderCultureResult("en-US");

        var username = updateService.GetUsername(update);
        var userCultureCode = await cultureService.GetCodeByUsername(username);

        if (!userCultureCode.IsFailure() && userCultureCode.Data is not null)
            return new ProviderCultureResult(userCultureCode.Data);

        // case before botUser is registered
        var cultureName = updateService.GetUserCultureName(update.Message);
        var cultureCode = await cultureService.GetCodeByCultureName(cultureName);

        return new ProviderCultureResult(cultureCode);
    }
}