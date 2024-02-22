﻿using JWT_EF_Core.Interface;
using JWT_EF_Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JWT_EF_Core.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        }

        public string CreateToken(AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}
