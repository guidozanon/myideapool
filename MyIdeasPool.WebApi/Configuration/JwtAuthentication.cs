﻿using Microsoft.IdentityModel.Tokens;
using System;

namespace MyIdeasPool.WebApi.Configuration
{
	public class JwtAuthentication
	{
		public string SecurityKey { get; set; }
		public string ValidIssuer { get; set; }
		public string ValidAudience { get; set; }

		public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Convert.FromBase64String(SecurityKey));
		public SigningCredentials SigningCredentials => new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
	}

}