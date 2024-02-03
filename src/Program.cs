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


var factory = new DesignTimeDbContextFactory();
var context = factory.CreateDbContext([]);


const string path_level0 = "../GADM/gadm41_DOM_0.json";
// const string path_level1 = "../GADM/gadm41_DOM_1.json";
// const string path_level2 = "../GADM/gadm41_DOM_2.json";

//Get the stream of the file
using var fileStream = File.OpenRead(path_level0);

//Deserialize the file
var level0 = JsonSerializer.Deserialize<src.GADM.Level0.Root>(fileStream);

//If the deserialization is successful proceed to add the data to the database
if (level0 != null && level0.features != null)
{
    foreach (var country in level0.features)
    {
        var countryEntity = new src.Data.Entities.mCountry
        {
            Nombre = country.properties.COUNTRY,
            Codigo = country.properties.GID_0,
            //Shape = country.geometry
        };
        
        //Check if the country already exists in the database
        var exists = context.Countries.Any(c => c.Codigo == countryEntity.Codigo);
        if (!exists)
            context.Countries.Add(countryEntity);
    }
    //Save the changes to the database
    context.SaveChanges();
}


// var level1 = JsonSerializer.Deserialize<src.GADM.Level1.Root>(path_level1);
// var level2 = JsonSerializer.Deserialize<src.GADM.Level1.Root>(path_level2);
