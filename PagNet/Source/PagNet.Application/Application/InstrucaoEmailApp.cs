using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PagNet.Application.Application
{
    public class InstrucaoEmailApp : IInstrucaoEmailApp
    {
        private readonly IPagNet_InstrucaoEmailService _email;
        private readonly IOperadoraService _ope;
        private readonly IPagNet_CadEmpresaService _empresa;

        public InstrucaoEmailApp(IPagNet_InstrucaoEmailService email,
                     IOperadoraService ope,
                     IPagNet_CadEmpresaService empresa)
        {
            _email = email;
            _ope = ope;
            _empresa = empresa;
        }

        public async Task<IDictionary<string, string>> SalvaInstrucao(InstrucaoEmailVm email)
        {
            var resultado = new Dictionary<string, string>();

            try
            {

                PAGNET_INSTRUCAOEMAIL instrucao;
                if (email.CODINSTRUCAOEMAIL == 0)
                {

                    instrucao = new PAGNET_INSTRUCAOEMAIL();
                    instrucao = InstrucaoEmailVm.ToEntity(email);

                    var dadosRetorno = await _email.IncluirEmail(instrucao);
                }
                else
                {
                    instrucao = _email.GetInstrucaoById(email.CODINSTRUCAOEMAIL).Result;

                    instrucao.ASSUNTO = email.ASSUNTO;
                    instrucao.MENSAGEM = email.MENSAGEM;
                    instrucao.CODEMPRESA = Convert.ToInt32(email.codEmpresa);

                    _email.AtualizaEmail(instrucao);
                }

                resultado.Add("Sucesso", "Email Cadastrado com Sucesso!");

            }
            catch (Exception ex)
            {
                string[] lines = Regex.Split(ex.Message, "\r\n");
                //resultado.Add(ex.ParamName, lines[0]);
            }

            return resultado;

        }

        public InstrucaoEmailVm GetInstrucaoEmailById(int? id, int codEmpresa)
        {
            try
            {
                InstrucaoEmailVm DadosEmail = new InstrucaoEmailVm();

                var email = _email.ConsultaInstrucaoById(codEmpresa).Result;
                var empresa = _empresa.ConsultaEmpresaById(codEmpresa).Result;

                if (email.Count > 0)
                {
                    DadosEmail.ASSUNTO = email[0].ASSUNTO;
                    DadosEmail.MENSAGEM = email[0].MENSAGEM;
                    DadosEmail.codEmpresa = empresa.CODEMPRESA.ToString();
                    DadosEmail.nmEmpresa = empresa.NMFANTASIA;
                    DadosEmail.CODINSTRUCAOEMAIL = email[0].CODINSTRUCAOEMAIL;
                }
                else
                {
                    DadosEmail.codEmpresa = empresa.CODEMPRESA.ToString();
                    DadosEmail.nmEmpresa = empresa.NMFANTASIA;
                }
                return DadosEmail;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
