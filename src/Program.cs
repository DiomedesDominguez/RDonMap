using DNMOFT.CountryOnMap.Core;


var factory = new DesignTimeDbContextFactory();
var context = factory.CreateDbContext([]);
var helper = new MapHelper();
helper.SaveMap(context);