using Api.Data;
using Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<HouseDbContext>(options =>
options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddScoped<IHouseRepository, HouseRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(p =>
p.WithOrigins("http://localhost:3000")
.AllowAnyHeader()
.AllowAnyMethod());

app.UseHttpsRedirection();

app.MapGet("/houses", (IHouseRepository repo) => repo.GetAll())
.Produces<List<HouseDto>>(StatusCodes.Status200OK);

app.MapGet("/houses/{houseId:int}",
    async (int houseId, IHouseRepository _repository)
    =>
    {
        var house = await _repository.GetById(id: houseId);
        if (house == null)
        {
            return Results.Problem($"House with ID {houseId} not found.", statusCode: 404);
        }

        return Results.Ok(house);
    }).ProducesProblem(404)
    .Produces<HouseDetailDto>(StatusCodes.Status200OK);

app.MapPost("/houses",
    async ([FromBody] HouseDetailDto dto, IHouseRepository _repository) =>
    {
        if(!MiniValidator.TryValidate(dto, out var errors))
            return Results.ValidationProblem(errors);
        var newHouse = await _repository.Add(dto);
        return Results.Created($"/house/{newHouse.Id}", newHouse);
    }).Produces<HouseDetailDto>(StatusCodes.Status201Created)
    .ProducesValidationProblem();

app.MapPut("/houses", async ([FromBody] HouseDetailDto dto, IHouseRepository _repository) =>
{
    if (!MiniValidator.TryValidate(dto, out var errors))
        return Results.ValidationProblem(errors);
    var house = await _repository.GetById(dto.Id);
    if (house == null)
        return Results.Problem($"House with ID {dto.Id} not found.", statusCode: 404);

    var updatedHouse = await _repository.Update(dto);
    return Results.Ok(updatedHouse);

}).ProducesProblem(404)
.Produces<HouseDetailDto>(StatusCodes.Status200OK)
.ProducesValidationProblem();

app.MapDelete("/houses/{houseId:int}", async (int houseId, IHouseRepository _repository) =>
{
    var house = await _repository.GetById(houseId);
    if (house == null)
        return Results.Problem($"House with ID {houseId} not found.", statusCode: 404);

    await _repository.Delete(houseId);
    return Results.Ok();

}).ProducesProblem(404)
.Produces(StatusCodes.Status200OK);

app.Run();