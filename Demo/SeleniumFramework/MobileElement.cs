namespace Demo.SeleniumFramework
{
    public class MobileElement : BaseItem
    {
        public MobileElement(string xpathLocator, string description) : this(new List<string> { xpathLocator }, description)
        {
        }

        public MobileElement(List<string> xpathLocators, string description) : base(xpathLocators, description)
        {
        }
    }
}
