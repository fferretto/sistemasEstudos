using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Application.Application
{
    public class AparenciaSistemaApp : IAparenciaSistemaApp
    {
        private readonly IPagNet_MenuService _menu;
        private readonly IPagNet_RelatorioService _relatorio;
        private readonly IOperadoraService _ope;


        public AparenciaSistemaApp(IPagNet_MenuService menu,
                                IOperadoraService ope,
                                IPagNet_RelatorioService relatorio)
        {
            _menu = menu;
            _relatorio = relatorio;
            _ope = ope;
        }

        public AparenciaSistemaVm CarregaLayoutAtual(int codOper, int codEmpresa)
        {
            AparenciaSistemaVm Aparencia = new AparenciaSistemaVm();

            var Operadora = _ope.GetOperadoraById(codOper).Result;

            string CaminhoArquivo = Path.Combine(Operadora.CAMINHOARQUIVO, Operadora.NOMOPERAFIL, codEmpresa.ToString(), "ClasseCss");

            DirectoryInfo dir = new DirectoryInfo(CaminhoArquivo);
            var file = dir.GetFiles("*.*").ToList();

            var arquivo = file.Where(y => !y.Name.Contains("Thumbs")).OrderByDescending(x => x.LastAccessTime).FirstOrDefault();

            return CarregaVmByClasse(arquivo.FullName);
        }
        private AparenciaSistemaVm CarregaVmByClasse(string CaminhoCss)
        {
            try
            {
                AparenciaSistemaVm Aparencia = new AparenciaSistemaVm();

                var linhasArquivo = File.ReadAllLines(CaminhoCss);
                for (int i = 0; i < linhasArquivo.Length; i++)
                {
                    //Cor do cabeção e menu
                    if (linhasArquivo[i].Contains(".Color-Header-menu"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("background-color:"))
                            {
                                var posicao = linhasArquivo[i].Split(':');
                                Aparencia.Cabecaho = posicao[1].Replace(";", "").Trim();

                            }
                            i++;
                        }
                    }
                    //Alteração das cores das linhas do cabecalho, menu e informações do usuario
                    else if (linhasArquivo[i].Contains(".page-header {")) // LinhaCabecalho
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("border-color:"))
                            {
                                var posicao = linhasArquivo[i].Split(':');
                                Aparencia.LinhaCabecalho = posicao[1].Replace(";", "").Trim();
                            }
                            i++;
                        }
                    }
                    //Alteração das cores do texto do cabeçalho
                    else if (linhasArquivo[i].Contains(".text-header {")) //corTexto1
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("color:"))
                            {
                                var posicao = linhasArquivo[i].Split(':');
                                Aparencia.corTexto1 = posicao[1].Replace(";", "").Trim();
                            }
                            i++;
                        }
                    }
                    //Alteração das cores da informações do usuario
                    else if (linhasArquivo[i].Contains(".info-user {")) //corTexto1
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("color:"))
                            {
                                var posicao = linhasArquivo[i].Split(':');
                                Aparencia.corTexto2 = posicao[1].Replace(";", "").Trim();
                            }
                            i++;
                        }
                    }
                    else if (linhasArquivo[i].Contains(".mnuAlteraSenha")) //corTexto3
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("color:"))
                            {
                                var posicao = linhasArquivo[i].Split(':');
                                Aparencia.corTexto3 = posicao[1].Replace(";", "").Trim();
                            }
                            i++;
                        }
                    }
                    else if (linhasArquivo[i].Contains(".menu li a {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("color:"))
                            {
                                var posicao = linhasArquivo[i].Split(':');
                                Aparencia.corTexto4 = posicao[1].Replace(";", "").Trim();
                            }
                            i++;
                        }
                    }
                    //Alteração da Cor do Painel
                    else if (linhasArquivo[i].Contains(".panel-heading {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("background-color:"))
                            {
                                var posicao = linhasArquivo[i].Split(':');
                                Aparencia.PainelSuperior = posicao[1].Replace(";", "").Trim();
                            }
                            else if (linhasArquivo[i].Contains("border-color:"))
                            {
                                var posicao = linhasArquivo[i].Split(':');
                                Aparencia.LinhaPainel = posicao[1].Replace(";", "").Trim();
                            }
                            i++;
                        }
                    }
                    //Alteração da Cor dos Botões
                    else if (linhasArquivo[i].Contains(".btn-success {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("background-color:"))
                            {
                                var posicao = linhasArquivo[i].Split(':');
                                Aparencia.btnSucesso = posicao[1].Replace(";", "").Trim();
                            }
                            i++;
                        }
                    }
                    else if (linhasArquivo[i].Contains(".btn-danger {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("background-color:"))
                            {
                                var posicao = linhasArquivo[i].Split(':');
                                Aparencia.btnDanger = posicao[1].Replace(";", "").Trim();
                            }
                            i++;
                        }
                    }
                    else if (linhasArquivo[i].Contains(".btn-default {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("background-color:"))
                            {
                                var posicao = linhasArquivo[i].Split(':');
                                Aparencia.btnDefault = posicao[1].Replace(";", "").Trim();
                            }
                            i++;
                        }
                    }
                }

                return Aparencia;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IDictionary<string, string>> LayoutDefaultPagNet(AparenciaSistemaVm model)
        {
            try
            {
                string CaminhoArquivo = Path.Combine(model.CaminhoDefaultPagNet, "site.css");

                AparenciaSistemaVm VmDefault = CarregaVmByClasse(CaminhoArquivo);
                VmDefault.CodOpe = model.CodOpe;
                VmDefault.codEmpresa = model.codEmpresa;

                return await SalvarLayout(VmDefault);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            throw new NotImplementedException();
        }

        public async Task<IDictionary<string, string>> SalvarLayout(AparenciaSistemaVm vm)
        {
            var resultado = new Dictionary<string, string>();

            try
            {
                var Operadora = _ope.GetOperadoraById(vm.CodOpe).Result;

                string CaminhoArquivo = Path.Combine(Operadora.CAMINHOARQUIVO, Operadora.NOMOPERAFIL, vm.codEmpresa.ToString(), "ClasseCss");
                DirectoryInfo dir = new DirectoryInfo(CaminhoArquivo);
                var file = dir.GetFiles("*.*").ToList();

                var nmArquivo = "V" + (file.Count + 1) + "_Style.css";
                var CaminhoNovoArquivo = Path.Combine(CaminhoArquivo, nmArquivo);

                var arquivoCss = file.OrderByDescending(x => x.LastAccessTime).FirstOrDefault();

                var linhasArquivo = File.ReadAllLines(arquivoCss.FullName);

                StreamWriter _NovoArquivoCss = new StreamWriter(CaminhoNovoArquivo, true);
                string TextArquivo = "";

                for (int i = 0; i < linhasArquivo.Length; i++)
                {
                    //Cor do cabeção e menu
                    if (linhasArquivo[i].Contains(".Color-Header-menu"))
                    {
                        while(!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("background-color:"))
                            {
                                TextArquivo += $"background-color: #{vm.Cabecaho.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                    else if(linhasArquivo[i].Contains(".chiller-theme .sidebar-wrapper {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("background-color:"))
                            {
                                TextArquivo += $"background-color: #{vm.Cabecaho.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                    //Alteração das cores das linhas do cabecalho, menu e informações do usuario
                    else if (linhasArquivo[i].Contains(".page-header {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("border-color:"))
                            {
                                TextArquivo += $"border-color: #{vm.LinhaCabecalho.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;

                    }
                    else if (linhasArquivo[i].Contains(".nav-side-menu {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("border-top:"))
                            {
                                TextArquivo += $"border-top: 1px solid #{vm.LinhaCabecalho.Replace("#", "")};" + Environment.NewLine;
                            }
                            else if(linhasArquivo[i].Contains("border-left:"))
                            {
                                TextArquivo += $"border-left: 3px solid #{vm.LinhaCabecalho.Replace("#", "")};" + Environment.NewLine;
                            }
                            else if (linhasArquivo[i].Contains("color:"))
                            {
                                TextArquivo += $"color: #{vm.corTexto2.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;

                    }
                    else if (linhasArquivo[i].Contains(".sidebar-header"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("border-top:"))
                            {
                                TextArquivo += $"border-top: 1px solid #{vm.LinhaCabecalho.Replace("#", "")};" + Environment.NewLine;
                            }
                            else if (linhasArquivo[i].Contains("info-user"))
                            {
                                while (!linhasArquivo[i].Contains("}"))
                                {
                                    if (linhasArquivo[i].Contains("color:"))
                                    {
                                        TextArquivo += $"color: #{vm.corTexto1.Replace("#", "")};" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                                    }
                                    i++;
                                }
                                break;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                    //Alteração das cores do texto do cabeçalho e informações do usuario
                    else if (linhasArquivo[i].Contains(".text-header {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("color:"))
                            {
                                TextArquivo += $"color: #{vm.corTexto1.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                    else if (linhasArquivo[i].Contains("info-user"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("color:"))
                            {
                                TextArquivo += $"color: #{vm.corTexto2.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                    else if(linhasArquivo[i].Contains(".menu li a {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("color:"))
                            {
                                TextArquivo += $"color: #{vm.corTexto4.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                    //Alteração da cor do Terceiro Texto
                    else if (linhasArquivo[i].Contains(".mnuAlteraSenha"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("color:"))
                            {
                                TextArquivo += $"color: #{vm.corTexto3.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                    else if (linhasArquivo[i].Contains(".panel-heading {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("background-color:"))
                            {
                                TextArquivo += $"background-color: #{vm.PainelSuperior.Replace("#", "")};" + Environment.NewLine;
                            }
                            else if (linhasArquivo[i].Contains("border-color:"))
                            {
                                TextArquivo += $"border-color: #{vm.LinhaPainel.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                    else if (linhasArquivo[i].Contains(".btn-success {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("background-color:"))
                            {
                                TextArquivo += $"background-color: #{vm.btnSucesso.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                    else if (linhasArquivo[i].Contains(".btn-danger {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("background-color:"))
                            {
                                TextArquivo += $"background-color: #{vm.btnDanger.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                    else if (linhasArquivo[i].Contains(".btn-default {"))
                    {
                        while (!linhasArquivo[i].Contains("}"))
                        {
                            if (linhasArquivo[i].Contains("background-color:"))
                            {
                                TextArquivo += $"background-color: #{vm.btnDefault.Replace("#", "")};" + Environment.NewLine;
                            }
                            else
                            {
                                TextArquivo += linhasArquivo[i] + Environment.NewLine;
                            }
                            i++;
                        }
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                    else
                    {
                        TextArquivo += linhasArquivo[i] + Environment.NewLine;
                    }
                }

                _NovoArquivoCss.WriteLine(TextArquivo);

                _NovoArquivoCss.Close();
                _NovoArquivoCss.Dispose();
                _NovoArquivoCss = null;

                string caminhoimagens = Path.Combine(Operadora.CAMINHOARQUIVO, Operadora.NOMOPERAFIL, vm.codEmpresa.ToString());

                AlterImageLogo(caminhoimagens);
                AlterImageIcone(caminhoimagens);

                resultado.Add("Sucesso", "Alteração realizada com sucesso!");

            }
            catch (ArgumentException ex)
            {
                if (!resultado.ContainsKey(ex.ParamName)) resultado.Add(ex.ParamName, ex.Message);
            }
            return resultado;
        }
        private void AlterImageLogo(string CaminhoPadrao)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(CaminhoPadrao, "Imagens", "Logo"));
            var Arquivos = dir.GetFiles("*.*");

            if (Arquivos.Length > 0)
            {
                //verifica primeiro se existe a ultima logo que foi alterada
                foreach (var file in Arquivos)
                {
                    if (file.Name.Contains("New"))
                    {
                        var newName = "V" + (Arquivos.Length + 1) + "_" +file.Name.Substring(3, file.Name.Length - 3).Replace("New", "");
                        var NovoCaminho = Path.Combine(CaminhoPadrao, "Imagens", "Logo", newName);

                        File.Move(file.FullName, NovoCaminho);
                    }
                }
            }
        }
        private void AlterImageIcone(string CaminhoPadrao)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(CaminhoPadrao, "Imagens", "Icone"));
            var Arquivos = dir.GetFiles("*.*");

            if (Arquivos.Length > 0)
            {
                //verifica primeiro se existe a ultima logo que foi alterada
                foreach (var file in Arquivos)
                {
                    if (file.Name.Contains("New"))
                    {
                        var newName = "V" + (Arquivos.Length + 1) + "_" + file.Name.Substring(3, file.Name.Length - 3).Replace("New", "");
                        var NovoCaminho = Path.Combine(CaminhoPadrao, "Imagens", "Icone", newName);

                        File.Move(file.FullName, NovoCaminho);
                    }
                }
            }
        }

    }
}
