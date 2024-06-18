using Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;
public class HouseRepository : IHouseRepository
{
    private readonly HouseDbContext _context;
    public HouseRepository(HouseDbContext context)
    {
        _context = context;
    }

    public async Task<List<HouseDto>> GetAll()
    {
        return await _context.Houses
        .Select(h => new HouseDto(h.Id, h.Address, h.Country, h.Price))
        .ToListAsync(); ;
    }

    public async Task<HouseDetailDto?> GetById(int id)
    {
        var entity = await _context.Houses.SingleOrDefaultAsync(x => x.Id == id);
        if (entity == null)
            return null;
        return EntityToDetailDto(entity);
    }

    public async Task<HouseDetailDto> Add(HouseDetailDto houseDetailDto)
    {
        var entity = new HouseEntity();
        DtoToEntity(houseDetailDto, entity);
        _context.Houses.Add(entity);
        await _context.SaveChangesAsync();
        return EntityToDetailDto(entity);
    }

    public async Task<HouseDetailDto> Update(HouseDetailDto houseDetailDto)
    {
        var entity = await _context.Houses.FindAsync(houseDetailDto.Id);
        if (entity == null)
            throw new ArgumentException($"Error updating house {houseDetailDto.Id}");

        DtoToEntity(houseDetailDto, entity);
        _context.Entry(entity).State = EntityState.Modified;        
        await _context.SaveChangesAsync();
        return EntityToDetailDto(entity);
    }

    public async Task Delete(int id)
    {
        var entity = await _context.Houses.FindAsync(id);
        if (entity == null)
            throw new ArgumentException($"Error updating house {id}");

        _context.Houses.Remove(entity);
        await _context.SaveChangesAsync();
    }
    private static void DtoToEntity(HouseDetailDto dto, HouseEntity entity)
    {
        entity.Address = dto.Address;
        entity.Country = dto.Country;
        entity.Price = dto.Price;
        entity.Description = dto.Description;
        entity.Photo = dto.Photo;
    }

    private static HouseDetailDto EntityToDetailDto(HouseEntity entity)
        => new HouseDetailDto(entity.Id, entity.Address, entity.Country, entity.Price, entity.Description, entity.Photo);
}