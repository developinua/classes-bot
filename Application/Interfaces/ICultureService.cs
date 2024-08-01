using System.Threading.Tasks;
using Core.Entities;
using ResultNet;

namespace Application.Interfaces;

public interface ICultureService
{
    Task<Culture> GetByName(string? name);
    Task<Culture> GetByCode(string? code);
    Task<Result<string>> GetCodeByUsername(string? username);
    Task<string> GetCodeByCultureName(string? cultureName);
}