using Domain.Interfaces.Services;

namespace WebClientMVC.Services;

public interface IAppServices
{
    public ISesionService SesionService { get; }
   
}
