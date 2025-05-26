

namespace Models
{
    public class WebSiteModel : ModelBase
    {
        public WebSiteModel(string name, string login, string password, string webAddress, bool isFavourite) : base(name, isFavourite)
        {
            
            Login = login;
            Password = password;
            
            WebAddress = webAddress;
            
        }
        
        public string Login { get;  set; }
        public string Password { get;  set; }
        public string WebAddress { get;  set; }
    }
}
