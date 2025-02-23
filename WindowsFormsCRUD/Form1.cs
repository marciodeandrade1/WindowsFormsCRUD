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
            con = new SqlConnection("DATA SOURCE=MARCIODEANDRADE; DATABASE=CRUD; INTEGRATED SECURITY=SSPI;");
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

    }
}

