using Microsoft.AspNetCore.Mvc;

namespace NotificationService.WebApi.Controllers;

/// <summary>
/// Простой контроллер для проверки доступности сервиса уведомлений.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "Notification Service OK" });
}