using KmacHelper.KMAC;
using MicroServicesApi.Dtos;
using MicroServicesApi.Helpers;
using MicroServicesApi.Models;
using MicroServicesApi.ViewModels;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroServicesApi.Repository
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<AppUser> _users;
        private readonly IConfiguration _config;

        public UserService(IMongoDbSettings settings, IConfiguration config)
        {
            _config = config;
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            _users = db.GetCollection<AppUser>(nameof(AppUser));
        }

        public async Task<MobileResponse<RegisterViewDto>> RegisterUser(RegisterViewModel model, CancellationToken ct)
        {
            if (await _users.Find(x => x.Email == model.Email).AnyAsync(ct))
                return MobileResponse<RegisterViewDto>.Fail("Email is already registered.");

            if (await _users.Find(x => x.CNIC == model.CNIC).AnyAsync(ct))
                return MobileResponse<RegisterViewDto>.Fail("CNIC is already registered.");

            var user = new AppUser
            {
                UserName = model.Username.Trim(),
                CNIC = model.CNIC.Trim(),
                Email = model.Email.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                DateCreated = DateTime.UtcNow,
                Roles = new List<string> { model.Role }
            };

            await _users.InsertOneAsync(user, cancellationToken: ct);

            return MobileResponse<RegisterViewDto>.Success(new RegisterViewDto(user.Id, user.CNIC), "Register Successful");
        }

        public async Task<MobileResponse<LoginResponseModel>> LoginUser(LoginViewModel model, CancellationToken ct)
        {
            var user = await _users.Find(x => x.CNIC == model.CNIC).FirstOrDefaultAsync(ct);
            if (user is null)
                return MobileResponse<LoginResponseModel>.Fail("CNIC is invalid.", "404");

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                return MobileResponse<LoginResponseModel>.Fail("Password is invalid.", "404");

            var jwt = GenerateKmacJwtToken(user.Id, user.Roles, user.Email);
            var expiryMinutes = _config.GetValue<int>("JWTKey:TokenExpiryTimeInMinutes");
            var expireTime = DateTime.UtcNow.AddMinutes(expiryMinutes);

            return MobileResponse<LoginResponseModel>.Success(new LoginResponseModel
            {
                Id = user.Id,
                AccessToken = jwt,
                ExpireTokenTime = expireTime,
                //RefreshToken = "" // add refresh logic if needed
            }, "Login Successful", "200");
        }

        private string GenerateKmacJwtToken(string userId, IEnumerable<string> roles, string email)
        {
            var secret = _config["JWTKey:Secret"] ?? throw new InvalidOperationException("JWT secret is missing.");
            var issuer = _config["JWTKey:ValidIssuer"];
            var audience = _config["JWTKey:ValidAudience"];
            var expiryMinutes = int.Parse(_config["JWTKey:TokenExpiryTimeInMinutes"] ?? "30");

            var derivedKey = KmacHelperClass.DeriveKmacKey(userId, roles, email, Encoding.UTF8.GetBytes(secret));

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
            claims.AddRange(roles.Select(r => new Claim("role", r)));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(derivedKey), SecurityAlgorithms.HmacSha512)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //private static byte[] DeriveKmacKey(string userId, IEnumerable<string> roles, string email, byte[] secret)
        //{
        //    var rolesStr = string.Join(",", roles.OrderBy(r => r));
        //    var inputData = Encoding.UTF8.GetBytes($"{userId}|{rolesStr}|{email}");

        //    var kmac = new Org.BouncyCastle.Crypto.Macs.KMac(256, secret);
        //    kmac.Init(new Org.BouncyCastle.Crypto.Parameters.KeyParameter(secret));
        //    kmac.BlockUpdate(inputData, 0, inputData.Length);

        //    var output = new byte[kmac.GetMacSize()];
        //    kmac.DoFinal(output, 0);
        //    return output;
        //}
    }
}
