using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using MonadicBits;
using WatsonSync.Components;
using WatsonSync.Models;

namespace WatsonSync.Controllers;

using static Functional;

[Authorize]
[Route("users")]
public sealed class UsersController : Controller
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository) =>
        this.userRepository = userRepository;

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Create([FromBody] NewUserRequest newUserRequest) =>
        (from emailAddress in ValidateEmailAddress(newUserRequest.Email)
            from user in userRepository.Create(emailAddress)
            select new NewUserResponse(user.Token))
        .Match<IActionResult>(
            response => Created(string.Empty, response),
            () => StatusCode(500));

    private static Maybe<string> ValidateEmailAddress(string emailAddress) =>
        MailAddress.TryCreate(emailAddress, out var result) ? result.Address : Nothing;
}