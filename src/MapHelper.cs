using System.Text.Json;
using DNMOFT.CountryOnMap.Core.Data.Context;
using DNMOFT.CountryOnMap.Core.Data.Entities;
using DNMOFT.CountryOnMap.Core.GADM;
using NetTopologySuite.Geometries;

namespace DNMOFT.CountryOnMap.Core;

/// <summary>
/// Helper class for mapping and saving entities to the database.
/// </summary>
public class MapHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    public void SaveMap(AppDbContext dbContext)
    {
        SaveEntity(EntityType.Country, dbContext);
        SaveEntity(EntityType.Province, dbContext);
        SaveEntity(EntityType.Municipality, dbContext);
    }

    private const string path_level0 = "../GADM/gadm41_DOM_0.json";
    private const string path_level1 = "../GADM/gadm41_DOM_1.json";
    private const string path_level2 = "../GADM/gadm41_DOM_2.json";

    private void SaveEntity(EntityType entityType, AppDbContext dbContext)
    {
        var path_file = "";
        switch (entityType)
        {
            case EntityType.Country:
                path_file = path_level0;
                break;
            case EntityType.Province:
                path_file = path_level1;
                break;
            case EntityType.Municipality:
                path_file = path_level2;
                break;
        }

        var fileStream = File.OpenRead(path_file);
        var file_content = JsonSerializer.Deserialize<Level2.Root>(fileStream);

        fileStream.Close();
        fileStream.Dispose();

        if (file_content != null && file_content.features != null)
        {
            foreach (var feature in file_content.features)
            {
                var exists = false;
                dynamic entity = null;
                switch (entityType)
                {
                    case EntityType.Country:
                        entity = new mCountry
                        {
                            Nombre = feature.properties.COUNTRY,
                            Codigo = feature.properties.GID_0,
                        };
                        exists = dbContext.Countries.Any(c => c.Codigo == feature.properties.GID_0);
                        break;
                    case EntityType.Province:
                        entity = new mProvince
                        {
                            Nombre = feature.properties.NAME_1,
                            Codigo = feature.properties.GID_1,
                            Tipo = feature.properties.TYPE_1,
                            CountryId = dbContext.Countries.Where(c => c.Codigo == feature.properties.GID_0).Select(x => x.Id).FirstOrDefault()
                        };
                        exists = dbContext.Provinces.Any(p => p.Codigo == feature.properties.GID_1);
                        break;
                    case EntityType.Municipality:
                        entity = new mMunicipality
                        {
                            Nombre = feature.properties.NAME_2,
                            Codigo = feature.properties.GID_2,
                            Tipo = feature.properties.TYPE_2,
                            ProvinceId = dbContext.Provinces.Where(c => c.Codigo == feature.properties.GID_1).Select(x => x.Id).FirstOrDefault()
                        };
                        exists = dbContext.Provinces.Any(p => p.Codigo == feature.properties.GID_2);
                        break;
                }

                if (exists)
                    continue;

                SetMultiPolygon(feature, entity);

                switch (entityType)
                {
                    case EntityType.Country:
                        dbContext.Countries.Add(entity);
                        break;
                    case EntityType.Province:
                        dbContext.Provinces.Add(entity);
                        break;
                    case EntityType.Municipality:
                        dbContext.Municipalities.Add(entity);
                        break;
                }
            }
        }

        dbContext.SaveChanges();
    }

    private void SetMultiPolygon(dynamic feature, dynamic entity)
    {
        var poligonos = new List<Polygon>();

        foreach (var coord in feature.geometry.coordinates)
        {
            foreach (var ring in coord)
            {
                var coordenadas = new Coordinate[ring.Count];
                var i = 0;
                foreach (var point in ring)
                {
                    coordenadas[i++] = new Coordinate(point[0], point[1]);
                }

                poligonos.Add(new Polygon(new LinearRing(coordenadas)));
            }
        }

        entity.Coordenadas = new MultiPolygon(poligonos.ToArray());
    }
}
