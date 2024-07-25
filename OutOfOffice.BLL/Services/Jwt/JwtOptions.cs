using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.BLL.Services.Jwt
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;

        public int ExpiresHours { get; set; }

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;
    }
}
