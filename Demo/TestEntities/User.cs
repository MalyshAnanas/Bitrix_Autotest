using Demo.BaseFramework.ScriptInterraction;

namespace Demo.TestEntities
{
    public class User
    {
        public User() { }

        public User(
            string emailAkaLogin,
            string password,
            string name,
            string lastName)
        {
            Login = emailAkaLogin;
            Password = password;
            Name = name;
            LastName = lastName;
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NameLastName => Name + " " + LastName;

        public string GetDBid(Uri portalAdress, User portalAdmin)
        {
            var result = DatabaseExecutor.ExecuteQuery("select ID from b" +
                "_user where EMAIL = '" + Login + "'", portalAdress, portalAdmin);
            return result.Count == 0 ? null : result[0].ID;
        }
    }
}
