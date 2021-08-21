using CMS_CarnesDonFernando.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CMS_CarnesDonFernando.Helpers
{
    public class TokenProviderHelper
    {

        public string LoginUser(string pEmail, string pPassword)
        {
            //Get user details for the user who is trying to login - JRozario
            var user = UserList.SingleOrDefault(x => x.UserMail == pEmail);

            //Authenticate User, Check if its a registered user in DB
            if (user == null)
                return null;

            //If its registered user, check user password stored in DB
            //For demo, password is not hashed. Its just a string comparison
            //In reality, password would be hashed and stored in DB. Before comparing, hash the password
            if (pPassword == user.UserPassword)
            {
                //Authentication successful, Issue Token with user credentials
                //Provide the security key which was given in the JWToken configuration in Startup.cs
                var key = Encoding.ASCII.GetBytes("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
                //Generate Token for user
                var JWToken = new JwtSecurityToken(
                    //issuer: "http://localhost:63734/",
                    //audience: "http://localhost:63734/",
                    issuer: "http://cmsmomentosdonfernando.azurewebsites.net/",
                    audience: "http://cmsmomentosdonfernando.azurewebsites.net/",
                    claims: GetUserClaims(user),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                    //Using HS256 Algorithm to encrypt Token - JRozario
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                return token;
            }
            else
            {
                return null;
            }
        }

        
        private List<UserModel> UserList = new List<UserModel>
        {
            new UserModel { UserMail = "mercadeo@carnesdonfernando.com", UserPassword = "M0m3nt0sD0nF3rn@nd0" },
        };

        
        private IEnumerable<Claim> GetUserClaims(UserModel user)
        {
            IEnumerable<Claim> claims = new Claim[]
                    {
                    new Claim(ClaimTypes.Email, user.UserMail),
                    new Claim("", user.UserPassword)
                    };
            return claims;
        }


    }
}
