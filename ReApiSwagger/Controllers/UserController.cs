using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReApiSwagger.Dtos.User;
using ReApiSwagger.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReApiSwagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<AppUser> signInManager,
        IConfiguration configuration,
        IMapper mapper,
        IValidator<UserRegistrDto> registerValidator,
        IValidator<UserLoginDto> loginValidator
        ) : ControllerBase
    {
        //[HttpGet]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        //    await roleManager.CreateAsync(new IdentityRole { Name = "User" });
        //    return Ok("Roles created successfully.");
        //}
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRegistrDto userRegDto)
        {
            var validationResult = await registerValidator.ValidateAsync(userRegDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
            }
            var user = await userManager.FindByNameAsync(userRegDto.NickName);
            if (user != null) return BadRequest("User with this nickname already exists.");
            user = mapper.Map<AppUser>(userRegDto);
            var result = await userManager.CreateAsync(user, userRegDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors.Select(e => e.Description).ToArray());
            await userManager.AddToRoleAsync(user, "User");
            return Ok("User created successfully.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var validationResult = await loginValidator.ValidateAsync(userLoginDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
            }
            var user = await userManager.FindByNameAsync(userLoginDto.NickName);
            if (user == null) return Unauthorized("Invalid nickname or password.");
            var result = await signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid nickname or password.");
            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("NickName", user.NickName ?? string.Empty)
                };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
               var jwtsecurityToken = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
                );  
            var token = new JwtSecurityTokenHandler().WriteToken(jwtsecurityToken);
            return Ok(new { token });
        }
    }
}
