using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public UserController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
          if (_context.Tbl_User == null)
          {
              return NotFound();
          }
            return await _context.Tbl_User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Tbl_User == null)
          {
              return NotFound();
          }
            var user = await _context.Tbl_User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.intUserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Tbl_User == null)
          {
              return Problem("Entity set 'AppDbContext.User'  is null.");
          }
            user.dtCreated_on ??= DateTime.Now;
            user.dtUpdated_on ??= DateTime.Now;
            _context.Tbl_User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.intUserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Tbl_User == null)
            {
                return NotFound();
            }
            var user = await _context.Tbl_User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Tbl_User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Tbl_User?.Any(e => e.intUserId == id)).GetValueOrDefault();
        }

        //[HttpGet("{email}/{password}")]
        //public async Task<ActionResult<User>> Login(string email, string password)
        //{
        //    if (email is not null && password is not null)
        //    {
        //        var user = await _context.Tbl_User
        //            .Where(x => x.strUserName!.ToLower().Equals(email.ToLower()) && x.strPassword == password)
        //            .FirstOrDefaultAsync();
        //        return user != null ? Ok(user) : NotFound("User not found");
        //    }
        //    return BadRequest("Invalid Request");
        //}

        [HttpGet("{email}/{password}")]
        public async Task<ActionResult<User>> Login(string email, string password)
        {
            if (email is not null && password is not null)
            {
                var user = await _context.Tbl_User
                    .Where(x => x.strUserName!.ToLower().Equals(email.ToLower()) && x.strPassword == password)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        new Claim("UserName", user.strUserName)
    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(120),
                        signingCredentials: signIn);

                    var userModel = new Response
                    {
                        intUserId = user.intUserId,
                        strName = user.strName,
                        strUserName = user.strUserName,
                        strPassword = user.strPassword,
                        longContactNo = user.longContactNo,
                        strRole = user.strRole,
                        dtCreated_on = DateTime.UtcNow,
                        dtUpdated_on = DateTime.UtcNow,
                        strUserMessage = "Login Success",
                        strUserToken = new JwtSecurityTokenHandler().WriteToken(token)
                    };

                    return Ok(userModel);
                }
                return NotFound("User not found");
            }
            return BadRequest("Invalid Request");
        }


        [HttpGet]
        [Route("GetUserByEmailAndPhone")]
        public async Task<ActionResult<User>> GetUserByEmailAndPhone(string email, string phone)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone))
            {
                return BadRequest("Email and phone number must be provided.");
            }

            var user = await _context.Tbl_User.FirstOrDefaultAsync(u => u.strUserName == email || u.longContactNo.ToString() == phone);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        [HttpGet("{userId}/details")]
        public async Task<ActionResult<User>> GetUserDetailsByUserId(int userId)
        {
            if (_context.Tbl_User == null)
            {
                return NotFound();
            }

            var user = await _context.Tbl_User.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
    }
}

