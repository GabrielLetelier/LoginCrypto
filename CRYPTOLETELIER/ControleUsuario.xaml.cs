using CRYPTOLETELIER.Model;
using CRYPTOLETELIER.Utilitarios;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace CRYPTOLETELIER
{
    public partial class ControleUsuario : Window
    {
        public ControleUsuario()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ExibeDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
                this.Close();
            }
        }
        private void btnIncluir_Click(object sender, RoutedEventArgs e)
        {
            Usuario usuario = new Usuario();
            usuario.Login = txtUsuario.Text;
            usuario.Senha = txtSenha.Text;

            if (ValidaDados())
            {
                try
                {
                    UsuariosDAO.Insert(usuario);
                    ExibeDados();
                    btnLimpar_Click(null, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" Erro : " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            txtUsuario.Text = "";
            txtSenha.Text = "";
            txtUsuario.Focus();
        }

        public void ExibeDados()
        {
            List<Usuario> listaUsuarios = UsuariosDAO.Get().Usuarios;

            foreach(var usuario in listaUsuarios)
            {
                usuario.Senha = Decrypt.DecryptData(usuario.Senha);
            }

            DataTable dtb = CollectionHelper.ConvertTo(listaUsuarios);
            
            lstvUsuarios.DataContext = dtb.DefaultView;
        }

        private void btnDeletar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirma exclusão deste registro ?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (lstvUsuarios.SelectedItems.Count > 0)
                {
                    DataRowView drv = (DataRowView)lstvUsuarios.SelectedItem;
                    int id = Convert.ToInt32(drv.Row[0].ToString());
                    try
                    {
                        UsuariosDAO.Delete(id);
                        ExibeDados();
                        btnLimpar_Click(null, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(" Erro : " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;

            if (ValidaDados())
            {
                if (lstvUsuarios.SelectedItems.Count > 0)
                {
                    DataRowView drv = (DataRowView)lstvUsuarios.SelectedItem;
                    var usuarioParaAtualizar = new Usuario();
                    usuarioParaAtualizar.ID = Convert.ToInt32(drv.Row[0]);
                    usuarioParaAtualizar.Login = usuario;
                    usuarioParaAtualizar.Senha = senha;
                    try
                    {
                        UsuariosDAO.Update(usuarioParaAtualizar);
                        ExibeDados();
                        MessageBox.Show("Atualizado com sucesso.", "Atualização", MessageBoxButton.OK, MessageBoxImage.Information);
                        btnLimpar_Click(null,e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(" Erro : " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private bool ValidaDados()
        {
            bool retorno;
            if (txtSenha.Text == string.Empty || txtSenha.Text =="***" || txtSenha.Text.Length < 6)
            {
                MessageBox.Show("Informe a senha do Usuário com no mínimo seis caracteres.", "Senha", MessageBoxButton.YesNo, MessageBoxImage.Error);
                txtSenha.Focus();
                return false;
            }
            else
            {
                retorno = true;
            }
            if (txtUsuario.Text == string.Empty || txtUsuario.Text.Length < 6)
            {
                MessageBox.Show("Informe o nome do Usuário com no mínimo seis caracteres.", "Senha", MessageBoxButton.YesNo, MessageBoxImage.Error);
                txtSenha.Focus();
                return false;
            }
            else
            {
                retorno = true;
            }
            return retorno;
        }

        private void txtUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void lstvUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView drv = (DataRowView)lstvUsuarios.SelectedItem;

            if (drv != null)
            {
                var sel = drv.Row.ItemArray;

                string login = sel[1].ToString();
                string senha = sel[2].ToString();

                txtUsuario.Text = login;
                txtSenha.Text = senha;
            }
        }
    }
}