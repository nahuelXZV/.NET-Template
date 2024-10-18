using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace WebApi.Controllers;

public abstract class BaseController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();
}
