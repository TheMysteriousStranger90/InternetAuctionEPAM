using System.Collections.Generic;

namespace BLL.Models
{
    public class AuthResponseModel
    {
        public string Token { get; set; }

        public bool Success { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
