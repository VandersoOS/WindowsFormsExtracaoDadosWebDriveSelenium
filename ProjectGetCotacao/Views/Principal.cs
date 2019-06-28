using CotacaoDolar.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ProjectGetCotacao.ConexaoBD;
using ProjectGetCotacao.ConexaoBD.Repository;
using System;
using System.Windows.Forms;
using TesteCotacaoDolar.Views;

namespace ProjectGetCotacao.Views
{
    public partial class Principal : Form
    {
        private IWebDriver _driver;
        private Cotacao _cotacao;
        private SeleniumConfiguracao _configuracao;
        private string valor;
        private DateTime data;

        public Principal()
        {
            InitializeComponent();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            //Aqui será chamado os metodos para a extração dos primeiros dados.
            //Ao sair do Load o processo será executado automáticamente.
            //A cada 5 minutos.
            CriarObjConfig();
            ExecutarExtracaoDados();
        }

        private void ExecutarExtracaoDados()
        {
            CarreagarCaminhoChrmeDriver();

            System.Threading.Thread.Sleep(1000);
            CarregarPagina();

            System.Threading.Thread.Sleep(1000);
            ObterCotacoes();

            if (!ValidarEntracaoDados())
            {
                return;
            }

            CriarObjCotacao();

            System.Threading.Thread.Sleep(1000);
            Gravar();

            Fechar();

            timer.Start();
        }

        private void CriarObjConfig()
        {
            _configuracao = new SeleniumConfiguracao
            {
                //Sera necessário mudar o caminho de acordo com o seu computador.
                //O arquivo está dentro do projeto, e deverá apenas mudar o caminho inical do diretório.
                //Segue exemplo abaixo
                CaminhoDriverChrome = "C:\\ProjectGetCotacao\\packages\\Selenium.WebDriver.ChromeDriver.75.0.3770.90\\driver\\win32\\",

                //Por questões éticas não posso deixar a URL que efetuei a extração dos dados
                //Sugiro que caso queira testar a extração procure um site que exiba as cotações do dólar em tempo real
                UrlPaginaCotacoes = "https://www.procureurl.com",
                Timeout = 60
            };
        }

        private void CarreagarCaminhoChrmeDriver()
        {
            ChromeOptions optionsFF = new ChromeOptions();
            optionsFF.AddArgument("--headless");

            try
            {
                _driver = new ChromeDriver(_configuracao.CaminhoDriverChrome, optionsFF);
            }
            catch (Exception)
            {
                MessageBox.Show("Erro Verifique caminho configuração do CaminhoDriverChrome, está incorreto!!" + Environment.NewLine + "Corrija e reinicie a aplicação", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        public void CarregarPagina()
        {
            try
            {
                _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
                _driver.Navigate().GoToUrl(_configuracao.UrlPaginaCotacoes);
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao carregar página!!", "" + Environment.NewLine + "Página pode estar fora do ar!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ObterCotacoes()
        {
            try
            {
                //pegue o elemento da página via
                //.FindElement(By.Id("DolarComercial")).GetAttribute("value");
                //.FindElement(By.ClassName("container"))
                //.FindElement(By.TagName("tbody"))
                //.FindElements(By.TagName("tr"));
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                wait.Until((d) => d.FindElement(By.Id("comercial")) != null);

                valor = _driver.FindElement(By.Id("comercial")).GetAttribute("value");
                data = DateTime.Now;
            }
            catch (Exception)
            {
                MessageBox.Show("Erro na extração dos dados!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }


        private bool ValidarEntracaoDados()
        {
            if (string.IsNullOrEmpty(valor))
            {
                MessageBox.Show("Erro Valor invalido");
                return false;
            }

            if (data == default(DateTime))
            {
                MessageBox.Show("Erro Data");
                return false;
            }

            return true;
        }
        private void CriarObjCotacao()
        {
            _cotacao = new Cotacao
            {
                Valor = decimal.Parse(valor.ToString()),
                DataInclusao = data
            };
        }

        private void Gravar()
        {
            Repository.GravarDb(_cotacao);

            if (!string.IsNullOrEmpty(Repository.ErrorMessage))
            {
                MessageBox.Show(Repository.ErrorMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            ListarCotacoes f = new ListarCotacoes();
            f.ShowDialog();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //A cada 5 minutos essa operação será executada!!
            timer.Stop();
            ExecutarExtracaoDados();
            timer.Start();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Deseja realmente sair da Aplicação?", "" + Environment.NewLine + "Todos os Processos serão interrompidos!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Application.Exit();
        }
    }
}
