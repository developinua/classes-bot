using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Features.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResultNet;

namespace Application.Services;

public class CultureService(IClassesDbContext context, ILogger<ICultureService> logger) : ICultureService
{
    public async Task<Culture> GetByName(string? name)
    {
        Culture defaultCulture = new();

        if (string.IsNullOrWhiteSpace(name))
            return defaultCulture;

        //todo: map bot culture name to db culture name
        var culture = await context.Cultures
            .AsNoTracking()
            .FirstOrDefaultAsync(doc => doc.Name == name);

        return culture ?? defaultCulture;
    }

    public async Task<Culture> GetByCode(string? code)
    {
        Culture defaultCulture = new();

        if (string.IsNullOrWhiteSpace(code))
            return defaultCulture;

        var culture = await context.Cultures
            .AsNoTracking()
            .FirstOrDefaultAsync(doc => doc.Code == code);

        return culture ?? defaultCulture;
    }

    public async Task<Result<string>> GetCodeByUsername(string? username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            logger.LogWarning("Username is null or empty when try to get language code.");
            return Result.Failure<string>();
        }

        var cultureCode = await context.Users
            .AsNoTracking()
            .Include(x => x.UserProfile.Culture)
            .Where(x => x.NickName == username)
            .Select(x => x.UserProfile.Culture.Code)
            .FirstOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(cultureCode))
        {
            logger.LogWarning("Can't get culture code for username {Username}.", username);
            return Result.Failure<string>();
        }

        return cultureCode;
    }

    public async Task<string> GetCodeByCultureName(string? cultureName)
    {
        var defaultCultureCode = new Culture().Code;

        if (string.IsNullOrWhiteSpace(cultureName))
        {
            logger.LogWarning("BotUser culture settings is not filled in.");
            return defaultCultureCode;
        }

        var cultureCode = await context.Cultures
            .AsNoTracking()
            .Where(x => x.Name == cultureName)
            .Select(x => x.Code)
            .FirstOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(cultureCode))
        {
            logger.LogWarning("Can't get culture code for culture name {CultureName}.", cultureName);
            return defaultCultureCode;
        }

        return cultureCode;
    }
}