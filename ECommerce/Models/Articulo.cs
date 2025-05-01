using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class Articulo
    {
        [Display(Name = "Id Articulo")] public int IdArticulo { get; set; }
        [Display(Name = "Id Categoria")] public int IdCategoria { get; set; }
        [Display(Name = "Nombre")] public string Nombre { get; set; }
        [Display(Name = "Precio")] public decimal Precio { get; set; }
        [Display(Name = "Stock")] public int Stock { get; set; }
        [Display(Name = "Descripcion")] public string Descripcion { get; set; }
    }
}
