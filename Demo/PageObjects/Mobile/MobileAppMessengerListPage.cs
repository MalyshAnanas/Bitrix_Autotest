using Demo.BaseFramework;
using Demo.SeleniumFramework;
using Demo.TestEntities;

namespace Demo.PageObjects.Mobile;

public class MobileAppMessengerListPage
{
    public MobileAppCollabCreatePage OpenCreateCollab()
    {
        createNewChatBtn.Click();
        createNewCollabBtn.Click();

        return new MobileAppCollabCreatePage();
    }

    public bool IsCollabDisplayed(B24CollabEntity collab)
    {
        var collabTitle = new MobileElement(
            $"//android.widget.TextView[@content-desc=\"{collab.Name}\"]",
            $"Коллаба {collab.Name}");

        return WaitersCore.WaitForConditionReached(
            () => collabTitle.WaitDisplayed(),
            2, 6,
            $"Ожидание коллабы {collab.Name}");
    }

    #region Elements
    private MobileElement createNewChatBtn => new MobileElement(
        "//android.widget.ImageButton[@resource-id=\"com.bitrix24.android:id/component_fab\"]",
        "Кнопка '+'");

    private MobileElement createNewCollabBtn => new MobileElement(
        "//android.view.ViewGroup[@content-desc=\"create_collab\"]",
        "Создать коллабу");
    #endregion
}