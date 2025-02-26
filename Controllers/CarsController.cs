using Microsoft.AspNetCore.Mvc;

namespace DotNet8.WebApi.Controllers;

[ApiController]
[Route("api/cars")]
public class CarsController(ILogger<CarsController> logger) : ControllerBase {
    private readonly static List<Car> Cars = [];
    private static int _vinCounter = 1000;

    // GET: api/cars
    [HttpGet] public IActionResult GetAllCars() {
        logger.LogInformation("GET all cars");

        return Ok(Cars);
    }

    // GET: api/cars/{id}
    [HttpGet("{id:guid}")]
    public IActionResult GetCar(Guid id) {
        logger.LogInformation($"GET {id}");
        Car? car = Cars.Find(c => c.Id == id);

        return car == null ? NotFound() : Ok(car);
    }

    // POST: api/cars
    [HttpPost] public IActionResult AddCar([FromBody] CarRequest request) {
        logger.LogInformation($"POST {request}");
        if (request.Year < 1886 || request.Year > DateTime.Now.Year + 1)
            return BadRequest("Invalid year");

        string vin = string.IsNullOrEmpty(request.Vin)
            ? GenerateVin()
            : request.Vin;

        Car newCar = new(
            id: Guid.NewGuid(),
            make: request.Make,
            model: request.Model,
            year: request.Year,
            vin: vin,
            addedDate: DateTime.UtcNow
        );

        Cars.Add(newCar);

        return CreatedAtAction(nameof(GetCar), new { id = newCar.Id }, newCar);
    }

    // PUT: api/cars/{id}
    [HttpPut("{id:guid}")]
    public IActionResult UpdateCar(Guid id, [FromBody] CarRequest request) {
        logger.LogInformation($"PUT {id}");
        Car? car = Cars.Find(c => c.Id == id);

        if (car == null) return NotFound();

        car.Update(request.Make, request.Model, request.Year, request.Vin);

        return NoContent();
    }

    // PATCH: api/cars/{id}
    [HttpPatch("{id:guid}")]
    public IActionResult PartialUpdate(Guid id, [FromBody] CarPartialUpdate request) {
        logger.LogInformation($"PATCH {id}");
        Car? car = Cars.Find(c => c.Id == id);

        if (car == null) return NotFound();

        if (!string.IsNullOrEmpty(request.Make)) car.Make = request.Make;
        if (!string.IsNullOrEmpty(request.Model)) car.Model = request.Model;
        if (request.Year.HasValue) car.Year = request.Year.Value;
        if (!string.IsNullOrEmpty(request.Vin)) car.Vin = request.Vin;

        return NoContent();
    }

    // DELETE: api/cars/{id}
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteCar(Guid id) {
        logger.LogInformation($"DELETE {id}");
        var car = Cars.Find(c => c.Id == id);

        if (car == null) return NotFound();

        Cars.Remove(car);

        return NoContent();
    }

    // HEAD: api/cars
    [HttpHead] public IActionResult Head() {
        logger.LogInformation("HEAD");
        return Ok();
    }

    // OPTIONS: api/cars
    [HttpOptions] public IActionResult Options() {
        logger.LogInformation("OPTIONS");
        Response.Headers.Append("Allow", "GET, HEAD, POST, PUT, PATCH, DELETE, OPTIONS");

        return Ok();
    }

    private static string GenerateVin() => $"1M8GDM9A_{_vinCounter++}";
}

public record CarRequest(
    string Make,
    string Model,
    int Year,
    string? Vin = null
);

public record CarPartialUpdate(
    string? Make = null,
    string? Model = null,
    int? Year = null,
    string? Vin = null
);

public class Car(Guid id, string make, string model, int year, string vin, DateTime addedDate) {
    public Guid Id { get; } = id;
    public string Make { get; set; } = make;
    public string Model { get; set; } = model;
    public int Year { get; set; } = year;
    public string Vin { get; set; } = vin;
    public DateTime AddedDate { get; } = addedDate;

    public void Update(string make, string model, int year, string? vin) {
        Make = make;
        Model = model;
        Year = year;
        if (!string.IsNullOrEmpty(vin)) Vin = vin;
    }
}
