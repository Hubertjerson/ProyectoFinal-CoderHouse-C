using System.Data.SqlClient;
using System.Data;
using ApiSistemaDeVentas.Models;
using ApiGestionVenta.Repositories;
using System;

namespace SistemaVentasApi.Repositories
{
    public class VentaRepository
    {
        public List<Venta> listarVenta()
        {
            List<Venta> lista = new List<Venta>();
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Venta", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                Venta venta = new Venta();
                                venta.Id = Convert.ToInt32(reader["ID"]);
                                venta.Comentarios = reader["Comentarios"].ToString();
                                venta.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                                lista.Add(venta);
                            }
                        }
                        else
                        {
                                throw new Exception("Error al Obtener las ventas");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
            return lista;
        }

        public Venta? obtenerVenta(int id)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
                {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Venta WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Venta venta = obtenerVentaDesdeReader(reader);
                            return venta;                            
                        }
                        else
                        {
                            return null;
                        }
                    }
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

        public void crearVenta(Venta venta)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Venta(Comentarios, IdUsuario) VALUES(@Comentarios, @IdUsuario);", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.AddWithValue("@Comentarios", venta.Comentarios);
                    cmd.Parameters.AddWithValue("@IdUsuario", venta.IdUsuario);
                    cmd.ExecuteNonQuery();
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

        public Venta? modificarVenta(int id, Venta ventaActualizada)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
                try
                {
                    Venta? venta = obtenerVenta(id);
                    if(venta == null)
                    {
                        return null;
                    }
                    List<string> camposActualizados = new List<string>();
                    if(venta.Comentarios != ventaActualizada.Comentarios && !string.IsNullOrEmpty(ventaActualizada.Comentarios))
                    {
                        camposActualizados.Add("Comentarios = @Comentarios");
                        venta.Comentarios = ventaActualizada.Comentarios;
                    }
                    if(venta.IdUsuario != ventaActualizada.IdUsuario && ventaActualizada.IdUsuario > 0)
                    {
                        camposActualizados.Add("IdUsuario = @IdUsuario");
                        venta.IdUsuario = ventaActualizada.IdUsuario;
                    }
                    if (camposActualizados.Count == 0)
                    {
                        throw new Exception("No new fields to update");
                    }
                    using (SqlCommand cmd = new SqlCommand($"UPDATE Venta SET {String.Join(", ", camposActualizados)} WHERE id = @id", conexion))
                    {
                        conexion.Open();
                        cmd.Parameters.AddWithValue("@Comentarios", venta.Comentarios);
                        cmd.Parameters.AddWithValue("@IdUsuario", venta.IdUsuario);
                        cmd.ExecuteNonQuery();
                        return venta;
                    }
                }
                catch
                {
                    throw;
                }
        }

        public bool eliminarVenta (int id)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            {
                try
                {
                    int filasAfectadas = 0;
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Venta WHERE id = @id", conexion))
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

        private Venta obtenerVentaDesdeReader(SqlDataReader reader)
        {
            Venta venta = new Venta();
            venta.Id = Convert.ToInt32(reader["ID"]);
            venta.Comentarios = reader["Comentarios"].ToString();
            venta.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
            return venta;
        }
    }
}
