using System.Data.SqlClient;
using System.Data;
using ApiSistemaDeVentas.Models;


namespace ApiGestionVenta.Repositories
{
    public class ProductosRepository
    {

        public List<Producto> listarProductos()
        {
            List<Producto> lista = new List<Producto>();
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM producto", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                    Producto producto = new Producto();
                                    producto.Id = Convert.ToInt32(reader["Id"]);
                                    producto.Descripciones = reader["Descripciones"].ToString();
                                    producto.Costo = Convert.ToDouble(reader["Costo"].ToString());
                                    producto.PrecioVenta = Convert.ToDouble(reader["PrecioVenta"].ToString());
                                    producto.Stock = Convert.ToInt32(reader["Stock"]);
                                    producto.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                                    lista.Add(producto);
                            }
                        }
                        else
                        {
                                throw new Exception("Error al Obtener los Productos");
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

        public static List<Producto> GetProductos(int id)
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Producto WHERE IdUsuario = @IdUsuario", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        dataAdapter.SelectCommand = cmd;
                        DataTable table = new DataTable();
                        dataAdapter.Fill(table);
                        foreach (DataRow row in table.Rows)
                        {
                            Producto producto = new Producto();
                            producto.Id = Convert.ToInt32(row["Id"]);
                            producto.Descripciones = row["Descripciones"].ToString();
                            producto.Costo = Convert.ToDouble(row["Costo"]);
                            producto.PrecioVenta = Convert.ToDouble(row["PrecioVenta"]);
                            producto.Stock = Convert.ToInt32(row["Stock"]);
                            producto.IdUsuario = Convert.ToInt32(row["IdUsuario"]);

                            productos.Add(producto);
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
            return productos;
        }

        public void crearProducto(Producto producto)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Producto(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) VALUES(@Descripciones, @Costo, @PrecioVenta, @Stock, @IdUsuario);", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.AddWithValue("@Descripciones", producto.Descripciones);
                    cmd.Parameters.AddWithValue("@Costo", producto.Costo);
                    cmd.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@IdUsuario", producto.IdUsuario);
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


        public Producto? obtenerProducto(int id)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM producto WHERE id = @id", conexion))
                    {
                        conexion.Open();
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                Producto producto = obtenerProductoDesdeReader(reader);
                                return producto;
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

        public Producto? modificarProducto(int id ,Producto productoActualizado)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
                try
                {
                    Producto? producto = obtenerProducto(id);
                    if (producto == null)
                    {
                        return null;
                    }
                    List<string> camposActualiados = new List<string>();
                    if(producto.Descripciones != productoActualizado.Descripciones && !string.IsNullOrEmpty(productoActualizado.Descripciones))
                    {
                        camposActualiados.Add("Descripciones = @Descripciones");
                        producto.Descripciones = productoActualizado.Descripciones;
                    }
                    if(producto.Costo  != productoActualizado.Costo && productoActualizado.Costo > 0)
                    {
                        camposActualiados.Add("Costo = @Costo");
                        producto.Costo = productoActualizado.Costo;
                    }
                    if(producto.PrecioVenta != productoActualizado.PrecioVenta && productoActualizado.PrecioVenta > 0)
                    {
                        camposActualiados.Add("PrecioVenta = @PrecioVenta");
                        producto.PrecioVenta = productoActualizado.PrecioVenta;
                    }
                    if(producto.Stock != productoActualizado.Stock && productoActualizado.Stock > 0)
                    {
                        camposActualiados.Add("Stock = @Stock");
                        producto.Stock= productoActualizado.Stock;
                    }
                    if(camposActualiados.Count == 0)
                    {
                        throw new Exception("No new fields to update");
                    }
                    using(SqlCommand cmd = new SqlCommand($"UPDATE Producto SET {String.Join(", ", camposActualiados)} WHERE id = @id", conexion))
                    {
                        conexion.Open();
                        cmd.Parameters.AddWithValue("@Descripciones", producto.Descripciones);
                        cmd.Parameters.AddWithValue("@Costo", producto.Costo);
                        cmd.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
                        cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                        cmd.Parameters.AddWithValue("@IdUsuario", producto.IdUsuario);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        return producto;
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

        public bool eliminarProducto(int id)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Producto WHERE id = @id", conexion))
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

        private Producto obtenerProductoDesdeReader(SqlDataReader reader)
        {
            Producto producto = new Producto();
            producto.Id = Convert.ToInt32(reader["Id"]);
            producto.Descripciones = reader["Descripciones"].ToString();
            producto.Costo = Convert.ToDouble(reader["Costo"].ToString());
            producto.PrecioVenta = Convert.ToDouble(reader["PrecioVenta"].ToString());
            producto.Stock = Convert.ToInt32(reader["Stock"]);
            producto.IdUsuario= Convert.ToInt32(reader["IdUsuario"]);
            return producto;
        }

    }
}
