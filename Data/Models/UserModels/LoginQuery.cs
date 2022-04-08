namespace crmweb.Models.UserModels
{
    public class LoginQuery
    {
        public string GrantType { get; set; }
        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte Application { get; set; }
    }
}
