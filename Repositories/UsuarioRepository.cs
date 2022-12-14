using System.Data.SqlClient;
using System.Data;
using ApiSistemaDeVentas.Models;

namespace ApiGestionVenta.Repositories
{
    public class UsuarioRepository
    {
        public List<Usuario> listarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Usuario usuario = new Usuario();
                                usuario.Id = Convert.ToInt32(reader["ID"]);
                                usuario.Nombre = reader["Nombre"].ToString();
                                usuario.Apellido = reader["Apellido"].ToString();
                                usuario.NombreUsuario = reader["NombreUsuario"].ToString();
                                usuario.Mail = reader["Mail"].ToString();
                                lista.Add(usuario);
                            }
                        }
                        else
                        {
                            throw new Exception("Error al obtener los Usuarios registrados");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
            return lista;
        }
        

        public Usuario? obtenerUsuarioPorUserName(string nombre)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE NombreUsuario = @nombre", conexion))
                    {
                        conexion.Open();
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                Usuario usuario = obtenerUsuarioDesdeReader(reader);
                                return usuario;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    conexion.Close();
                }
        }


        public void crearUsuario(Usuario usuario)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Usuario(Nombre, Apellido, NombreUsuario,Contraseña, Mail) VALUES(@Nombre, @Apellido, @NombreUsuario,@Contrasenia, @Mail);", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("Contrasenia", usuario.Contrasenia);
                    cmd.Parameters.AddWithValue("Mail", usuario.Mail);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                    throw;
            }
            finally 
            { 
                conexion.Close(); 
            }
        }

        public Usuario? modificarUsuario(int id , Usuario usuarioActualizar)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
                try
                {
                    Usuario? usuario = obtenerUsuario(id);
                    if(usuario == null)
                    {
                        return null;
                    }
                    List<string> camposActualizado = new List<string>();
                    if(usuario.Nombre != usuarioActualizar.Nombre && !string.IsNullOrEmpty(usuarioActualizar.Nombre))
                    {
                        camposActualizado.Add("Nombre = @Nombre");
                        usuario.Nombre = usuarioActualizar.Nombre;
                    }
                    if(usuario.Apellido != usuarioActualizar.Apellido && !string.IsNullOrEmpty(usuarioActualizar.Apellido))
                    {
                        camposActualizado.Add("Apellido = @Apellido");
                        usuario.Apellido = usuarioActualizar.Apellido;
                    }
                    if(usuario.NombreUsuario != usuarioActualizar.NombreUsuario && !string.IsNullOrEmpty(usuarioActualizar.NombreUsuario))
                    {
                        camposActualizado.Add("NombreUsuario = @NombreUsuario");
                        usuario.NombreUsuario = usuarioActualizar.NombreUsuario;
                    }
                    if(usuario.Contrasenia != usuarioActualizar.Contrasenia && !string.IsNullOrEmpty(usuarioActualizar.Contrasenia))
                    {
                        camposActualizado.Add("Contraseña = @Contrasenia");
                        usuario.Contrasenia = usuarioActualizar.Contrasenia;
                    }
                    if(usuario.Mail != usuarioActualizar.Mail && !string.IsNullOrEmpty(usuarioActualizar.Mail))
                    {
                        camposActualizado.Add("Mail = @Mail");
                        usuario.Mail = usuarioActualizar.Mail;
                    }
                    if(camposActualizado.Count == 0)
                    {
                        throw new Exception("No new fields to update");
                    }
                    using (SqlCommand cmd = new SqlCommand($"UPDATE Usuario SET {String.Join(", ", camposActualizado)} WHERE id = @id", conexion))
                    {
                        conexion.Open();
                        cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                        cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                        cmd.Parameters.AddWithValue("@Contrasenia", usuario.Contrasenia);
                        cmd.Parameters.AddWithValue("@Mail", usuario.Mail);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        return usuario;
                    }

                }
                catch
                {
                    throw;
                }
                finally
                {
                    conexion.Close();
                }
        }

        public Usuario? obtenerUsuario(int id)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE id = @id", conexion))
                    {
                        conexion.Open();
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                Usuario usuario = obtenerUsuarioDesdeReader(reader);
                                return usuario;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    conexion.Close();
                }
        }

        public bool eliminarUsuario(int id)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            {
                try
                {
                    int filasAfectadas = 0;
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Usuario WHERE id = @id", conexion))
                    {
                        conexion.Open();
                        cmd.Parameters.AddWithValue("@id", id);
                        filasAfectadas = cmd.ExecuteNonQuery();
                    }
                    conexion.Close();
                    return filasAfectadas > 0;
                }
                catch
                {
                    throw;
                }
            }
        }

        private Usuario obtenerUsuarioDesdeReader(SqlDataReader reader)
        {
            Usuario usuario = new Usuario();
            usuario.Id = Convert.ToInt32(reader["Id"]);
            usuario.Nombre = reader["Nombre"].ToString();
            usuario.Apellido = reader["Apellido"].ToString();
            usuario.NombreUsuario = reader["NombreUsuario"].ToString();
            usuario.Mail = reader["Mail"].ToString();
            return usuario;
        }
    }
}
