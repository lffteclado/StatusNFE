using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatusNFE.Models;

namespace StatusNFE.Controllers
{
    public class HomeController : Controller
    {
        List<StatusNfe> casas = new List<StatusNfe>();

        public ActionResult Index()
        {
            LerArquivo("autosete");
            LerArquivo("calisto");
            LerArquivo("cardiesel");

            return View(casas);
        }

        public void LerArquivo(string nomeCasa)
        {
            IEnumerable<string> arquivo = new List<string>();
            string nomeArquivo = string.Empty;//Armazena o nome do Arquivo Lido

            //Armazena as informaçõe do Diretorio, caminho, arquivos e todas as informações do Diretorio"~/Content/Uploads"
            DirectoryInfo dirInfo = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Uploads"));

            //Carrega os arquivo da Pasta Uploads em uma lista
            var listaArquivos = dirInfo.GetFiles();

            //Pesquiso na lista o arquivo com o mesmo código da Empresa Selecionada
            var nome = listaArquivos.Where(x => x.Name.Contains(nomeCasa+".txt"));

            //Mapeado o Diretorio Upload para usá-lo na hora de ler o arquivo encontrado
            var uploadPath = Server.MapPath("~/Content/Uploads");

            //Objeto para Abrir e Fechar o Arquivo.
            StreamReader sr;

            //For para correr a lista de arquivo e encontrar o arquivo correposndente a Casa Escolhida
            foreach (var arq in nome)
            {
                nomeArquivo = arq.Name;
                sr = System.IO.File.OpenText(uploadPath + "/" + nomeArquivo);

                arquivo = System.IO.File.ReadAllLines(uploadPath + "/" + nomeArquivo);

                sr.Close();
            }

            procuraProcessos(arquivo, nomeCasa);

        }

        public void procuraProcessos(IEnumerable<string> arquivo, string nomeCasa)
        {
            List<string> linhas = new List<string>();
            

            foreach (string line in arquivo)
            {
                string lineUpper = line.ToUpper();

                if (lineUpper.Contains("NFE.EXE") || lineUpper.Contains("INTEGRACAO.EXE") || lineUpper.Contains("SCHEDULER EDI.EXE") || lineUpper.Contains("TIMER PROGRAMACAO.EXE") || lineUpper.Contains("NFSE.EXE"))
                {
                    linhas.Add(line);
                }

            }

            StatusNfe casa = new StatusNfe();
            List<string> listaRefinada = new List<string>();

            foreach (var item in linhas)
            {         
                string[] itens = item.Split(' ');

                for (int i = 0; i < itens.Count(); i++)
                {
                    if (itens[i] != "")
                    {
                        listaRefinada.Add(itens[i].ToUpper());

                    }
                }

                casa.Casa = nomeCasa;

                if (listaRefinada.Contains("NFE.EXE"))
                {
                    casa.Nfe = true;
                }

                if (listaRefinada.Contains("NFSE.EXE"))
                {
                    casa.Nfse = true;
                }

                if (listaRefinada.Contains("INTEGRACAO.EXE"))
                {
                    casa.Integracao = true;
                }

                if (listaRefinada.Contains("SCHEDULER"))
                {
                    casa.Edi = true;
                }

                if (listaRefinada.Contains("TIMER"))
                {
                    casa.Timer = true;
                }
            }

            listaRefinada.Clear();
            linhas.Clear();

            casas.Add(casa);

            //if (listaArquivos.Length > 0)
            //{
            //    listaArquivos[0].Delete();
            //}
        }
    }
}