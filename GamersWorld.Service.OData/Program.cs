using GamersWorld.Data;
using GamersWorld.DataContext;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Game>("Games"); // !!!

builder.Services.AddGamersWorld();

builder.Services.AddControllers().AddOData(options =>
    options
        .AddRouteComponents(routePrefix: "gamebox/v{version}", model: modelBuilder.GetEdmModel())
        .Select()
        .Expand()
        .Filter()
        .OrderBy()
        .SetMaxTop(10)
        .Count()
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
