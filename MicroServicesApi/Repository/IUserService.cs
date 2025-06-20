using MicroServicesApi.Dtos;
using MicroServicesApi.Helpers;
using MicroServicesApi.ViewModels;

namespace MicroServicesApi.Repository;

public interface IUserService
{
    Task<MobileResponse<LoginResponseModel>> LoginUser(LoginViewModel model, CancellationToken cancellationToken);
    Task<MobileResponse<RegisterViewDto>> RegisterUser(RegisterViewModel model, CancellationToken cancellationToken);
}
