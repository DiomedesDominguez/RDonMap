using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;


const string path_level0 = "src/GADM/gadm41_DOM_0.json";
const string path_level1 = "src/GADM/gadm41_DOM_1.json";
const string path_level2 = "src/GADM/gadm41_DOM_2.json";

var level0 = JsonSerializer.Deserialize<src.GADM.Level0.Root>(path_level0);
var level1 = JsonSerializer.Deserialize<src.GADM.Level1.Root>(path_level1);
var level2 = JsonSerializer.Deserialize<src.GADM.Level1.Root>(path_level2);
