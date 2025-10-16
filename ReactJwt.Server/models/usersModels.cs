namespace ReactJwt.Server.Models
{
    public class usersModels
    {
        //private UsersStoreContext context;
        public int Id { get; set; }
        public string userName { get; set; } = null!;
        public string password_hash { get; set; } = null!;

        public string password_salt { get; set; } = null!;

    }
}
