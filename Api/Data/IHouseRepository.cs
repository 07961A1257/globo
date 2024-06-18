using Api.Dtos;

namespace Api.Data
{
    public interface IHouseRepository
    {
        Task<List<HouseDto>> GetAll();
        Task<HouseDetailDto?> GetById(int id);
        Task<HouseDetailDto> Add(HouseDetailDto houseDetailDto);
        Task<HouseDetailDto> Update(HouseDetailDto houseDetailDto);
        Task Delete(int id);
    }
}