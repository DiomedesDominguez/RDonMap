
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace src.Data.Entities
{
    /// <summary>
    /// Clase que representa la entidad de país.
    /// </summary>
    public class mCountry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        //Pendiente de agregar las referencias a las coordenadas que delimitan el poligono del país.
    }
}