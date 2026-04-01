namespace Demo.TestEntities
{
    public class PortalData
    {
        public PortalData(Uri portalAdress, User admin)
        {
            Adress = portalAdress ?? throw new ArgumentNullException(nameof(portalAdress));
            Admin = admin ?? throw new ArgumentNullException(nameof(admin));
        }

        public Uri Adress { get; }
        public User Admin { get; }
    }
}
