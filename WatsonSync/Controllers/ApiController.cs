using Microsoft.AspNetCore.Mvc;
using WatsonSync.Models;

namespace WatsonSync.Controllers;

public abstract class ApiController : Controller
{
    protected User CurrentUser => (User)HttpContext.Items["User"];
}