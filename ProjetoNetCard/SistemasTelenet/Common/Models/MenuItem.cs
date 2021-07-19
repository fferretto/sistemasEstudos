using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace NetCard.Common.Models
{
    public class MenuItem
    {
        public MenuItem()
        {
            Itens = new List<MenuItem>();
        }

        public MenuItem(string _texto, string _idPermissao, string _controller, string _action, List<MenuItem> _item)
        {
            Texto = _texto;
            IdPermissao = _idPermissao;
            Controller = _controller;
            Action = _action;
            Itens = _item;
        }

        public MenuItem(string _texto, string _idPermissao, string _controller, string _action, List<MenuItem> _item, bool acessoLivre)
        {
            Texto = _texto;
            IdPermissao = _idPermissao;
            Acesso = acessoLivre;
            Controller = _controller;
            Action = _action;
            Itens = _item;
        }

        public MenuItem(Relatorio _relatorio, List<MenuItem> _item)
        {
            Relatorio = _relatorio;
            Acesso = true;
            Itens = _item;
        }

        private void ConfiguraMenuItemConsultaCargas(MenuItem menuItem)
        {
            menuItem.Itens.Insert(4, new MenuItem("Listagem de cargas solicitadas", "", "CliListCargasSolicitadas", "Index", null));
        }

        public string Texto { get; set; }
        public string IdPermissao { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool Acesso { get; set; }
        public Relatorio Relatorio { get; set; }

        public void Insert(int index, MenuItem _item)
        {
            Itens.Insert(index, _item);
        }

        public void add(MenuItem _item)
        {
            Itens.Add(_item);
        }

        public List<MenuItem> Itens { get; set; }

        public virtual object GetRouteValues()
        {
            return new { Id = IdPermissao };
        }

        public List<MenuItem> AcessoMenuCliente(ObjConn objConexao, DadosAcesso dadosAcesso, List<MenuItem> menuItem, out Permissao permissao)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "MW_PERMISSAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.String, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@ACESSO", DbType.String, dadosAcesso.Acesso),
                               new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Acesso == "parceria"? 0 : dadosAcesso.Codigo),
                               new Parametro("@CNPJ", DbType.String, dadosAcesso.Cnpj),
                               new Parametro("@LOGIN", DbType.String, dadosAcesso.Login)
                           };

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            permissao = new Permissao();

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    permissao.FALTLIMITE = dr["FALTLIMITE"].ToString();
                    permissao.FBLOQCART = dr["FBLOQCART"].ToString();
                    permissao.FCANCCART = dr["FCANCCART"].ToString();
                    permissao.FCARGA = dr["FCARGA"].ToString();
                    permissao.FCONSREDE = dr["FCONSREDE"].ToString();
                    permissao.FDESBLOQCART = dr["FDESBLOQCART"].ToString();
                    permissao.FEXTMOV = dr["FEXTMOV"].ToString();
                    permissao.FINCCART = dr["FINCCART"].ToString();
                    permissao.FLISTCART = dr["FLISTCART"].ToString();
                    permissao.FLISTINCCART = dr["FLISTINCCART"].ToString();
                    permissao.FLISTTRANSAB = dr["FLISTTRANSAB"].ToString();
                    permissao.FSEGVIACART = dr["FSEGVIACART"].ToString();
                    permissao.FTRANSFSALDO = dr["FTRANSFSALDO"].ToString();
                    permissao.FTRANSFSALDOCLI = dr["FTRANSFSALDOCLI"].ToString();

                    foreach (var subItem in menuItem.Where(item => item.Itens != null).SelectMany(item => item.Itens))
                    {
                        if (subItem.IdPermissao == "alterarsenha")
                        {
                            subItem.Acesso = true;
                            continue;
                        }
                        if (subItem.IdPermissao != "FGERALCONSULTAS")
                        {
                            subItem.Acesso = subItem.IdPermissao == string.Empty || (dr[subItem.IdPermissao].ToString() == Constantes.sim);
                            if (subItem.Itens == null) continue;
                            foreach (var subSubItem in subItem.Itens)
                            {
                                subSubItem.Acesso = subSubItem.IdPermissao == string.Empty || (dr[subSubItem.IdPermissao].ToString() == Constantes.sim);
                            }
                        }
                        else
                        {
                            subItem.Acesso = permissao.FGERALCONSULTAS == Constantes.sim;
                            if (subItem.Itens == null) continue;
                            foreach (var subSubItem in subItem.Itens)
                            {
                                subSubItem.Acesso = permissao.FGERALCONSULTAS == Constantes.sim;
                            }
                        }
                    }
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return menuItem;
        }

        public List<MenuItem> MenuClientePre(ObjConn objConexao)
        {
            var menuCliente = new List<MenuItem>();
            menuCliente.Add(new MenuItem("Tarefas", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Inclusão", "FINCCART", "CliInclusaoCartao", "InclusaoCartao", null ),
                    new MenuItem("Manutenção de Usuário", "FINCCART", "CliManuCartao", "ManuCartao", null ),
                    new MenuItem("Bloqueio em massa", "FBLOQCART", "CliBloqCart", "BloqueioEmMassa", null ),
                    new MenuItem("Desbloqueio em massa", "FDESBLOQCART", "CliDesbloqCart", "Index", null ),
                    new MenuItem("Agend. de Bloqueio/Desbloqueio", "FBLOQCART", "CliAgenBloqCard", "Index", null ),
                    new MenuItem("Carga",  "", "Home", "Index", new List<MenuItem>
                        {
                            new MenuItemCarga("Solicitar carga via arquivo", "cargaarquivo"),
                            new MenuItemCarga("Solicitar carga manual", "cargamanual")
                        }),
                    new MenuItem("Transferência de Saldo", "", "", "", new List<MenuItem>
                        {
                            new MenuItem("Para Conta Cliente", "FTRANSFSALDOCLI", "CliTransfSaldo", "TransfSaldoContaCli",  null ),
                            new MenuItem("Entre Cartões", "FTRANSFSALDO", "CliTransfSaldo", "TransfSaldo",  null )
                        }),
                    new MenuItem("Alterar senha", "alterarsenha", "CliSenhaWeb", "AlterarSenhaWeb", null )
                }));
            menuCliente.Add(new MenuItem("Consultas", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Listagem de Cartões", "FLISTCART", "CliListCart", "ListagemCartoes",  null ),
                    new MenuItem("Listagem de Inclusões", "FLISTINCCART", "CliListInc", "ListagemInclusoes", null ),
                    new MenuItem("Listagem de 2° Via de Cartões", "FGERALCONSULTAS", "CliListCartSegVia", "ListagemCartoesSegundaVia", null ),
                    new MenuItem("Listagem de cargas", "FEXTMOV", "CliListCargas", "ListagemCarga", null ),
                    new MenuItem("Listagem de Transações", "FEXTMOV", "MovimentacaoUsuario", "MovimentacaoUsuario", null ),
                    new MenuItem("Saldo de usuário", "FGERALCONSULTAS", "SaldoUsuario", "SaldoUsuario", null ),
                    new MenuItem("Consulta rede credenciada", "FCONSREDE", "ConsultaRede", "ConsultaRede", null ),
                    new MenuItem("Consulta Agendamentos", "FBLOQCART", "CliConsultaAgendamento", "CliConsultaAgendamento", null )
                }));
            menuCliente.Add(new MenuItem("Relatórios", "", "Home", "Index", new List<MenuItem>()));

            ConfiguraMenuItemConsultaCargas(menuCliente[1]);

            return menuCliente;
        }

        public List<MenuItem> MenuClientePos()
        {
            var menuCliente = new List<MenuItem>();
            menuCliente.Add(new MenuItem("Tarefas", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Inclusão", "FINCCART", "CliInclusaoCartao", "InclusaoCartao", null ),
                    new MenuItem("Manutenção de Usuário", "FINCCART", "CliManuCartao", "ManuCartao", null ),
                    new MenuItem("Bloqueio em massa", "FBLOQCART", "CliBloqCart", "BloqueioEmMassa", null ),
                    new MenuItem("Desbloqueio em massa", "FDESBLOQCART", "CliDesbloqCart", "Index", null ),
                    new MenuItem("Agend. de Bloqueio/Desbloqueio", "", "CliAgenBloqCard", "Index", null ),
                    new MenuItem("Cancelamento em massa", "FCANCCART", "CliCancelamentoEmMassa", "CancelamentoEmMassa", null ),
                    new MenuItem("Alterar senha", "alterarsenha", "CliSenhaWeb", "AlterarSenhaWeb", null )
                }));
            menuCliente.Add(new MenuItem("Consultas", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Listagem de Cartões", "FLISTCART", "CliListCart", "ListagemCartoes",  null ),
                    new MenuItem("Listagem de inclusões", "FLISTINCCART", "CliListInc", "ListagemInclusoes", null ),
                    new MenuItem("Listagem de 2° via de cartões", "FGERALCONSULTAS", "CliListCartSegVia", "ListagemCartoesSegundaVia", null ),
                    new MenuItem("Listagem dos Lotes", "FEXTMOV", "ListLote", "ListagemLotes", null ),
                    new MenuItem("Listagem de Transações", "FEXTMOV", "MovimentacaoUsuario", "MovimentacaoUsuario", null ),
                    new MenuItem("Compras em Aberto", "FLISTTRANSAB", "CliComprasAberto", "ComprasAberto", null ),
                    new MenuItem("Saldo de usuário", "FGERALCONSULTAS", "SaldoUsuario", "SaldoUsuario", null ),
                    new MenuItem("Consulta rede credenciada", "FCONSREDE", "ConsultaRede", "ConsultaRede", null ),
                    new MenuItem("Consulta Agendamentos", "FBLOQCART", "CliConsultaAgendamento", "CliConsultaAgendamento", null )
                }));
            menuCliente.Add(new MenuItem("Relatórios", "", "Home", "Index", new List<MenuItem>()));
            return menuCliente;
        }

        public List<MenuItem> MenuUsuarioPre()
        {
            var menuUsuario = new List<MenuItem>();
            menuUsuario.Add(new MenuItem("Consultas", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Listagem de Transações", "", "MovimentacaoUsuario", "MovimentacaoUsuario", null, true),
                    new MenuItem("Consulta de saldo", "SaldoUsuario", "SaldoUsuario", "SaldoUsuario", null, true),
                    new MenuItem("Consulta de carga programada", "CargaProgramada", "UsuCargaProgramada", "CargaProgramada", null, true),
                    new MenuItem("Consulta rede credenciada", "ConsultaRede", "ConsultaRede", "ConsultaRede", null, true)
                }, true));
            menuUsuario.Add(new MenuItem("Tarefas", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Atualizar dados cadastrais", "", "AlteracaoUsuario", "AlterarDados", null, true)
                }, true));
            menuUsuario.Add(new MenuItem("Segurança", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Alterar senha da web", "", "UsuSenhaWeb", "AlterarSenhaWeb", null, true),
                    new MenuItem("Alterar senha do cartão", "", "UsuSenhaCartao", "AlterarSenhaCartao", null, true),
                    new MenuItem("Alterar Pergunta Secreta", "", "UsuSenhaWeb", "RecuperarSenha", null, true)
                }, true));
            return menuUsuario;
        }

        public List<MenuItem> MenuUsuarioPos()
        {
            var menuUsuario = new List<MenuItem>();
            menuUsuario.Add(new MenuItem("Consultas", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Lançamentos para desconto", "", "ListLote", "ListagemLotes", null, true),
                    new MenuItem("Listagem de Transações", "", "MovimentacaoUsuario", "MovimentacaoUsuario", null, true),
                    new MenuItem("Consulta saldo e limites", "SaldoUsuario", "SaldoUsuario", "SaldoUsuario", null, true),
                    new MenuItem("Consulta rede credenciada", "ConsultaRede", "ConsultaRede", "ConsultaRede", null, true)
                }, true));
            menuUsuario.Add(new MenuItem("Tarefas", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Atualizar dados cadastrais", "", "AlteracaoUsuario", "AlterarDados", null, true)
                }, true));
            menuUsuario.Add(new MenuItem("Segurança", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Alterar senha da web", "", "UsuSenhaWeb", "AlterarSenhaWeb", null, true),
                    new MenuItem("Alterar senha do cartão", "", "UsuSenhaCartao", "AlterarSenhaCartao", null, true),
                    new MenuItem("Alterar Pergunta Secreta", "", "UsuSenhaWeb", "RecuperarSenha", null, true)
                }, true));
            return menuUsuario;
        }

        public List<MenuItem> MenuUsuarioPontuacao()
        {
            var menuCredenciado = new List<MenuItem>();
            menuCredenciado.Add(new MenuItem("Fidelidade", "", "UsuTermoAdesao", "Pontuacao", new List<MenuItem>
                {
                    new MenuItem("Converter Pontuação", "", "UsuConvPontos", "ConvPontuacao", null, true),
                    new MenuItem("Extrato Pontuação", "", "ExtratoPontuacao", "ExtratoPontuacao", null, true),
                    new MenuItem("Cancelar Adesão", "", "UsuCancAdesao", "CancelarAdesao", null, true)
                }, true));
            return menuCredenciado;
        }

        public List<MenuItem> MenuCredenciadoPos()
        {
            var menuCredenciado = new List<MenuItem>();
            menuCredenciado.Add(new MenuItem("Consultas", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Movimentação diária", "MovimentacaoDiaria", "CreMovimentacaoDiaria", "MovimentacaoDiaria", null, true),
                    new MenuItem("Movimento de lotes fechados", "MovimentoLoteFechado", "CreMovimentoLoteFechado", "MovimentoLoteFechado",  null, true),
                    new MenuItem("Movimento de lotes abertos", "MovimentoLoteAberto", "CreMovimentoLoteAberto", "MovimentoLoteAberto", null, true),
                    new MenuItem("Consulta pendências", "Pendencias", "CrePendencias", "Pendencias", null, true)
                }, true));
            menuCredenciado.Add(new MenuItem("Alterar senha", "alterarsenha", "CreSenhaWeb", "AlterarSenhaWeb", null, true));
            menuCredenciado.Add(new MenuItem("Relatórios", "", "Home", "Index", new List<MenuItem>(), true));
            return menuCredenciado;
        }

        public List<MenuItem> MenuCredenciadoPre()
        {
            var menuCredenciado = new List<MenuItem>();
            menuCredenciado.Add(new MenuItem("Consultas", "", "Home", "Index", new List<MenuItem>
                {
                    new MenuItem("Movimentação diária", "MovimentacaoDiaria", "CreMovimentacaoDiaria", "MovimentacaoDiaria", null, true),
                    new MenuItem("Movimento de lotes fechados", "MovimentoLoteFechado", "CreMovimentoLoteFechado", "MovimentoLoteFechado",  null, true),
                    new MenuItem("Movimento de lotes abertos", "MovimentoLoteAberto", "CreMovimentoLoteAberto", "MovimentoLoteAberto",  null, true),
                    new MenuItem("Consulta pendências", "Pendencias", "CrePendencias", "Pendencias",  null, true)
                }, true));
            menuCredenciado.Add(new MenuItem("Alterar senha", "alterarsenha", "CreSenhaWeb", "AlterarSenhaWeb", null, true));
            menuCredenciado.Add(new MenuItem("Relatórios", "", "Home", "Index", new List<MenuItem>(), true));
            //{
            //    new MenuItem("Movimento diário de Vendas", "MovimentoLoteAberto", "CreMovimentoLoteAberto", "MovimentoLoteAberto",  null ),
            //}));
            return menuCredenciado;
        }

        public List<MenuItem> MenuCredenciadoPontuacao()
        {
            var menuCredenciado = new List<MenuItem>();
            menuCredenciado.Add(new MenuItem("Fidelidade", "", "CreFidelidade", "ConsultaPontuacao", new List<MenuItem>
                {
                    new MenuItem("Consulta Pontuação", "", "CreFidelidade", "ConsultaPontuacao", null, true),
                    new MenuItem("Extrato Pontuação", "", "ExtratoPontuacao", "ExtratoPontuacao", null, true),
                    new MenuItem("Pontuar Cartão", "", "CrePontuarCartao", "PontuarCartao",  null, true)
                }, true));
            return menuCredenciado;
        }

        public bool HabilitaTrasnfSalCli(ObjConn objConexao)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'HABTRANSFSALCLI'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public string AcessoBloqParc(ObjConn objConexao)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'ACESSOBLOQPARC'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }

        public List<int> AcessoBloqParcCliente(ObjConn objConexao, string acessoBloqParc)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            var sql = "SELECT CODCLI FROM CLIENTE_POS WITH (NOLOCK) WHERE STA='01' AND CODPARCERIA IN (" + acessoBloqParc + ")";
            var cmd = db.GetSqlStringCommand(sql.ToString());
            cmd.CommandTimeout = 60;
            IDataReader dr = null;
            var listaResult = new List<int>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var TESTE = Convert.ToInt32(dr["CODCLI"]);
                    listaResult.Add(TESTE);
                }
                dr.Close();
            }
            catch (Exception)
            {
                if (dr != null)
                    dr.Close();
            }
            return listaResult;

        }

        public decimal SaldoConta(ObjConn objConexao, DadosAcesso dadosAcesso)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "SELECT SALDOCONTA FROM VCLIENTEVA WITH (NOLOCK) WHERE CODCLI = @CODCLI ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            return Convert.ToDecimal(db.ExecuteScalar(cmd));
        }

        public bool HabilitaTrocaCpfFic(ObjConn objConexao, DadosAcesso dadosAcesso)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            var vTabela = dadosAcesso.Sistema.cartaoPJVA == 0 ? "VCLIENTE" : dadosAcesso.Sistema.cartaoPJVA == 1 ? "VCLIENTEVA" : "";
            var sql = "SELECT HABTROCACPFTEMP FROM " + vTabela + " WITH (NOLOCK) WHERE CODCLI = @CODCLI ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool HabilitaCartaoTemp(ObjConn objConexao, DadosAcesso dadosAcesso)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            var vTabela = dadosAcesso.Sistema.cartaoPJVA == 0 ? "VCLIENTE" : dadosAcesso.Sistema.cartaoPJVA == 1 ? "VCLIENTEVA" : "";
            var sql = "SELECT HABCPFTEMP FROM " + vTabela + " WITH (NOLOCK) WHERE CODCLI = @CODCLI ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool HabilitaCrediHabita(ObjConn objConexao, int tipoProd)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));            
            var sql = "SELECT PENDPAPEL FROM TIPOPRODUTO WITH (NOLOCK) WHERE TIPOPROD = @TIPOPROD ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "TIPOPROD", DbType.Int32, tipoProd);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool HabilitaSuspender(ObjConn objConexao)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'HABSUSPMW'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool ContaDigitalHabilitada(ObjConn objConexao, int tipoProd)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            var sql = "SELECT CONTADIG FROM TIPOPRODUTO WITH (NOLOCK) WHERE TIPOPROD = @TIPOPROD ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "TIPOPROD", DbType.Int32, tipoProd);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool HabilitaCashback(ObjConn objConexao)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'HABCASHBACK'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool HabilitaBloqueioUsuario(ObjConn objConexao, int sistema)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            var tabela = sistema == 0 ? "PARAM" : "PARAMVA";
            var sql = "SELECT VAL FROM " + tabela + " WITH (NOLOCK) WHERE ID0 = 'BLOQCARTUSU'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }
    }
}