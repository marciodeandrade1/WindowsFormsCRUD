using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsCRUD
{
    public partial class Form1 : Form
    {
        //declaração das variáveis globais
        SqlDataReader dr;
        SqlConnection con;
        SqlCommand cmd;
        string sqlstr;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //acesso - conexão e abrir banco de dados
            con = new SqlConnection("Data Source=marciodeandrade;Initial Catalog=CRUD;Integrated Security=True;");
            cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            //chama método para carregar dados
            LoadData();
        }

        //método para carregar dados
        private void LoadData()
        {
            cmd.CommandText = "SELECT Id, Nome, Cargo, Salario, Status FROM Funcionarios ORDER BY Id";
            dr = cmd.ExecuteReader();
            ShowData();
        }

        private void ShowData()
        {
            if (dr.Read())
            {
                txtId.Text = dr[0].ToString();
                txtNome.Text = dr[1].ToString();
                txtCargo.Text = dr[2].ToString();
                txtSalario.Text = dr[3].ToString();
                chkStatus.Checked = Convert.ToBoolean(dr[4]);
            }
            else
                MessageBox.Show("Não existe dados!");
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            txtId.Text = txtNome.Text = txtCargo.Text = txtSalario.Text = " ";
            chkStatus.Checked = false;
            dr.Close();
            cmd.CommandText = "SELECT ISNULL(MAX(Id), 1000) + 1 FROM Funcionarios";
            txtId.Text = cmd.ExecuteScalar().ToString();
            btnInserir.Enabled = true;
            btnProximo.Enabled = false;
            txtNome.Focus();
        }
        private void ExecuteDML()
        {
            DialogResult dr = MessageBox.Show("Tem certeza de executar a instrução sql abaixo" + sqlstr, "informação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                cmd.CommandText = sqlstr;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                    MessageBox.Show("Instrução executada com sucesso!");
                else
                    MessageBox.Show("Falha na execução da instrução!");
            }
        }

        private void btnProximo_Click(object sender, EventArgs e)
        {
            ShowData();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    if (txtNome.Text == " ")
                        MessageBox.Show("Favor digitar um nome!", "Nome do funcionário", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (txtCargo.Text == " ")
                        MessageBox.Show("Favor digitar detalhes do cargo!", "Cargo detalhes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (txtSalario.Text == " ")
                        MessageBox.Show("Favor digitar o salário!", "Salario Detalhes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (txtNome.Text != " " && txtSalario.Text != " " && txtSalario.Text != " ")
                    {
                        sqlstr = String.Format("INSERT INTO Funcionarios(Id, Nome, Cargo, Salario, Status) VALUES({0}, '{1}', '{2}', {3}, {4})", txtId.Text, txtNome.Text, txtCargo.Text, txtSalario.Text, Convert.ToInt32(chkStatus.Checked));
                        ExecuteDML();
                        btnInserir.Enabled = false;
                        LoadData();
                        btnProximo.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERRO! " + ex.Message.ToString());
                }
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNome.Text == " ")
                    MessageBox.Show("Favor digitar o nome! ", "Nome do funcionário", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (txtCargo.Text == " ")
                    MessageBox.Show("Favor digitar o cargo! ", "Detalhes do cargo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (txtSalario.Text == " ")
                    MessageBox.Show("Favor digitar o salário !", "Salario Detalhes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (txtNome.Text != " " && txtCargo.Text != " " && txtSalario.Text != " ")
                {
                    sqlstr = String.Format("UPDATE Funcionarios SET Nome= '{0}', Cargo= '{1}', Salario={2}, Status={3} WHERE Id={4}", txtNome.Text, txtCargo.Text, txtSalario.Text, Convert.ToInt32(chkStatus.Checked), txtId.Text);
                    dr.Close();
                    ExecuteDML();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO! " + ex.Message.ToString());
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            sqlstr = "DELETE FROM Funcionarios WHERE Id=" + txtId.Text;
            dr.Close();
            ExecuteDML();
            LoadData();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            if (con.State != ConnectionState.Closed)
                con.Close();
            this.Close();
        }
    }
}

