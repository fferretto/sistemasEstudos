using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PagNet.Application.Application
{
    public class ValidaUsuarioApp : IValidaUsuarioApp
    {
        private readonly IPagNet_MenuService _menu;
        private readonly IPagNet_RelatorioService _relatorio;
        private readonly IOperadoraService _ope;


        public ValidaUsuarioApp(IPagNet_MenuService menu,
                                IOperadoraService ope,
                                IPagNet_RelatorioService relatorio)
        {
            _menu = menu;
            _relatorio = relatorio;
            _ope = ope;
        }

        public static string HashValue(string value)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hashBytes;
            using (HashAlgorithm hash = SHA1.Create())
                hashBytes = hash.ComputeHash(encoding.GetBytes(value));

            StringBuilder hashValue = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
            {
                hashValue.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
            }

            return hashValue.ToString();
        }

        public List<MenuVMs> CarregaMenus(int cod_ope, string PerfilOperadora, string caminhoImagemDefault, bool PossuiNetCard, string login)
        {
            var menus = new List<MenuVMs>();
            DadosLayoutVm css = new DadosLayoutVm();

            var listAllMenus = _menu.ListaMenusAtivos();
            var ListaMenuRelaorio = _relatorio.GetAllRelatorios();

            if (login.IndexOf("tln@") < 0)
            {
                listAllMenus.Remove(listAllMenus.Where(x => x.Nome == "mnuEmpresa").FirstOrDefault());
                listAllMenus.Remove(listAllMenus.Where(x => x.Nome == "mnuConfigDescontoFolha").FirstOrDefault());
            }
            
            var listaMenusPai = listAllMenus.Where(x => x.Nivel == 1).OrderBy(x => x.Ordem).ToList();


            foreach (var pai in listaMenusPai)
            {
                if (pai.Nome != "mnuRelatorio")
                {
                    menus.Add(MenuVMs.ToViewMenus(pai, listAllMenus));
                }
                else
                {
                    menus.Add(MenuVMs.ToViewMenusRelatorio(pai, ListaMenuRelaorio));
                }
            }

            return menus;
        }

        public string getCaminhoCss(int cod_ope, string PerfilOperadora, string caminhoDefault, int codEmpresa)
        {
            var Operadora = _ope.GetOperadoraById(cod_ope).Result;

            string CaminhoArquivo = Path.Combine(Operadora.CAMINHOARQUIVO, Operadora.NOMOPERAFIL, codEmpresa.ToString(), "ClasseCss");

            return GetClassCss(PerfilOperadora, CaminhoArquivo, caminhoDefault);
        }

        public string getCaminhoLogo(int cod_ope, string PerfilOperadora, string caminhoDefault, int codEmpresa)
        {
                var Operadora = _ope.GetOperadoraById(cod_ope).Result;

                string CaminhoArquivo = Path.Combine(Operadora.CAMINHOARQUIVO, Operadora.NOMOPERAFIL, codEmpresa.ToString(), "Imagens", "Logo");

                return GetNomeLogo(PerfilOperadora, CaminhoArquivo, caminhoDefault);
        }

        private string GetNomeLogo(string PerfilOperadora, string caminhoImageRede, string caminhoDefault)
        {
            var nmArquivo = "V1_ImagemLogo.png";
            var fullNameCaminhoDefault = Path.Combine(caminhoDefault, "images", "Logos", "LogoDefault", "LogoPagNet.png");

            if (!Directory.Exists(caminhoImageRede))
            {
                Directory.CreateDirectory(caminhoImageRede);
                var path = Path.Combine(caminhoImageRede, nmArquivo);
                File.Copy(fullNameCaminhoDefault, path, true);
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(caminhoImageRede);

                var file = dir.GetFiles("*.*").ToList();
                if (file.Count == 0)
                {
                    var path = Path.Combine(caminhoImageRede, nmArquivo);
                    File.Copy(fullNameCaminhoDefault, path, true);
                }
                else
                {
                    var arquivo = file.Where(y => !y.Name.Contains("New") && !y.Name.Contains("Thumbs")).OrderByDescending(x => x.LastAccessTime).FirstOrDefault();
                    if (arquivo != null)
                    {
                        nmArquivo = arquivo.Name;
                    }
                    else
                    {
                        var path = Path.Combine(caminhoImageRede, nmArquivo);
                        File.Copy(fullNameCaminhoDefault, path, true);
                    }


                }
            }

            var caminhoRetorno = Path.Combine(caminhoImageRede, nmArquivo);

            return caminhoRetorno;
        }
        private string GetClassCss(string PerfilOperadora, string caminhoClasseCss, string caminhoDefault)
        {

            var nmArquivo = "V1_Style.css";
            
            if (!Directory.Exists(caminhoClasseCss))
            {
                Directory.CreateDirectory(caminhoClasseCss);
                var path = Path.Combine(caminhoClasseCss, nmArquivo);
                File.Copy(Path.Combine(caminhoDefault, "css", "site.css"), path, true);
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(caminhoClasseCss);
                var file = dir.GetFiles("*.*").ToList();
                if (file.Count == 0)
                {
                    var path = Path.Combine(caminhoClasseCss, nmArquivo);
                    File.Copy(Path.Combine(caminhoDefault, "css", "site.css"), path, true);
                }
                else
                {
                    var arquivo = file.OrderByDescending(x => x.LastAccessTime).FirstOrDefault();
                    nmArquivo = arquivo.Name;

                }
            }


            var caminhoRetorno = Path.Combine(caminhoClasseCss, nmArquivo);
            
            return caminhoRetorno;
        }

        public string getCaminhoIco(int cod_ope, string PerfilOperadora, string caminhoDefault, int codEmpresa)
        {
            var Operadora = _ope.GetOperadoraById(cod_ope).Result;

            string CaminhoArquivo = Path.Combine(Operadora.CAMINHOARQUIVO, Operadora.NOMOPERAFIL, codEmpresa.ToString(), "Imagens", "Icone");

            return GetNomeIco(PerfilOperadora, CaminhoArquivo, caminhoDefault);
        }

        private string GetNomeIco(string PerfilOperadora, string caminhoImageRede, string caminhoDefault)
        {
            var nmArquivo = "V1_ImagemIcone.ico";
            var fullNameCaminhoDefault = Path.Combine(caminhoDefault, "images", "icoPagNet.ico");



            if (!Directory.Exists(caminhoImageRede))
            {
                Directory.CreateDirectory(caminhoImageRede);
                var path = Path.Combine(caminhoImageRede, nmArquivo);
                File.Copy(fullNameCaminhoDefault, path, true);
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(caminhoImageRede);

                var file = dir.GetFiles("*.*").ToList();
                if (file.Count == 0)
                {
                    var path = Path.Combine(caminhoImageRede, nmArquivo);
                    File.Copy(fullNameCaminhoDefault, path, true);
                }
                else
                {
                    var arquivo = file.Where(y => !y.Name.Contains("New") && !y.Name.Contains("Thumbs")).OrderByDescending(x => x.LastAccessTime).FirstOrDefault();
                    if (arquivo != null)
                    {
                        nmArquivo = arquivo.Name;
                    }
                    else
                    {
                        var path = Path.Combine(caminhoImageRede, nmArquivo);
                        File.Copy(fullNameCaminhoDefault, path, true);
                    }

                }
            }

            var caminhoRetorno = Path.Combine(caminhoImageRede, nmArquivo);

            return caminhoRetorno;
        }

        public bool ValidaBaseDados(int cod_ope, string BaseDados)
        {
            try
            {
                var dados = _ope.GetOperadoraById(cod_ope).Result;
                var nmBaseDados = dados.BD_NC;

                if (BaseDados != nmBaseDados)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

