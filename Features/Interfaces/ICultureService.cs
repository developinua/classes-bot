using Core.Entities;
using ResultNet;

namespace Features.Interfaces;

public interface ICultureService
{
    Task<Culture> GetByName(string? name);
    Task<Culture> GetByCode(string? code);
    Task<Result<string>> GetCodeByUsername(string? username);
    Task<string> GetCodeByCultureName(string? cultureName);
}