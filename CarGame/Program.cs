using System.Text.Json;
using System.Text.Json.Nodes;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// The tune-up sliders are saved here so they survive reloads, other browsers and the http/https port switch.
var tuneFile = Path.Combine(app.Environment.ContentRootPath, "App_Data", "tune.json");

app.MapGet("/api/tune", () =>
    File.Exists(tuneFile) ? Results.Content(File.ReadAllText(tuneFile), "application/json") : Results.Json(new { }));

app.MapPost("/api/tune", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var body = await reader.ReadToEndAsync();
    JsonObject? obj = null;
    try { obj = body.Length <= 4096 ? JsonNode.Parse(body) as JsonObject : null; } catch (JsonException) { }
    if (obj is null)
        return Results.BadRequest();

    // keep only numeric values
    var clean = new JsonObject();
    foreach (var (key, value) in obj)
        if (value is JsonValue v && v.TryGetValue<double>(out var d) && double.IsFinite(d))
            clean[key] = d;

    Directory.CreateDirectory(Path.GetDirectoryName(tuneFile)!);
    await File.WriteAllTextAsync(tuneFile, clean.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    return Results.Ok();
});

app.Run();
