using Identity.DbContext;
using Identity.DTOs;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            TokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (result.Succeeded)
            {
                UserDTO userDTO = CreateUserDTO(user);
                userDTO.Allergies = user.Allergies.Select(a => a.Name).ToList();
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDTO.Email))
                return BadRequest("The email is not available");

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDTO.Username))
                return BadRequest("The username is not available");

            List<Allergy> newAllergies = registerDTO.Allergies
                .Where(a => !_context.Allergies
                    .Select(x => x.Name.ToUpper())
                    .Contains(a.Name.ToUpper()))
                .ToList();

            _context.Allergies.AddRange(newAllergies);
            await _context.SaveChangesAsync();

            var user = new ApplicationUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.Username,
                IsVegan = registerDTO.isVegan,
                Allergies = registerDTO.Allergies.ToList()
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            return result.Succeeded 
                ? CreateUserDTO(user) 
                : (ActionResult<UserDTO>)BadRequest("Problem registering user");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            return CreateUserDTO(user);
        }

        private UserDTO CreateUserDTO(ApplicationUser user) => new UserDTO
        {
            DisplayName = user.DisplayName,
            Token = _tokenService.CreateToken(user),
            Username = user.UserName,
            IsVegan = user.IsVegan,
            Allergies = user.Allergies.Select(a => a.Name).ToList()
        };
    }
}
