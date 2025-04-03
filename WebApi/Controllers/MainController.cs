using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class MainController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();
}
