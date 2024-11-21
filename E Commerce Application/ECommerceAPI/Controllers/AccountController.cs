using ECommerceAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly AppDbContext _context;

        public AccountController(IConfiguration configuration, AppDbContext context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
        }


        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.strNewPassword))
            {
                return BadRequest("Invalid request.");
            }

            var user = await _context.Tbl_User.SingleOrDefaultAsync(u => "+91" + u.longContactNo.ToString() == request.longContactNo);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.strPassword = request.strNewPassword;
            await _context.SaveChangesAsync();

            return Ok("Password reset successfully.");
        }
    }

    public class ResetPasswordRequest
    {
        public string longContactNo { get; set; }
        public string strNewPassword { get; set; }
    }
}