using Demo.SeleniumFramework;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.Disk;

public class RecycleBinPage
{
    public IWebDriver Driver { get; }

    public RecycleBinPage(IWebDriver driver = default)
    {
        Driver = driver;
    }

    private WebItemWrap fileForDelete = new WebItemWrap(
        "//table[@id='trashcan_3_table']//tr[@class='main-grid-row main-grid-row-body'][1]",
        "Первый файл из списка файлов на второй странице");

    private WebItemWrap btnDelete = new WebItemWrap("//div[@class='ui-action-panel-item '][3]",
        "Кнопка 'Удалить' в корзине");

    private WebItemWrap btnDeleteForever = new WebItemWrap("//span[@class='ui-btn ui-btn-light-border']",
        "Кнопка 'Удалить навсегда'");

    public RecycleBinPage OpenPage(int pageNumber)
    {
        // XPath элемента с номером текущей страницы
        var currentPage = new WebItemWrap(
            "//span[@class='main-ui-pagination-page main-ui-pagination-active']",
            "Текущая активная страница");

        string currentPageText = currentPage.InnerText()?.Trim();

        // Проверяем, не совпадает ли номер текущей страницы с нужной
        if (currentPageText == pageNumber.ToString())
        {
            // Уже на нужной странице, кликать не нужно
            return new RecycleBinPage(Driver);
        }

        // Иначе кликаем по кнопке нужной страницы
        var btnPage = new WebItemWrap(
            $"//a[@class='main-ui-pagination-page' and text()='{pageNumber}']",
            $"Кнопка для перехода на страницу {pageNumber}");

        btnPage.Click(Driver);

        return new RecycleBinPage(Driver);
    }


    public RecycleBinPage Delete()
    {
        fileForDelete.Click(Driver);
        btnDelete.Click(Driver);
        btnDeleteForever.Click(Driver);
        return this;
    }

    public int GetCountFile()
    {
        int fileCount = 0;

        while (true)
        {
            // Считаем файлы на текущей странице
            var counter = new WebItemWrap(
                "//span[contains(@class,'main-grid-counter-displayed')]",
                "Количество файлов на странице");

            string textValue = counter.InnerText()?.Trim();

            if (!int.TryParse(textValue, out int number))
                throw new Exception($"Не удалось преобразовать '{textValue}' в число.");

            fileCount += number;

            // Проверяем кнопку "Следующая"
            var nextButton = new WebItemWrap(
                "//a[contains(@class,'main-ui-pagination-next') and not(contains(@class,'disabled'))]",
                "Кнопка следующей страницы");

            if (!nextButton.Exists())
                break;

            // Переходим дальше
            nextButton.Click(Driver);
        }

        return fileCount;
    }

}