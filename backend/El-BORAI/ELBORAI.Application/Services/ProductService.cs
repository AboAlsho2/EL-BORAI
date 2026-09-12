using ELBORAI.Application.DTOs.Products;
using ELBORAI.Application.Interfaces;
using ELBORAI.Application.Interfaces.Repositories;
using ELBORAI.Application.Interfaces.Services;
using ELBORAI.Application.Specifications.Products;
using ELBORAI.Application.Specifications.Users;
using ELBORAI.Domain.Entities;

namespace ELBORAI.Application.Services;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;


    public ProductService(
      IGenericRepository<Product> productRepository,
      IGenericRepository<Category> categoryRepository,
      IGenericRepository<User> userRepository,
      ICurrentUserService currentUserService,
      IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
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

        var keycloakUserId = _currentUserService.KeycloakUserId;

        if (keycloakUserId is null)
        {
            throw new UnauthorizedAccessException();
        }

        var specification =
         new UserByKeycloakIdSpecification(keycloakUserId);

        var user = await _userRepository
            .GetBySpecificationAsync(specification);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId,
            MerchantId = user.Id
        };

        await _productRepository.AddAsync(product);

        await _unitOfWork.SaveChangesAsync();

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CategoryName = category.Name
        };


    }


    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var specification = new ProductByIdSpecification(id);

        var product = await _productRepository
            .GetBySpecificationAsync(specification);

        if (product is null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name
        };
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        var specification = new AllProductsSpecification();

        var products = await _productRepository
            .GetAllBySpecificationAsync(specification);

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CategoryId = p.CategoryId,
            CategoryName = p.Category.Name
        }).ToList();
    }



    public async Task<bool> UpdateAsync(
        int id,
        UpdateProductDto dto)
    {
        if (!_currentUserService.IsAuthenticated)
            throw new UnauthorizedAccessException();

        if (!_currentUserService.IsInRole("MERCHANT"))
            throw new UnauthorizedAccessException();

        if (_currentUserService.KeycloakUserId is null)
            throw new UnauthorizedAccessException();

        // 1. Get current local user
        var userSpecification =
            new UserByKeycloakIdSpecification(
                _currentUserService.KeycloakUserId);

        var user = await _userRepository
            .GetBySpecificationAsync(userSpecification);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        // 2. Get product
        var productSpecification =
            new ProductByIdSpecification(id);

        var product = await _productRepository
            .GetBySpecificationAsync(productSpecification);

        if (product is null)
            return false;

        // 3. Check ownership
        if (product.MerchantId != user.Id)
            throw new UnauthorizedAccessException();

        // 4. Check category
        var category = await _categoryRepository
            .GetByIdAsync(dto.CategoryId);

        if (category is null)
            throw new KeyNotFoundException("Category not found.");

        // 5. Update product
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.CategoryId = dto.CategoryId;

        _productRepository.Update(product);

        // 6. Save changes
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        if (!_currentUserService.IsAuthenticated)
            throw new UnauthorizedAccessException();

        if (!_currentUserService.IsInRole("MERCHANT"))
            throw new UnauthorizedAccessException();

        if (_currentUserService.KeycloakUserId is null)
            throw new UnauthorizedAccessException();

        // Get current local user
        var userSpecification =
            new UserByKeycloakIdSpecification(
                _currentUserService.KeycloakUserId);

        var user = await _userRepository
            .GetBySpecificationAsync(userSpecification);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        // Get product
        var productSpecification =
            new ProductByIdSpecification(id);

        var product = await _productRepository
            .GetBySpecificationAsync(productSpecification);

        if (product is null)
            return false;

        // Ownership check
        if (product.MerchantId != user.Id)
            throw new UnauthorizedAccessException();

        // Delete
        _productRepository.Delete(product);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}