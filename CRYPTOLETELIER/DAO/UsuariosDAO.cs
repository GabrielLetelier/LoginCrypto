using CRYPTOLETELIER.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CRYPTOLETELIER
{
    public static class UsuariosDAO
    {
        private static string PathJsonUsuarios
        {
            get => Directory.GetCurrentDirectory() + @"\Lista\ListaUsuarios.json"; 
        }

        public static void Insert(Usuario usuario)
        {
            // Gerando na tabela
            ListaUsuarios listaUsuarios = Get();

            int incrementoID = listaUsuarios.Usuarios.Last().ID + 1;

            usuario.ID = incrementoID;
            usuario.Senha = Encrypt.EncryptData(usuario.Senha);

            listaUsuarios.Usuarios.Add(usuario);

            using (StreamWriter file = File.CreateText(PathJsonUsuarios))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, listaUsuarios);
            }
        }

        public static ListaUsuarios Get()
        {
            if (File.Exists(PathJsonUsuarios))
            {
                // Read json file
                using (StreamReader file = File.OpenText(PathJsonUsuarios)) 
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return (ListaUsuarios)serializer.Deserialize(file, typeof(ListaUsuarios));
                }
            }

            return null;
        }

        public static void Delete(int id)
        {
            ListaUsuarios listaUsuarios = UsuariosDAO.Get();

            Usuario usuarioJson = listaUsuarios.Usuarios.First(u => u.ID == id);

            listaUsuarios.Usuarios.Remove(usuarioJson);

            using (StreamWriter file = File.CreateText(PathJsonUsuarios))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, listaUsuarios);
            }
        }

        public static void Update(Usuario usuario)
        {
            ListaUsuarios listaUsuarios = UsuariosDAO.Get();

            Usuario usuarioJson = listaUsuarios.Usuarios.First(u => u.ID == usuario.ID);

            usuarioJson.Login = usuario.Login;

            if(usuario.Senha != "***")
                usuarioJson.Senha = Encrypt.EncryptData(usuario.Senha);

            using (StreamWriter file = File.CreateText(PathJsonUsuarios))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, listaUsuarios);
            }
        }
    }
}