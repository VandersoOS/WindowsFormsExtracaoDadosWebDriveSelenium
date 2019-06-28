using ProjectGetCotacao.ConexaoBD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectGetCotacao.ConexaoBD.Repository
{
    public class Repository
    {
        public static string ErrorMessage { get; private set; }

        public static void GravarDb(Cotacao cotacao)
        {
            ErrorMessage = string.Empty;

            using (var conexao = new CotacaoBDEntities())
            {
                try
                {
                    conexao.Cotacao.Add(cotacao);
                    conexao.SaveChanges();
                }
                catch (Exception)
                {
                    ErrorMessage = "Erro ao gravar no banco de dados!!";
                }
            }
           
        }

        public static List<Cotacao> ListarCotacoes()
        {
            List<Cotacao> result = new List<Cotacao>();

            ErrorMessage = string.Empty;

            using (var conexao = new CotacaoBDEntities())
            {
                try
                {
                    result = conexao.Cotacao.OrderByDescending(x => x.DataInclusao).ToList();
                }
                catch (Exception)
                {
                    ErrorMessage = "Erro ao carregar a lista!!";
                }
            }
            return result;
        }

        public static void ExcluirCotacao(int cotacaoID)
        {
            using (var conexao = new CotacaoBDEntities())
            {
                ErrorMessage = string.Empty;

                try
                {
                    var cotacao = conexao.Cotacao.Single(x => x.CotacaoID == cotacaoID);

                    //Mandamos remover o registro
                    conexao.Cotacao.Remove(cotacao);

                    conexao.SaveChanges();
                }
                catch (Exception)
                {
                    ErrorMessage = "Erro ao remover cotação!!";
                }
            }
        }
    }
}
