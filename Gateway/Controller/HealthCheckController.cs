using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controller;

public class HealthCheckController : ControllerBase
{
    [HttpGet("/")]
    [HttpHead("/")]
    public Task<IResult> Root() => Task.FromResult(Results.Ok());

    [HttpGet("/health-check")]
    public Task<IResult> Get() => Task.FromResult(Results.Ok());

    // todo: add db health check
}