using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace desafiocoaniquem.Models
{
    public class IndexViewModel
    {
        [Display(Name = "Componentes")]
        [Required(ErrorMessage = "Debe seleccionar un elemento")]
        public string ComponenteSelected { get; set; }

        public IList<SelectListItem> Componentes { get; set; }

        [Display(Name = "Productos")]
        [Required(ErrorMessage = "Debe tener producto asociado")]
        public IList<Producto> Productos { get; set; }

        public string CodigoServicio { get; set; }
    }

}