using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_JWT.Data;
using WebAPI_JWT.Models;

namespace WebAPI_JWT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserInfoesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserInfoesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/UserInfoes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserInfos()
    {
        return await _context.UserInfos.ToListAsync();
    }

    // GET: api/UserInfoes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserInfo>> GetUserInfo(int id)
    {
        var userInfo = await _context.UserInfos.FindAsync(id);

        if (userInfo == null)
        {
            return NotFound();
        }

        return userInfo;
    }

    // PUT: api/UserInfoes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserInfo(int id, UserInfo userInfo)
    {
        if (id != userInfo.UserId)
        {
            return BadRequest();
        }

        _context.Entry(userInfo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserInfoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/UserInfoes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<UserInfo>> PostUserInfo(UserInfo userInfo)
    {
        _context.UserInfos.Add(userInfo);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUserInfo", new { id = userInfo.UserId }, userInfo);
    }

    // DELETE: api/UserInfoes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserInfo(int id)
    {
        var userInfo = await _context.UserInfos.FindAsync(id);
        if (userInfo == null)
        {
            return NotFound();
        }

        _context.UserInfos.Remove(userInfo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserInfoExists(int id)
    {
        return _context.UserInfos.Any(e => e.UserId == id);
    }
}