using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Ajuda.Controllers
{
    [Area("Ajuda")]
    [Authorize]
    public class TreinamentoController : ClientSessionControllerBase
    {
        private readonly IPagNetUser _user;
        private readonly ICadastrosApp _cadastro;
        private readonly IDiversosApp _diversos;

        public TreinamentoController(ICadastrosApp cadastro,
                                     IPagNetUser user,
                                     IDiversosApp diversos)
        {
            _cadastro = cadastro;
            _user = user;
            _diversos = diversos;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult VisualizarVideo(string id)
        {
            TreinamentoVm modal = new TreinamentoVm();
            modal.TipoTreinamento = id;

            switch (id)
            {
                case "AlterarDataPGTO":
                    modal.Titulo = "Alterar Data de Pagamento";
                    modal.TextoCabecalho = "Este vídeo irá ensinar como alterar a data de pagamento/Vencimento de um título";
                    modal.NomedoVideo = "Alterar data PGTO";
                    break;
                case "GerarArquivoRemessa":
                    modal.Titulo = "Gerar Arquivo de Remessa para Pagamento";
                    modal.TextoCabecalho = "Este vídeo irá ensinar como gerar o arquivo de remesa de um título";
                    modal.NomedoVideo = "Gerando arquivo de remessa para pagamento";
                    break;
                case "CadastroContaCorrente":
                    modal.Titulo = "Cadastrar/Configurar uma Conta Corrente";
                    modal.TextoCabecalho = "Este vídeo irá ensinar como cadastrar/configurar uma conta corrente";
                    modal.NomedoVideo = "Configuração de conta corrente";
                    break;
                case "ProcessaRetorno":
                    modal.Titulo = "Processa Arquivo de Retorno";
                    modal.TextoCabecalho = "Este vídeo irá ensinar processar o arquivo de retorno do banco";
                    modal.NomedoVideo = "Processar arquivo de retorno do banco";
                    break;
                case "CadastroFavorecido":
                    modal.Titulo = "Cadatrar Favorecido";
                    modal.TextoCabecalho = "Este vídeo irá ensinar como cadastrar um novo favorecido";
                    modal.NomedoVideo = "Cadastro de Favorecido";
                    break;
                case "CadastroUsuario":
                    modal.Titulo = "Cadastro de Usuário";
                    modal.TextoCabecalho = "Este vídeo irá ensinar como cadastrar um usuário para utilizar o sistema Pagnet";
                    modal.NomedoVideo = "Cadastro de Usuario";
                    break;
            }


            return View(modal);
        }
        public IActionResult RetornaVideoTreinamento(string nomeVideo, string extensao)
        {
            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope);
            var CaminhoArquivo = Path.Combine(CaminhoPadrao, "TreinamentoSistema", "Videos");
            var path = Path.Combine(CaminhoArquivo, nomeVideo + "." + extensao);

            //inclui esta regra apenas para corrigir a extensão no retorno, pois não existe video/ogv e sim video/ogg
            if (extensao == "ogv") extensao = "ogg";

            return File(System.IO.File.ReadAllBytes(path), "video/" + extensao);
        }

    }
}