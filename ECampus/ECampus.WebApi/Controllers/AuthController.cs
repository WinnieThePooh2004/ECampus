using ECampus.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = ECampus.Services.Contracts.Services.IAuthorizationService;

namespace ECampus.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;

    public AuthController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginDto login, CancellationToken token = default)
    {
        return Ok(await _authorizationService.Login(login, token));
    }

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp(RegistrationDto registrationDto, CancellationToken token)
    {
        return Ok(await _authorizationService.SignUp(registrationDto, token));
    }

    [HttpPut]
    [Route("login/validate")]
    public async Task<IActionResult> ValidateLogin(LoginDto loginDto, CancellationToken token)
    {
        return Ok(await _authorizationService.ValidateLogin(loginDto, token));
    }
    
    [HttpPut]
    [Route("signup/validate")]
    public async Task<IActionResult> ValidateSignUp(RegistrationDto registrationDto, CancellationToken token)
    {
        return Ok(await _authorizationService.ValidateSignUp(registrationDto, token));
    }
}