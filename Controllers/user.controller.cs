using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using First_Backend.Data;
using First_Backend.Models;
using First_Backend.Dtos;

namespace First_Backend.Controllers
{
    [ApiController] // Specified as an API controller
    [Route("api/user")]
    public class UserController: ControllerBase
    {
        private readonly MyDbContext _context;

        public UserController(MyDbContext context)
        {
            _context = context;
        }

        // Signs up a new user
        [HttpPost("signup")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            // Check if a user with the same name already exists
            bool isNameTaken = await _context.Users.AnyAsync(u => u.Name == user.Name);
            if (isNameTaken)
            {
                // Returns error code 400 for Bad Request
                return BadRequest("Username already exists");
            }

            // Check for duplicate email entries
            bool isEmailTaken = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (isEmailTaken)
            {
                // Returns error code 400 for Bad Request
                return BadRequest("Email already exists");
            }

            // Hash the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Add a new user
            _context.Users.Add(user);
            
            // Save changes
            await _context.SaveChangesAsync();

            // Create a UserDto to be returned in API Response
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            // Return HTTP response
            return CreatedAtAction(nameof(CreateUser), new { id = userDto.Id }, userDto);
        }

        // Get all user in the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                })
                .ToListAsync();
            
            return Ok(users);
        }

        // Get user by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id== id)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                })
                .FirstOrDefaultAsync();
            
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // Delete a user from the database
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            // Find the user to be deleted
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Delete from DB and save changes
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User don comot from the application" });
        }
    }
}