using System.Data.SqlClient;
using System.Data;
using ApiSistemaDeVentas.Models;
using ApiGestionVenta.Repositories;
using System;
using ProyectoFinal.Repositories;

namespace SistemaVentasApi.Repositories
{
    public class VentaRepository
    {
        public static List<Venta> listarVenta(int id)
        {
            List<Venta> lista = new List<Venta>();
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Venta WHERE IdUsuario =@IdUsuario", conexion))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", id);
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                Venta venta = new Venta();
                                venta.Id = Convert.ToInt32(reader["Id"]);
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


        public static void InsertVenta(List<Producto> productos, int IdUsuario)
        {
            Venta venta = new Venta();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
            conexion.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Venta (Comentarios, IdUsuario)VALUES(@Comentarios,@IdUsuario)", conexion))
                {
                    cmd.Parameters.AddWithValue("@Comentarios", "");
                    cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                    cmd.ExecuteNonQuery();
                    venta.Id = GetId.Get(cmd);
                    venta.IdUsuario = IdUsuario;
                }
                foreach (Producto producto in productos)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO ProductoVendido (Stock,IdProducto, IdVenta)VALUES(@Stock,@IdProducto,@IdVenta)", conexion))
                    {
                        cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                        cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                        cmd.Parameters.AddWithValue("@IdVenta", venta.Id);
                    }
                    using (SqlCommand cmd = new SqlCommand("UPDATE Producto SET Stock = stock - @Stock WHERE id = @IdProducto", conexion))
                    {
                        cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                        cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
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
    }
}
