using Demo.TestEntities;

namespace Demo.PageObjects
{
    public abstract class LoginPageBase
    {
        protected PortalData portalData;

        protected LoginPageBase(PortalData portal)
        {
            portalData = portal;
        }
    }
}

