using AndroidServerSide.Interfaces;
using AndroidServerSide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMemoryCache _memoryCache;
        public UserController(IRepository<User> userRepository, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            string cacheKey = "AllUsers";
            if (!_memoryCache.TryGetValue(cacheKey, out var usersFromCache))
            {
                var users = await _userRepository.GetAllAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _memoryCache.Set(cacheKey, users, cacheEntryOptions);
                return Ok(users);
            }
            return Ok(usersFromCache);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<User>> GetUser(string email)
        {
            string cacheKey = "User" + email;
            if (!_memoryCache.TryGetValue(cacheKey, out var userFromCache))
            {
                var user = await _userRepository.GetByIdAsync(email);
                if (user == null)
                    return NotFound();
                
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _memoryCache.Set(cacheKey, user, cacheEntryOptions);
                return Ok(user);
            }
            return Ok(userFromCache);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            await _userRepository.AddAsync(user);
            _memoryCache.Remove("AllUsers");
            return CreatedAtAction(nameof(GetUser), new { email = user.Email }, user);
        }

        [HttpPut("{email}")]
        public async Task<ActionResult> UpdateUser(string email, [FromBody] User user)
        {
            if (email != user.Email)
                return BadRequest();

            var existingUser = await _userRepository.GetByIdAsync(email);
            if (existingUser == null)
                return NotFound();

            await _userRepository.UpdateAsync(existingUser, user);
            _memoryCache.Remove("AllUsers");
            _memoryCache.Remove("User" + email);
            return NoContent();
        }

        [HttpDelete("{email}")]
        public async Task<ActionResult> DeleteUser(string email)
        {
            var user = await _userRepository.GetByIdAsync(email);
            if (user == null)
                return NotFound();

            await _userRepository.DeleteAsync(email);
            _memoryCache.Remove("AllUsers");
            _memoryCache.Remove("User" + email);
            return NoContent();
        }
    }
}
