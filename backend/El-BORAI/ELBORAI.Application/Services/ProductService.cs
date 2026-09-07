using ELBORAI.Application.DTOs.Products;
using ELBORAI.Application.Interfaces;
using ELBORAI.Application.Interfaces.Repositories;
using ELBORAI.Application.Interfaces.Services;
using ELBORAI.Domain.Entities;

namespace ELBORAI.Application.Services;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;


    public ProductService(
      IGenericRepository<Product> productRepository,
      IGenericRepository<Category> categoryRepository,
      ICurrentUserService currentUserService,
      IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        if (!_currentUserService.IsAuthenticated)
            throw new UnauthorizedAccessException();

        if (!_currentUserService.IsInRole("MERCHANT"))
            throw new UnauthorizedAccessException(
                "Only merchants can create products.");

        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);

        if (category is null)
        {
            throw new KeyNotFoundException(
                "Category was not found.");
        }

        var merchantId = _currentUserService.UserId;

        if (merchantId is null)
        {
            throw new UnauthorizedAccessException();
        }

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId,
            MerchantId = merchantId.Value
        };

        await _productRepository.AddAsync(product);

        await _unitOfWork.SaveChangesAsync();


    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ProductDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(int id, UpdateProductDto dto)
    {
        throw new NotImplementedException();
    }
}