using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DotNet8.WebApi.Controllers;

[ApiController]
[Route("api/dango")]
public class DangosController(ILogger<DangosController> logger) : ControllerBase {
    private readonly static List<DangoItem> DangoMenu = [];
    private static int _skuCounter = 1000;

    // GET: api/dango
    [HttpGet] public IActionResult GetAllDango() {
        logger.LogInformation("Get all Dangos");
        logger.LogInformation("Getting all dango menu items");

        return Ok(DangoMenu);
    }

    // GET: api/dango/{id}
    [HttpGet("{id:guid}")]
    public IActionResult GetDango(Guid id) {
        logger.LogInformation($"Getting dango menu item with id: {id}");
        DangoItem? dango = DangoMenu.Find(d => d.Id == id);

        return dango == null ? NotFound() : Ok(dango);
    }

    // POST: api/dango
    [HttpPost] public IActionResult AddDango([FromBody] DangoRequest request) {
        logger.LogInformation("Adding dango");
        if (request.SkewerCount <= 0)
            return BadRequest("Invalid skewer count");

        string? sku = string.IsNullOrEmpty(request.Sku) ? GenerateSku() : request.Sku;

        DangoItem newDango = new(
            id: Guid.NewGuid(),
            flavor: request.Flavor,
            glazeType: request.GlazeType,
            skewerCount: request.SkewerCount,
            sku: sku,
            isNewArrival: request.IsNewArrival
        );

        DangoMenu.Add(newDango);

        return CreatedAtAction(nameof(GetDango), new { id = newDango.Id }, newDango);
    }

    [ApiExplorerSettings(IgnoreApi = true)]// Hide from public docs
    [HttpPost("initialize")]
    public IActionResult InitializeMenu() {
        logger.LogInformation("Initializing dango menu");
        if (DangoMenu.Count != 0) return Ok("Menu initialized");

        var defaultMenu = JsonSerializer.Deserialize<List<DangoItem>>(System.IO.File.ReadAllText("Mock/default-dango-menu.json"));

        if (defaultMenu == null) return BadRequest("Unable to read default menu");

        DangoMenu.AddRange(defaultMenu);

        return Ok("Menu initialized");
    }

    // PUT: api/dango/{id}
    [HttpPut("{id:guid}")]
    public IActionResult UpdateDango(Guid id, [FromBody] DangoRequest request) {
        logger.LogInformation($"Updating dango menu with id: {id}");
        DangoItem? dango = DangoMenu.Find(d => d.Id == id);

        if (dango == null) return NotFound();

        dango.Update(request.Flavor, request.GlazeType, request.SkewerCount, request.Sku, request.IsNewArrival);

        return NoContent();
    }

    // PATCH: api/dango/{id}
    [HttpPatch("{id:guid}")]
    public IActionResult PartialUpdate(Guid id, [FromBody] DangoPartialUpdate request) {
        logger.LogInformation($"Partial update with id: {id}");
        DangoItem? dango = DangoMenu.Find(d => d.Id == id);

        if (dango == null) return NotFound();

        if (!string.IsNullOrEmpty(request.Flavor)) dango.Flavor = request.Flavor;
        if (!string.IsNullOrEmpty(request.GlazeType)) dango.GlazeType = request.GlazeType;
        if (request.SkewerCount.HasValue) dango.SkewerCount = request.SkewerCount.Value;
        if (!string.IsNullOrEmpty(request.Sku)) dango.Sku = request.Sku;
        if (request.IsNewArrival.HasValue) dango.IsNewArrival = request.IsNewArrival.Value;

        return NoContent();
    }

    // DELETE: api/dango/{id}
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteDango(Guid id) {
        logger.LogInformation($"Deleting dango menu with id: {id}");
        DangoItem? dango = DangoMenu.Find(d => d.Id == id);

        if (dango == null) return NotFound();

        DangoMenu.Remove(dango);

        return NoContent();
    }

    // HEAD: api/dango
    [HttpHead] public IActionResult Head() {
        logger.LogInformation("Head");
        return Ok();
    }

    // OPTIONS: api/dango
    [HttpOptions] public IActionResult Options() {
        logger.LogInformation("Options");
        Response.Headers.Append("Allow", "GET, HEAD, POST, PUT, PATCH, DELETE, OPTIONS");

        return Ok();
    }

    private static string GenerateSku() => $"DNG-{DateTime.Now:yyMM}-{_skuCounter++}";
}

// DTOs
public record DangoRequest(
    string Flavor,
    string GlazeType,
    int SkewerCount,
    bool IsNewArrival,
    string? Sku = null
);

public record DangoPartialUpdate(
    string? Flavor = null,
    string? GlazeType = null,
    int? SkewerCount = null,
    string? Sku = null,
    bool? IsNewArrival = null
);

public class DangoItem(
    Guid id,
    string flavor,
    string glazeType,
    int skewerCount,
    string sku,
    bool isNewArrival) {
    public Guid Id { get; } = id;
    public string Flavor { get; set; } = flavor;
    public string GlazeType { get; set; } = glazeType;
    public int SkewerCount { get; set; } = skewerCount;
    public string Sku { get; set; } = sku;
    public bool IsNewArrival { get; set; } = isNewArrival;
    public DateTime CreatedDate { get; } = DateTime.UtcNow;

    public void Update(string flavor, string glazeType, int skewerCount, string? sku, bool isNewArrival) {
        Flavor = flavor;
        GlazeType = glazeType;
        SkewerCount = skewerCount;
        if (!string.IsNullOrEmpty(sku)) Sku = sku;
        IsNewArrival = isNewArrival;
    }
}
