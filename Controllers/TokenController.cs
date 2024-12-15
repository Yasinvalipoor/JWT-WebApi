using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_JWT.Data;
using WebAPI_JWT.Models;

namespace WebAPI_JWT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public TokenController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> PostUser(UserInfo userInfo)
    {
        if (userInfo is not null && userInfo.UserName is not null && userInfo.UserPassword is not null)
        {
            var user = await GetUser(userInfo.UserName, userInfo.UserPassword);
            if (user != null)
            {
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),  // تاریخ صدور به صورت timestamp
                    new Claim("Id", userInfo.UserId.ToString()),
                    new Claim("UserName", userInfo.UserName),
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds().ToDateTime(), // تاریخ انقضا به صورت 10 دقیقه از اکنون
                    signingCredentials: credentials);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return BadRequest("Invalid");
        }
        return BadRequest("Invalid");
    }

    [HttpGet]
    private async Task<UserInfo> GetUser(string userName, string password)
    {
        return await _context.UserInfos.FirstOrDefaultAsync(u => u.UserName == userName && u.UserPassword == password);
    }
}



//[Route("api/[controller]")]
//[ApiController]
//public class TokenController : ControllerBase
//{
//    private readonly IConfiguration _configuration;
//    private readonly ApplicationDbContext _context;

//    public TokenController(IConfiguration configuration, ApplicationDbContext context)
//    {
//        _configuration = configuration;
//        _context = context;
//    }
//    [HttpPost]
//    public async Task<IActionResult> PostUser(UserInfo userInfo)
//    {

//        if (userInfo is not null && userInfo.UserName is not null && userInfo.UserPassword is not null)
//        {
//            var user = await GetUser(userInfo.UserName, userInfo.UserPassword);
//            if (user != null)
//            {
//                var claims = new[] {
//                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
//                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
//                    new Claim("Id",userInfo.UserId.ToString()),
//                    new Claim("UserName",userInfo.UserName),
//                    new Claim("Password",userInfo.UserPassword),
//                };

//                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
//                var token = new JwtSecurityToken(
//                    _configuration["Jwt:Issuer"],
//                    _configuration["Jwt:Audience"],
//                    claims,
//                    expires: DateTime.Now.AddMinutes(120),
//                    signingCredentials: credentials);

//                Console.WriteLine(new JwtSecurityTokenHandler().WriteToken(token));
//                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
//            }
//            return BadRequest("Invalid");

//        }
//        return BadRequest("Invalid");
//    }
//    [HttpGet]
//    private async Task<UserInfo> GetUser(string userName, string password)
//    {
//        return await _context.UserInfos.FirstOrDefaultAsync(u => u.UserName == userName && u.UserPassword == password);
//    }
//}