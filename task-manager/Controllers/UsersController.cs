using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_manager.Services;
using System.Security.Claims;
using task_manager.Infraestructure;
using task_manager.Models;

namespace task_manager.Controllers;

public class UsersController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _dbContext;

    public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? message = null)
    {
        if (message is not null)
        {
            ViewData["Message"] = message;
        }
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        IdentityUser? user = await _userManager.FindByEmailAsync(viewModel.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(viewModel);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, viewModel.PasswordHash, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: viewModel.RememberMe);
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(viewModel);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction(actionName: nameof(Login), controllerName: "Users");
    }

    [HttpGet]
    [AllowAnonymous]
    public ChallengeResult ExternalLogin(string provider, string? returnUrl = null)
    {
        string redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Users", new { returnUrl }) ?? string.Empty;
        AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Content("~/");
        string message = string.Empty;

        if (remoteError != null)
        {
            message = $"Error from external provider: {remoteError}";
            ModelState.AddModelError(string.Empty, message);
            return RedirectToAction(nameof(Login), new { message });
        }

        ExternalLoginInfo? info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            message = "Error loading external login information.";
            return RedirectToAction(nameof(Login), new { message });
        }

        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl ?? "/");
        }

        string email = string.Empty;
        string user = string.Empty;
        string? emailClaim = info.Principal.HasClaim(c => c.Type == ClaimTypes.Email) ? info.Principal.FindFirstValue(ClaimTypes.Email) : string.Empty;
        string? nameClaim = info.Principal.HasClaim(c => c.Type == ClaimTypes.Name) ? info.Principal.FindFirstValue(ClaimTypes.Name) : string.Empty;
        if (!string.IsNullOrWhiteSpace(emailClaim) && !string.IsNullOrWhiteSpace(nameClaim))
        {
            email = emailClaim;
            user = nameClaim;
        }
        else
        {
            message = "Email and name claims are required for external login.";
            return RedirectToAction(nameof(Login), new { message });
        }

        IdentityUser usuario = new() { UserName = user, Email = email };
        IdentityResult identityResult = await _userManager.CreateAsync(usuario);
        if (!identityResult.Succeeded)
        {
            message = identityResult.Errors.FirstOrDefault()?.Description ?? "An error occurred while creating the user.";
            return RedirectToAction(nameof(Login), new { message });
        }

        IdentityResult loginResult = await _userManager.AddLoginAsync(usuario, info);
        if (!loginResult.Succeeded)
        {
            message = loginResult.Errors.FirstOrDefault()?.Description ?? "An error occurred while adding the external login.";
            return RedirectToAction(nameof(Login), new { message });
        }
        await _signInManager.SignInAsync(usuario, isPersistent: true);
        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        IdentityUser user = new() { UserName = viewModel.UserName, Email = viewModel.Email };
        IdentityResult result = await _userManager.CreateAsync(user, viewModel.PasswordHash);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: true);
            return RedirectToAction("Index", "Home");
        }
        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(viewModel);
    }

    [HttpGet]
    [Authorize(Roles = Constants.RoleAdmin)]
    public async Task<IActionResult> List(string? message = null)
    {
        List<UsuarioViewModel> usuarios = await _dbContext.Users.Select(static u => new UsuarioViewModel
        {
            UserName = u.UserName ?? string.Empty,
            Email = u.Email ?? string.Empty
        }).ToListAsync();

        var viewModel = new UsersListViewModel
        {
            Users = usuarios,
            Message = message ?? string.Empty
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = Constants.RoleAdmin)]
    public async Task<IActionResult> MakeAdmin(string email)
    {
        IdentityUser? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return NotFound();
        }

        IdentityResult result = await _userManager.AddToRoleAsync(user, Constants.RoleAdmin);
        string successMessage = result.Succeeded ? "User promoted to admin successfully." : "An error occurred while promoting the user to admin.";
        return RedirectToAction(nameof(List), new { message = successMessage });
    }

    [HttpPost]
    [Authorize(Roles = Constants.RoleAdmin)]
    public async Task<IActionResult> RemoveAdmin(string email)
    {
        IdentityUser? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return NotFound();
        }

        IdentityResult result = await _userManager.RemoveFromRoleAsync(user, Constants.RoleAdmin);
        string successMessage = result.Succeeded ? "User removed from admin role successfully." : "An error occurred while removing the user from the admin role.";
        return RedirectToAction(nameof(List), new { message = successMessage });
    }

    [HttpGet]
    public async Task<IActionResult> AccessDenied()
    {
        return View();
    }
}
