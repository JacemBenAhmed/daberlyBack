using DaberlyProjet.Data;
using DaberlyProjet.DTO;
using DaberlyProjet.Models;
using DaberlyProjet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DaberlyProjet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _jwtSecret;
        private readonly PhotoUserService _userService;

        public UserController(AppDbContext context, IConfiguration configuration, PhotoUserService userService)
        {
            _context = context;
            _jwtSecret = configuration["Jwt:SecretKey"];
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserRegisterDTO dto)
        {
            if (dto.Password.Length < 6)
            {
                return BadRequest("Password must be at least 6 characters long.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Addresse = dto.Addresse,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = passwordHash,
                Role = "vendeur",
                blocked = true , // true until admin accept (if champs is OK ) 
                numCin = dto.numCin,
                DateCreated = DateTime.UtcNow,
                Region = "vide"
            };


            var existMail = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existMail == null)
            {

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok("User registered successfully");


            }
            else
            {
                return BadRequest("Mail exist deja");
            }


        }




        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)  )
            {
                return NotFound("Invalid email or password");
            }

            if(user.blocked == true)
            {
                return Unauthorized("user blocked ! ");
            }

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                Role = user.Role
            });
        }






        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("getUsersRole")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            var users = await _context.Users.Where(u=>u.Role == role).ToListAsync();

            if(users.Count == 0)
            {
                return NotFound("Users not found");
            }
            return Ok(users);
        }



        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User is not authenticated");
            }

            if (!int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                return BadRequest("Invalid User ID in token");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == currentUserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role
            });
        }

        [HttpGet("userByid/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }


        [HttpGet("userByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _context.Users.Where(c=>c.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
        [HttpGet("usersByAddress/{address}")]
        public async Task<IActionResult> GetUsersByAddress(string address)
        {
            var users = await _context.Users.Where(c => c.Addresse == address).ToListAsync();
            if (users == null)
            {
                return NotFound("User not found");
            }

            return Ok(users);
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (currentUserId != id && !User.IsInRole("admin"))
            {
                return Forbid("You are not authorized to delete this user.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User deleted successfully");
        }

        [HttpPut("setBlocked")]
        public async Task<IActionResult> setBlockedAccount(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("not Found");
            }
            else
            {
                user.blocked = !user.blocked;
                _context.SaveChanges();
                return Ok();
            }
        }

        [HttpPut("setRegion/{id}")]
        public async Task<IActionResult> setRegion(int id, [FromQuery] string region)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("not Found");
            }
            else
            {
                user.Region = region;
                _context.SaveChanges();
                return Ok();
            }
        }

        [HttpPut("setRole")]
        public async Task<IActionResult> setRoleUser(int id , string role)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("not Found");
            }
            else
            {
                if(role=="admin" | role=="agent" | role=="vendeur" | role=="client fidele")
                {
                    user.Role = role;
                    _context.SaveChanges();
                }

                
                return Ok();
            }

        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
