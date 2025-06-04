using EF_DBFIRST.Services;
using Microsoft.AspNetCore.Mvc;

namespace EF_DBFIRST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(CancellationToken token, int id)
    {
        await _clientService.DeleteClient(token, id);
        return Ok();
    }
}