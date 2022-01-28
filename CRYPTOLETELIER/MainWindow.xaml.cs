using CRYPTOLETELIER.Model;
using System;
using System.Windows;
using System.Linq;

namespace CRYPTOLETELIER
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

 

        private void btnSair_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            ListaUsuarios listaUsuarios = UsuariosDAO.Get();

            string senhaHash = Encrypt.EncryptData(txtSenha.Password);

            var usuario = listaUsuarios.Usuarios.Where(u => u.Login == txtLogin.Text && u.Senha == senhaHash);

            if(usuario.Count() > 0)
            {
                MessageBox.Show("Logado com sucesso", "Entrando");

                this.Hide();

                ControleUsuario controleUsuario = new ControleUsuario();

                controleUsuario.Show();
            }
            else
            {
                MessageBox.Show("Login não encontrado, verique login e senha", "ERRO");
            }
        }
    }
}
