using System.Configuration;
using desafiocoaniquem.Controllers;
using Microsoft.Extensions.Configuration;

namespace desafiocoaniquem.Models
{
    public class Configuration
    {
        public static string getConnectionString()
        {
            if (CoaniquemController._config is not null)
            {
                return CoaniquemController._config["ConnectionStrings:DefaultConnection"];
            }
            else
            {
                return ViewsController._config["ConnectionStrings:DefaultConnection"];
            }
        }
    }
}