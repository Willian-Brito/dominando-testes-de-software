using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace NerdStore.BDD.Tests.Config;

public class SeleniumHelper : IDisposable
{
    public IWebDriver WebDriver;
    public readonly ConfigurationHelper Configuration;
    public WebDriverWait Wait;
    
    public SeleniumHelper(Browser browser, ConfigurationHelper configuration, bool headless = true)
    {
        Configuration = configuration;
        WebDriver = WebDriverFactory.CreateWebDriver(browser, Configuration.WebDrivers, headless);
        WebDriver.Manage().Window.Maximize();
        Wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30));
    }
    
    public string ObterUrl()
    {
        return WebDriver.Url;
    }

    public void IrParaUrl(string url)
    {
        WebDriver.Navigate().GoToUrl(url);
    }
    
    public bool ValidarConteudoUrl(string conteudo)
    {
        return Wait.Until(driver => driver.Url.Contains(conteudo));
    }

    public void ClicarLinkPorTexto(string linkText)
    {
        var link = EsperarElementoVisivel(By.PartialLinkText(linkText));
        link.Click();
    }

    public void ClicarBotaoPorId(string botaoId)
    {
        var botao = EsperarElementoVisivel(By.Id(botaoId));
        botao.Click();
    }

    public void ClicarPorXPath(string xPath)
    {
        var elemento = EsperarElementoVisivel(By.XPath(xPath));
        elemento.Click();
    }

    public IWebElement ObterElementoPorClasse(string classeCss)
    {
        return EsperarElementoVisivel(By.ClassName(classeCss));
    }

    public IWebElement ObterElementoPorXPath(string xPath)
    {
        return EsperarElementoVisivel(By.XPath(xPath));
    }

    public void PreencherTextBoxPorId(string idCampo, string valorCampo)
    {
        var campo = EsperarElementoVisivel(By.Id(idCampo));
        campo.Clear();
        campo.SendKeys(valorCampo);
    }

    public void PreencherDropDownPorId(string idCampo, string valorCampo)
    {
        var campo = EsperarElementoVisivel(By.Id(idCampo));
        var selectElement = new SelectElement(campo);
        selectElement.SelectByValue(valorCampo);
    }

    public string ObterTextoElementoPorClasseCss(string className)
    {
        return EsperarElementoVisivel(By.ClassName(className)).Text;
    }

    public string ObterTextoElementoPorId(string id)
    {
        return EsperarElementoVisivel(By.Id(id)).Text;
    }

    public string ObterValorTextBoxPorId(string id)
    {
        return EsperarElementoVisivel(By.Id(id)).GetAttribute("value");
    }

    public IEnumerable<IWebElement> ObterListaPorClasse(string className)
    {
        return EsperarElementosPresentes(By.ClassName(className));
    }

    public bool ValidarSeElementoExistePorId(string id)
    {
        return ElementoExistente(By.Id(id));
    }

    public void VoltarNavegacao(int vezes = 1)
    {
        for (var i = 0; i < vezes; i++)
        {
            WebDriver.Navigate().Back();
        }
    }

    public void ObterScreenShot(string nome)
    {
        SalvarScreenShot(WebDriver.TakeScreenshot(), $"{DateTime.Now.ToFileTime()}_{nome}.png");
    }

    private void SalvarScreenShot(Screenshot screenshot, string fileName)
    {
        screenshot.SaveAsFile($"{Configuration.FolderPicture}{fileName}");
        // screenshot.SaveAsFile($"{Configuration.FolderPicture}{fileName}", ScreenshotImageFormat.Png);
    }

    private bool ElementoExistente(By by)
    {
        try
        {
            WebDriver.FindElement(by);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
    
    private IWebElement EsperarElementoVisivel(By by)
    {
        return Wait.Until(driver =>
        {
            var el = driver.FindElement(by);
            return el.Displayed ? el : null;
        });
    }

    private IEnumerable<IWebElement> EsperarElementosPresentes(By by)
    {
        return Wait.Until(driver =>
        {
            var list = driver.FindElements(by);
            return list.Any() ? list : null;
        });
    }
    
    public void Dispose()
    {
        WebDriver.Quit();
        WebDriver.Dispose();
    }
}