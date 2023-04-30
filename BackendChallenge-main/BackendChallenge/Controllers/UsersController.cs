using BackendChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendChallenge.Models;
using BackendChallenge.Utils;



namespace BackendChallenge.Controllers;

[ApiController]

//  /users endpoint
[Route("[controller]")]      
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly AppDbContext _db;

    public UsersController(ILogger<UsersController> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<ActionResult<IEnumerable<User>>> Index(CancellationToken token)
    {
        try
        {   
            var userToken = Request.Headers["UserToken"].FirstOrDefault();
            if (string.IsNullOrEmpty(userToken))
            {
                return BadRequest("UserToken header is missing.");
            }

            var userId = await TokenUtils.GetUserIdFromToken(userToken, token, _db);
            if (userId == null){
                return StatusCode(401, "Invalid or expired authentication token.");
            }

            var userCompanyId = await UserUtils.GetCompanyIdFromUserId((int)userId, token, _db);
            if (userCompanyId == null ){
                    throw new InvalidOperationException("Unable to get CompanyId from the userID!");
            }

            var users = await _db.Users
            .Where(u => u.CompanyId == _db.Users
                .Where(u2 => u2.UserId == userId)
                .Select(u2 => u2.CompanyId)
                .FirstOrDefault())
            .Select(u => new { u.UserId, u.FirstName, u.LastName })
            .ToListAsync(token);
            
            return Ok(users);
        }
        catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, "Internal server error.");
            }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected Error getting users");
            return StatusCode(500, "Internal server error.");
        }
    }

}