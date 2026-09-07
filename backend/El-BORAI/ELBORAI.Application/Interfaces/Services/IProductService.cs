using ELBORAI.Application.DTOs.Products;

namespace ELBORAI.Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(int id);

    Task<IReadOnlyList<ProductDto>> GetAllAsync();

    Task<ProductDto> CreateAsync(CreateProductDto dto);

    Task<bool> UpdateAsync(int id, UpdateProductDto dto);

    Task<bool> DeleteAsync(int id);
}