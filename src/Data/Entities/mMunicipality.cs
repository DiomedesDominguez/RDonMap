using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace DNMOFT.CountryOnMap.Core.Data.Entities;

public class mMunicipality
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(Order = 0)]
    public long Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;

    [Column(TypeName = "geography")]
    public MultiPolygon? Coordenadas { get; set; }

    [ForeignKey("Province")]
    public long? ProvinceId { get; set; }
    public virtual mProvince? Province { get; set; }
}
