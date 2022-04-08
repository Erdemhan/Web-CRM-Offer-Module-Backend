using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using crmweb.Common.Auxiliary;
using crmweb.Common.Extensions;
using crmweb.Data;
using crmweb.Data.Entities;
using crmweb.Models.UserModels;

namespace crmweb.Services
{
    public class AuthService
    {
        //Member Variables/////////////////////////////////////////////////////

        private readonly MainDb Db;
        private readonly IConfiguration Configuration;

        //Constructor//////////////////////////////////////////////////////////

        public AuthService(IConfiguration configuration, MainDb db)
        {
            Configuration = configuration;
            Db = db;
        }

        //Public Functions/////////////////////////////////////////////////////

        public async Task<Result<LoginInfo>> AuthForLogIn(LoginQuery loginQuery)
        {
            return loginQuery.GrantType switch
            {
                "password" => await GrantWithPassword(loginQuery),
                "refresh_token" => await GrantWithRefreshToken(loginQuery),
                _ => Result<LoginInfo>.PrepareFailure("Genel Hata")
            };
        }

        //Private Functions////////////////////////////////////////////////////

        private async Task<Result<LoginInfo>> GrantWithPassword(LoginQuery loginQuery)
        {
            var vClient = new AuthClient();
            Configuration.GetSection("AuthClient").Bind(vClient);

            //Kullanıcı Kontrolü ve Token
            ///////////////////////////////////////////////////////////////////////////////////////

            User vUser = await Db.Users
                .Where(User => User.UserName == loginQuery.UserName)
                .FirstOrDefaultAsync();

            if (vUser == null)
                return Result<LoginInfo>.PrepareFailure("Kullanıcı Bulunamadı!");

            if (vUser.Password != loginQuery.Password)
                return Result<LoginInfo>.PrepareFailure("Girilen Şifre Hatalı");


            var vIdentity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

            vIdentity.AddClaim(new Claim("aud", loginQuery.ClientId));
            vIdentity.AddClaim(new Claim("sub", vUser.Id.ToString()));

            DateTime vIssuedTime = DateTime.UtcNow;

            string vToken = PrepareToken(vClient, vIdentity, vIssuedTime);

            //RefreshToken Oluşturma
            ///////////////////////////////////////////////////////////////////////////////////////

            string vRefreshTokenId = Guid.NewGuid().ToString("n");
            var vRefreshToken = new UserToken()
            {
                Id = vRefreshTokenId.GetHash(),
                ClientId = vClient.Id,
                Subject = vUser.Id.ToString(),
                IssuedUtc = vIssuedTime,
                ExpiresUtc = vIssuedTime.Date.AddMinutes(Convert.ToDouble(vClient.RefreshTokenLifeTime))
            };

            var vTicket = new AuthenticationTicket(new ClaimsPrincipal(vIdentity), new AuthenticationProperties(),
                JwtBearerDefaults.AuthenticationScheme)
            {
                Properties =
                {
                    IssuedUtc = vRefreshToken.IssuedUtc,
                    ExpiresUtc = vRefreshToken.ExpiresUtc
                }
            };

            var vSerializer = new TicketSerializer();
            vRefreshToken.ProtectedTicket = Convert.ToBase64String(vSerializer.Serialize(vTicket));

            //Refresh token daha sonra işlem yapılabilmesi için veritabanına kaydediliyor ve Token'a atanıyor
            ///////////////////////////////////////////////////////////////////////////////////////

            UserToken vPreviousToken = Db.UserTokens.FirstOrDefault(token =>
                token.Subject == vRefreshToken.Subject && token.ClientId == vRefreshToken.ClientId);

            if (vPreviousToken != null)
                Db.UserTokens.Remove(vPreviousToken);

            Db.UserTokens.Add(vRefreshToken);
            await Db.SaveChangesAsync();

            //Cevap oluşturuluyor
            ///////////////////////////////////////////////////////////////////////////////////////

            int vTokenLifeTime = vClient.TokenLifeTime * 60;
            var vLoginInfo = new LoginInfo
            {
                UserId = vUser.Id,
                FullName = vUser.FirstName + " " + vUser.LastName,
                AccessToken = vToken,
                RefreshToken = vRefreshTokenId,
                TokenType = JwtBearerDefaults.AuthenticationScheme,
                ValidFor = vTokenLifeTime - 1,
                Issued = vIssuedTime.ToUniversalTime(),
                Expires = vIssuedTime.AddSeconds(vTokenLifeTime).ToUniversalTime()
            };

            return Result<LoginInfo>.PrepareSuccess(vLoginInfo);
        }

        private async Task<Result<LoginInfo>> GrantWithRefreshToken(LoginQuery loginQuery)
        {
            var vClient = new AuthClient();
            Configuration.GetSection("AuthClient").Bind(vClient);

            // Kaydedilmiş ticket bulunuyor
            //*********************************************************************************************************

            string vHashedTokenId = loginQuery.RefreshToken.GetHash();

            UserToken vUserToken = await Db.UserTokens.FirstOrDefaultAsync(token => token.Id == vHashedTokenId);
            if (vUserToken == null)
                return Result<LoginInfo>.PrepareFailure("Token bulunamadı");


            User vUser = await Db.Users.FindAsync(Convert.ToInt32(vUserToken.Subject));
            if (vUser == null)
                return Result<LoginInfo>.PrepareFailure("Kullanıcı bulunamadı");

            AuthenticationTicket vTicket =
                new TicketSerializer().Deserialize(Convert.FromBase64String(vUserToken.ProtectedTicket));

            //RefreshToken zamanı doldu ise veritabanından siliniyor
            if (vUserToken.ExpiresUtc < DateTime.UtcNow)
            {
                Db.UserTokens.Remove(vUserToken);
                await Db.SaveChangesAsync();

                return Result<LoginInfo>.PrepareFailure("Oturumun süresi dolduğu için yeniden giriş yapmalısınız");
            }

            ClaimsIdentity vIdentity = vTicket.Principal.Identities.FirstOrDefault();
            if (vIdentity == null)
                return Result<LoginInfo>.PrepareFailure("Oturum kimlik hatası");

            // Kaydedilmiş ticketten tekrar token elde ediliyor
            //*********************************************************************************************************

            DateTime vIssuedTime = DateTime.UtcNow;

            string vToken = PrepareToken(vClient, vIdentity, vIssuedTime);

            var vLoginInfo = new LoginInfo
            {
                UserId = vUser.Id,
                FullName = vUser.FirstName + " " + vUser.LastName,
                AccessToken = vToken,
                RefreshToken = loginQuery.RefreshToken,
                TokenType = JwtBearerDefaults.AuthenticationScheme,
                ValidFor = (vClient.TokenLifeTime * 60) - 1,
                Issued = vIssuedTime.ToUniversalTime(),
                Expires = vIssuedTime.AddMinutes(vClient.TokenLifeTime).ToUniversalTime()
            };

            return Result<LoginInfo>.PrepareSuccess(vLoginInfo);
        }

        private string PrepareToken(AuthClient client, ClaimsIdentity identity, DateTime issuedTime,
            DateTime? expiresTime = null)
        {
            var vClientId = "";
            if (identity.HasClaim(claim => claim.Type == "aud"))
                vClientId = identity.Claims.GetValue("aud");

            if (vClientId == "")
                throw new ArgumentException("Geçersiz istemci id");

            if (vClientId != client.Id)
                throw new ArgumentException("İstemci aynı değil");

            byte[] vSymmetricKeyAsBase64 = Base64UrlTextEncoder.Decode(client.Secret);
            var vSecurityKey = new SymmetricSecurityKey(vSymmetricKeyAsBase64) { KeyId = client.KeyId };
            var vSigningKey = new SigningCredentials(vSecurityKey, SecurityAlgorithms.HmacSha256);

            DateTime vIssued = issuedTime;
            DateTime vExpires = expiresTime ?? issuedTime.AddYears(1); //client.TokenLifeTime);

            if (vIssued > vExpires)
                vIssued = vExpires.AddDays(-1);

            var vTokenHandler = new JwtSecurityTokenHandler();
            vTokenHandler.OutboundAlgorithmMap.Clear();

            return vTokenHandler.CreateEncodedJwt(
                client.Issuer,
                null,
                identity,
                vIssued,
                vExpires,
                issuedTime,
                vSigningKey);
        }
    }
}