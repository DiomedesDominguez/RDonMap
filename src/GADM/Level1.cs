using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.GADM
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class Level1
    {
       // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Crs
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<List<List<List<double>>>> coordinates { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
        public string GID_1 { get; set; }
        public string GID_0 { get; set; }
        public string COUNTRY { get; set; }
        public string NAME_1 { get; set; }
        public string VARNAME_1 { get; set; }
        public string NL_NAME_1 { get; set; }
        public string TYPE_1 { get; set; }
        public string ENGTYPE_1 { get; set; }
        public string CC_1 { get; set; }
        public string HASC_1 { get; set; }
        public string ISO_1 { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public string name { get; set; }
        public Crs crs { get; set; }
        public List<Feature> features { get; set; }
    }

 
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}