﻿using Microsoft.AspNetCore.Mvc;

namespace Oikono.Api.Common.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        return Problem();
    }
}