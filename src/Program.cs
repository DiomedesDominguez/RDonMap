using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using src.Data.Context;
using NetTopologySuite.Geometries;


var factory = new DesignTimeDbContextFactory();
var context = factory.CreateDbContext([]);


const string path_level0 = "../GADM/gadm41_DOM_0.json";
const string path_level1 = "../GADM/gadm41_DOM_1.json";
// const string path_level2 = "../GADM/gadm41_DOM_2.json";

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
    foreach (var country in level0.features)
    {
        var countryEntity = new src.Data.Entities.mCountry
        {
            Nombre = country.properties.COUNTRY,
            Codigo = country.properties.GID_0,
        };
        //Check if the country already exists in the database
        var exists = context.Countries.Any(c => c.Codigo == countryEntity.Codigo);
        if (exists)
            continue;
            
        var poligonos = new List<Polygon>();

        foreach (var coord in country.geometry.coordinates)
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

        countryEntity.Coordenadas = new MultiPolygon(poligonos.ToArray());


        context.Countries.Add(countryEntity);
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
    foreach (var province in level1.features)
    {
        var provinceEntity = new src.Data.Entities.mProvince
        {
            Nombre = province.properties.NAME_1,
            Codigo = province.properties.GID_1,
            //Shape = province.geometry
        };
        var exists = context.Provinces.Any(p => p.Codigo == provinceEntity.Codigo);
        if (!exists)
            context.Provinces.Add(provinceEntity);
    }
    context.SaveChanges();
}
// Read