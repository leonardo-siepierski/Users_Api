using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using usersapi.Models;

namespace usersapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        private readonly ILogger<UsersController> _logger;

        public UsersController(UserContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            _logger.LogInformation($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Get all users");
            return _context.Users.ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogInformation($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} User with id {id} not found");

                return NotFound();
            }

            _logger.LogInformation($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Get user with {id} id");

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (user.Id != null && id != user.Id)
            {
                _logger.LogInformation($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} Id {id} is invalid");

                return BadRequest();
            }

            user.Id = id;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    _logger.LogInformation($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} User with id {id} not found");

                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} User with id {id} updated");

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            user.CreationDate = DateTime.ParseExact(DateTime.Now.ToString("dd'-'MM'-'yyyy'T'HH':'mm':'ss"), "dd'-'MM'-'yyyy'T'HH':'mm':'ss", null);
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    _logger.LogInformation($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} User with id {user.Id} already exists");

                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} New user created");

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogInformation($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} User with id {id} not found");

                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} User with id {id} was deleted");

            return user;
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
