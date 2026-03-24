public class GlobalConfiguration
{
    private static IConfiguration _configuration;

    public GlobalConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static ColectaConfig colectaConfig = new ColectaConfig();
    public static bool IsColecta => DateTime.Now >= Convert.ToDateTime(_configuration["AppSettings:Colecta:Fecha"]) ? true : false;
    public static string SantanderByPassURL => _configuration["AppSettings:SantanderByPassURL"];

    public static string EmailImage()
    {
        if (DateTime.Now > Convert.ToDateTime(_configuration["AppSettings:Colecta:Fecha"]))
        {
            //Imagen de colecta
            return _configuration["AppSettings:Colecta:EmailImage"];
        }
        else
        {
            //Imagen default
            return _configuration["AppSettings:EmailImage"];
        }
    }

    public class ColectaConfig
    {
        public DateTime Fecha => Convert.ToDateTime(_configuration["AppSettings:Colecta:Fecha"]);
        public IConfiguration Images => _configuration.GetSection("AppSettings:Colecta:Images");

    }

}