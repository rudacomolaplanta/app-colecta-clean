using System.ComponentModel.DataAnnotations;

namespace desafiocoaniquem.Models
{
    public class Producto
    {
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Precio")]
        public string Precio { get; set; }

        [Display(Name = "Cantidad")]
        public string Cantidad { get; set; }

        [Display(Name = "Glosa")]
        public string Glosa { get; set; }
    }

}