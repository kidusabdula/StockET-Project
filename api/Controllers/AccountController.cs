using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;


        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username!");

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, true);

            if (result.Succeeded)
            {
                return Ok(
                 new NewUserDTO
                 {
                     UserName = user.UserName,
                     Email = user.Email,
                     Token = _tokenService.CreateToken(user)
                 }
             );
            }

            else if (result.IsLockedOut)
            {

                return Unauthorized("Account is locked out after too many attempts. Please try again later.");
            }
            else
            {

                return Unauthorized("Invalid Username or Password. Please Try Again");
            }

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                if (registerDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                           new NewUserDTO
                           {
                               UserName = appUser.UserName,
                               Email = appUser.Email,
                               Token = _tokenService.CreateToken(appUser)
                           }
                       );
                    }
                    else
                    {
                        // Log role assignment errors here
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    // Log user creation errors here
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                // Log exception details here
                return StatusCode(500, new { Error = e.Message });
            }
        }

    }
}