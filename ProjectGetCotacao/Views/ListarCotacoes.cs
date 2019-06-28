using System;
using System.Windows.Forms;
using ProjectGetCotacao.ConexaoBD;
using ProjectGetCotacao.ConexaoBD.Repository;

namespace TesteCotacaoDolar.Views
{
    public partial class ListarCotacoes : Form
    {
        private Cotacao _linhaSelecionada;
        public ListarCotacoes()
        {
            InitializeComponent();
        }

        private void frmListar_Load(object sender, EventArgs e)
        {
            dgvCotacoes.AutoGenerateColumns = false;

            dgvCotacoes.Focus();
            CarregarLista();
        }

        private void CarregarLista()
        {
            var result = Repository.ListarCotacoes();

            if (string.IsNullOrEmpty(Repository.ErrorMessage))
                dgvCotacoes.DataSource = result;
            else
            {
                MessageBox.Show(Repository.ErrorMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            //
            btnExcluir.Enabled = result.Count > 0;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Deseja excluir cotação selecionada?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No) return;

            RemoverCotacao();
        }

        private void RemoverCotacao()
        {
            Repository.ExcluirCotacao(_linhaSelecionada.CotacaoID);

            if (string.IsNullOrEmpty(Repository.ErrorMessage))
            {
                MessageBox.Show("Cotação excluida com sucesso!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarLista();
            }
            else
                MessageBox.Show(Repository.ErrorMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvCotacoes_SelectionChanged(object sender, EventArgs e)
        {
            //Captura a linha selecionada do Grid
            _linhaSelecionada = (Cotacao)dgvCotacoes.CurrentRow.DataBoundItem;
        }
    }
}
