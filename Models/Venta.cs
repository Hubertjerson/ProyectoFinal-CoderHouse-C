namespace ApiSistemaDeVentas.Models
{
    public class Venta
    {
        public long? Id { get; set; }
        public string? Comentarios { get; set; }
        public int IdUsuario { get; set; }
        public List<ProductoVendido>? ProductosVendidos { get; set; }

        public Venta()
        {
            ProductosVendidos = new List<ProductoVendido>();
        }
    }
}
