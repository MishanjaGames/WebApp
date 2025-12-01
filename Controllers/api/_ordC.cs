using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace WebApp.api
{
    [Route("api/order")]
    [ApiController]
    public class ordController : Controller
    {
        private readonly ISo _serviceOrd;
        public ordController(ISo serviceOrd)
        {
            _serviceOrd = serviceOrd;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(
            [FromBody] IEnumerable<CartItem> cartItems)
        {
            //var token = HttpContext.Request.Headers.Authorization;
            //var data = ParseJWT(token);
            //Console.WriteLine(data);

            return null;
        }

        private async Task<string> ParseJWT(String token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken tokenData = tokenHandler.ReadJwtToken(token);
            Console.WriteLine(tokenData.RawPayload);
            Console.WriteLine(tokenData.Claims);
            Console.WriteLine(tokenData.Payload);
            Console.WriteLine(tokenData.RawData);
            return null;
        }
    }
}
