using ApiGestionVenta.Repositories;
using ApiSistemaDeVentas.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemaVentasApi.Repositories
{
    public class ProductoVentaRepository
    {
        public List<ProductoVendido> listarProductoVendido()
        {
            List<ProductoVendido> lista = new List<ProductoVendido>();
            using(SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
                {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ProductoVendido", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();
                                productoVendido.Id = Convert.ToInt32(reader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(reader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt32(reader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt32(reader["IdVenta"]);
                                lista.Add(productoVendido);
                            }
                        }
                        else
                        {
                            throw new Exception("Error al obtener Producto Vendido");
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

        public ProductoVendido? obtenerProductoVendido(int id)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ProductoVendido WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            ProductoVendido productoVendido = obtenerProductoVendidoDesdeReader(reader);
                            return productoVendido;
                        }
                        else
                        {
                            return null;
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

        public void crearProductoVendido(ProductoVendido productoVendido)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO ProductoVendido(Stock, IdProducto, IdVenta) VALUES(@Stock, @IdProducto, @IdVenta);", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.AddWithValue("@Stock", productoVendido.Stock);
                    cmd.Parameters.AddWithValue("@IdProducto", productoVendido.IdProducto);
                    cmd.Parameters.AddWithValue("@IdVenta", productoVendido.IdVenta);
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

        public ProductoVendido? modificarProductoVendido(int id, ProductoVendido productoActualizado)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
                try
                {
                    ProductoVendido? productoVendido = obtenerProductoVendido(id);
                    if (productoVendido == null)
                    {
                        return null;
                    }
                    List<string> campoActualizado = new List<string>();
                    if (productoVendido.Stock != productoActualizado.Stock && productoActualizado.Stock > 0)
                    {
                        campoActualizado.Add("Stock = @Stock");
                        productoVendido.Stock = productoActualizado.Stock;
                    }
                    if (productoVendido.IdProducto != productoActualizado.IdProducto && productoActualizado.IdProducto > 0)
                    {
                        campoActualizado.Add("IdProducto = @IdProducto");
                        productoVendido.IdProducto = productoActualizado.IdProducto;
                    }
                    if(productoVendido.IdVenta != productoActualizado.IdVenta && productoActualizado.IdVenta > 0)
                    {
                        campoActualizado.Add("IdVenta = @IdVenta");
                        productoVendido.IdVenta = productoActualizado.IdVenta;
                    }
                    if (campoActualizado.Count == 0)
                    {
                        throw new Exception("No new fields to update");
                    }
                    using (SqlCommand cmd = new SqlCommand($"UPDATE ProductoVendido SET {String.Join(", ", campoActualizado)} WHERE id = @id", conexion))
                    {
                        conexion.Open();
                        cmd.Parameters.AddWithValue("@Stock", productoVendido.Stock);
                        cmd.Parameters.AddWithValue("@IdProducto", productoVendido.IdProducto);
                        cmd.Parameters.AddWithValue("@IdVenta", productoVendido.IdVenta);
                        cmd.ExecuteNonQuery();
                        return productoVendido;
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

        public bool eliminarProductoVendido(int id)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM ProductoVendido WHERE id = @id", conexion))
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

        private ProductoVendido obtenerProductoVendidoDesdeReader(SqlDataReader reader)
        {
            ProductoVendido productoVendido = new ProductoVendido();
            productoVendido.Id = Convert.ToInt32(reader["Id"]);
            productoVendido.Stock = Convert.ToInt32(reader["Stock"]);
            productoVendido.IdProducto = Convert.ToInt32(reader["IdProducto"]);
            productoVendido.IdVenta = Convert.ToInt32(reader["IdVenta"]);
            return productoVendido;
        }
    }
}
