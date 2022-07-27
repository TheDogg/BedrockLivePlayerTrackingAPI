using DiscordBotTestAPI;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PlayerPositionsDb>(opt => opt.UseInMemoryDatabase("PlayerPosition"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/playerpositions", async (PlayerPositionsDb db) =>
{
    await db.PlayerPositions.ToListAsync();
});

app.MapGet("/playerpositions/{name}", async (string name, PlayerPositionsDb db) =>
    await db.PlayerPositions.FindAsync(name)
        is PlayerPosition playerPosition
            ? Results.Ok(playerPosition)
            : Results.NotFound()
);

app.MapPost("/playerposition", async (PlayerPosition playerPosition, PlayerPositionsDb db) =>
{
    if (db.PlayerPositions.Find(playerPosition.Name) != null)
    {
        db.PlayerPositions.Update(playerPosition);
    }
    else
    {
        db.PlayerPositions.Add(playerPosition);
    }
    await db.SaveChangesAsync();

    return Results.Created($"/playerpositions/{playerPosition.Name}", playerPosition);
});

app.MapPost("/playerpositions", async (PlayerPosition[] playerPositions, PlayerPositionsDb db) =>
{
    Console.WriteLine(playerPositions);
    db.Database.EnsureDeleted();
    foreach (var playerPosition in playerPositions)
    {
        db.PlayerPositions.Add(playerPosition);
    }
    await db.SaveChangesAsync();
    //Write PapyrusCS Marker File
    Helper.WritePapyrusPlayerPositionsFile(db);
    //Write Unmined Marker File
    Helper.WriteUnminedPlayerPositionsFile(db);
});

app.Run();

public class PlayerPosition
{
    //public int Id { get; set; }
    [Key]
    public string? Name { get; set; }
    public int DimensionId { get; set; }
    public int XCoord { get; set; }
    public int ZCoord { get; set; }
    public string? Color { get; set; }
    public bool Visible { get; set; }
}

public class PlayerPositionsDb : DbContext
{
    public PlayerPositionsDb(DbContextOptions<PlayerPositionsDb> options)
        : base(options) { }

    public DbSet<PlayerPosition> PlayerPositions => Set<PlayerPosition>();
}