using System.Text.Json;
using NetTopologySuite.Geometries;
using DNMOFT.CountryOnMap.Core.Data.Entities;


var factory = new DesignTimeDbContextFactory();
var context = factory.CreateDbContext([]);


const string path_level0 = "../GADM/gadm41_DOM_0.json";
const string path_level1 = "../GADM/gadm41_DOM_1.json";
const string path_level2 = "../GADM/gadm41_DOM_2.json";

//Get the stream of the file
var fileStream = File.OpenRead(path_level0);

//Deserialize the file
var level0 = JsonSerializer.Deserialize<src.GADM.Level0.Root>(fileStream);
fileStream.Close();
fileStream.Dispose();
#region Add the country data to the database
//If the deserialization is successful proceed to add the data to the database
if (level0 != null && level0.features != null)
{
    foreach (var feature in level0.features)
    {
        var entity = new mCountry
        {
            Nombre = feature.properties.COUNTRY,
            Codigo = feature.properties.GID_0,
        };
        //Check if the country already exists in the database
        var exists = context.Countries.Any(c => c.Codigo == entity.Codigo);
        if (exists)
            continue;

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
        context.Countries.Add(entity);
    }
    //Save the changes to the database
    context.SaveChanges();
}
#endregion

// Get the stream of the file level 1
fileStream = File.OpenRead(path_level1);

//Deserialize the file
var level1 = JsonSerializer.Deserialize<src.GADM.Level1.Root>(fileStream);
fileStream.Close();
fileStream.Dispose();
if (level1 != null && level1.features != null)
{
    foreach (var feature in level1.features)
    {
        var entity = new mProvince
        {
            Nombre = feature.properties.NAME_1,
            Codigo = feature.properties.GID_1,
            Tipo = feature.properties.TYPE_1,
            CountryId = context.Countries.Where(c => c.Codigo == feature.properties.GID_0).Select(x=>x.Id).FirstOrDefault()
        };
        var exists = context.Provinces.Any(p => p.Codigo == entity.Codigo);
        if (exists)
            continue;

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

        context.Provinces.Add(entity);
    }
    context.SaveChanges();
}

// Get the stream of the file level 2
fileStream = File.OpenRead(path_level2);

//Deserialize the file
var level2 = JsonSerializer.Deserialize<src.GADM.Level2.Root>(fileStream);
fileStream.Close();
fileStream.Dispose();
if (level2 != null && level2.features != null)
{
    foreach (var feature in level2.features)
    {
        var entity = new mMunicipality
        {
            Nombre = feature.properties.NAME_2,
            Codigo = feature.properties.GID_2,
            Tipo = feature.properties.TYPE_2,
            ProvinceId = context.Provinces.Where(c => c.Codigo == feature.properties.GID_1).Select(x=>x.Id).FirstOrDefault()
        };
        var exists = context.Municipalities.Any(p => p.Codigo == entity.Codigo);
        if (exists)
            continue;

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

        context.Municipalities.Add(entity);
    }
    context.SaveChanges();
}