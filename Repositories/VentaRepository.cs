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
        public List<Venta> GetVentas(int id)
        {
            List<Venta> lista = new List<Venta>();
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Venta WHERE IdUsuario = @IdUsuario", conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdUsuario", id);
                        conexion.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
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


        public void RegistrarVenta(Venta venta)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Venta(Comentarios, IdUsuario) VALUES(@Comentarios, @IdUsuario); SELECT @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.AddWithValue("@Comentarios", venta.Comentarios);
                    cmd.Parameters.AddWithValue("@IdUsuario", venta.IdUsuario);
                    venta.Id = long.Parse(cmd.ExecuteScalar().ToString());
                    if (venta.ProductosVendidos != null && venta.ProductosVendidos.Count > 0)
                    {
                        foreach (ProductoVendido productoVendido in venta.ProductosVendidos)
                        {
                            productoVendido.IdVenta = (int)venta.Id;
                            ProductoVendido productoVendidoRegistrado = RegistrarProducto(productoVendido);
                        }
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


        public bool eliminarVenta(int id)
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

        private ProductoVendido RegistrarProducto(ProductoVendido productoVendido)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            {
                Producto? producto = ProductosRepository.obtenerProductoSimplificadoPorId(productoVendido.IdProducto,conexion);
                if (producto != null)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO ProductoVendido(Stock, IdProducto, IdVenta) VALUES(@stock, @idProducto, @idVenta); SELECT @@Identity;", conexion))
                    {
                        cmd.Parameters.AddWithValue("@stock", productoVendido.Stock);
                        cmd.Parameters.AddWithValue("@idProducto", productoVendido.IdProducto);
                        cmd.Parameters.AddWithValue("@IdVenta", productoVendido.IdVenta);
                        productoVendido.Id = (int)long.Parse(cmd.ExecuteScalar().ToString());
                    }
                    DisminuiStock(producto, productoVendido.Stock);
                }
                else
                {
                    throw new Exception("Producto no encontrado");
                }
                return productoVendido;
            }
        }


        private void DisminuiStock(Producto producto, int cantidadVendida)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE Producto SET stock = @stock WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.AddWithValue("@stock", producto.Stock - cantidadVendida);
                    cmd.Parameters.AddWithValue("@id", producto.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
