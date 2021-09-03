using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Usabit.Framework.Message.Concrete
{
    public class ApplicationRequest
    {
        [JsonIgnore]
        public Guid IdentityId { get; set; }

        [JsonIgnore]
        public string Username { get; set; }

        public void HandleToken(string token) 
        {
            if (token == null)
                return;

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var jwt = jsonToken as JwtSecurityToken;

            string identityId = jwt.Claims.FirstOrDefault(it => it.Type == "primarysid").Value;
            string userName = jwt.Claims.FirstOrDefault(it => it.Type == "nameid").Value;

            if (!string.IsNullOrEmpty(identityId))
                IdentityId = new Guid(identityId);

            if (!string.IsNullOrEmpty(userName))
                Username = userName;
        }
    }
}
