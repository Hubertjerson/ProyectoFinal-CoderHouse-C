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
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM ProductoVendido", conexion))
                    {
                        conexion.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
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
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM ProductoVendido WHERE Id = @Id", conexion))
                    {
                        conexion.Open();
                        cmd.Parameters.AddWithValue("@Id", id);
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

        public List<ProductoVendido> GetProductoVendidos(int id)
        {
            List<ProductoVendido> listProductosVendidos = new List<ProductoVendido>();
            List<Producto> listProductos = ProductosRepository.GetProductos(id);
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
                try
                {
                    foreach (Producto producto in listProductos)
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM ProductoVendido WHERE IdProducto = @IdProducto", conexion))
                        {
                            conexion.Open();
                            cmd.Parameters.AddWithValue("@IdProducto", id);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        ProductoVendido productoVendido = new ProductoVendido();
                                        productoVendido.Id = Convert.ToInt32(reader["Id"]);
                                        productoVendido.Stock = Convert.ToInt32(reader["Stock"]);
                                        productoVendido.IdProducto = Convert.ToInt32(reader["IdProducto"]);
                                        productoVendido.IdVenta = Convert.ToInt32(reader["IdVenta"]);
                                        listProductosVendidos.Add(productoVendido);
                                    }
                                }
                                else
                                {
                                    throw new Exception("Error al Obtener los Productos Vendidos");
                                }
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
            return listProductosVendidos;
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
