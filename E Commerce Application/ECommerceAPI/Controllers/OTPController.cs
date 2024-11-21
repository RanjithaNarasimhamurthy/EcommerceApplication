using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;



namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OTPController : ControllerBase
    {

        private static readonly Dictionary<string, string> otpStorage = new Dictionary<string, string>();

        [HttpPost("sendotp")]
        public IActionResult SendOtp([FromBody] OtpRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest("Phone number is required");
            }

            var otp = new Random().Next(100000, 999999).ToString();
            otpStorage[request.PhoneNumber] = otp;

            const string accountSid = "AC670861034a0948617c332e40153a76ac";
            const string authToken = "39576860c5e8520506f8b5212e5e21b7";
            const string fromPhoneNumber = "+16167732913";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: $"Your OTP code is {otp}",
                from: new PhoneNumber(fromPhoneNumber),
                to: new PhoneNumber(request.PhoneNumber)
            );

            return Ok(new { Message = "OTP sent successfully" });
        }
        //public IActionResult SendOtp([FromBody] OtpRequest request)
        //{
        //    if (string.IsNullOrEmpty(request.PhoneNumber))
        //    {
        //        return BadRequest("Phone Number  is required");
        //    }

        //    var otp = new Random().Next(100000, 999999).ToString();
        //    otpStorage[request.PhoneNumber] = otp; 

        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Ecommerce App", "mailtrap@demomailtrap.com"));
        //    message.To.Add(new MailboxAddress(request.PhoneNumber, request.PhoneNumber));
        //    message.Subject = "Your OTP Code";
        //    message.Body = new TextPart("plain")
        //    {
        //        Text = $"Your OTP code is {otp}"
        //    };

        //    using (var client = new SmtpClient())
        //    {
        //        client.Connect("live.smtp.mailtrap.io", 587, false);
        //        client.Authenticate("api", "b6afc5d9ce5050fbe1939a368414802a");
        //        client.Send(message);
        //        client.Disconnect(true);
        //    }

        //    return Ok(new { Message = "OTP sent successfully" });
        //}

        [HttpPost("verifyotp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.Otp))
            {
                return BadRequest("Phone Number and OTP are required");
            }

            if (otpStorage.ContainsKey(request.PhoneNumber) && otpStorage[request.PhoneNumber] == request.Otp)
            {
                otpStorage.Remove(request.PhoneNumber);
                return Ok(new { Message = "OTP verified successfully" });
            }
            else
            {
                return Unauthorized("Invalid OTP");
            }
        }
    }

    public class OtpRequest
    {
        public string PhoneNumber { get; set; }
    }

    public class VerifyOtpRequest
    {
        public string PhoneNumber { get; set; }
        public string Otp { get; set; }
    }
}