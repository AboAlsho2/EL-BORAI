using ELBORAI.Application.DTOs.Carts;
using ELBORAI.Application.Interfaces;
using ELBORAI.Application.Interfaces.Repositories;
using ELBORAI.Application.Interfaces.Services;
using ELBORAI.Application.Specifications;
using ELBORAI.Application.Specifications.Carts;
using ELBORAI.Application.Specifications.Users;
using ELBORAI.Domain.Entities;

namespace ELBORAI.Application.Services;

public class CartService : ICartService
{
    private readonly IGenericRepository<Cart> _cartRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<CartItem> _cartItemRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public CartService(
        IGenericRepository<Cart> cartRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<User> userRepository,
        IGenericRepository<CartItem> cartItemRepository,
        ICurrentUserService currentUser,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
        _cartItemRepository = cartItemRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;

    }

    public async Task<bool> UpdateQuantityAsync(
        int productId,
        int quantity)
    {
        var user = await GetCurrentUserAsync();

        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.");

        var cartSpecification =
            new CartByUserSpecification(user.Id);

        var cart = await _cartRepository
            .GetBySpecificationAsync(cartSpecification);

        if (cart is null)
            return false;

        var itemSpecification =
            new CartItemByCartAndProductSpecification(
                cart.Id,
                productId);

        var cartItem = await _cartItemRepository
            .GetBySpecificationAsync(itemSpecification);

        if (cartItem is null)
            return false;

        var product = await _productRepository
            .GetByIdAsync(productId);

        if (product is null)
            throw new KeyNotFoundException(
                "Product not found.");

        if (quantity > product.Stock)
            throw new InvalidOperationException(
                "Not enough stock.");

        cartItem.Quantity = quantity;

        _cartItemRepository.Update(cartItem);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RemoveFromCartAsync(int productId)
    {
        var user = await GetCurrentUserAsync();

        var cartSpecification =
            new CartByUserSpecification(user.Id);

        var cart = await _cartRepository
            .GetBySpecificationAsync(cartSpecification);

        if (cart is null)
            return false;

        var itemSpecification =
            new CartItemByCartAndProductSpecification(
                cart.Id,
                productId);

        var cartItem = await _cartItemRepository
            .GetBySpecificationAsync(itemSpecification);

        if (cartItem is null)
            return false;

        _cartItemRepository.Delete(cartItem);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<CartDto> AddToCartAsync(AddToCartDto dto)
    {
        // 1. Get current user
        var user = await GetCurrentUserAsync();

        // 2. Validate quantity
        if (dto.Quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.");

        // 3. Get product
        var product = await _productRepository
            .GetByIdAsync(dto.ProductId);

        if (product is null)
            throw new KeyNotFoundException(
                "Product not found.");

        // 4. Check stock
        if (product.Stock < dto.Quantity)
            throw new InvalidOperationException(
                "Not enough stock.");

        // 5. Get user's cart
        var cartSpecification =
            new CartByUserSpecification(user.Id);

        var cart = await _cartRepository
            .GetBySpecificationAsync(cartSpecification);

        // 6. Create cart if it doesn't exist
        if (cart is null)
        {
            cart = new Cart
            {
                UserId = user.Id
            };

            await _cartRepository.AddAsync(cart);

            await _unitOfWork.SaveChangesAsync();
        }

        // 7. Check if product already exists in cart
        var itemSpecification =
            new CartItemByCartAndProductSpecification(
                cart.Id,
                product.Id);

        var cartItem = await _cartItemRepository
            .GetBySpecificationAsync(itemSpecification);

        // 8. Existing item
        if (cartItem is not null)
        {
            if (cartItem.Quantity + dto.Quantity > product.Stock)
                throw new InvalidOperationException(
                    "Not enough stock.");

            cartItem.Quantity += dto.Quantity;

            _cartItemRepository.Update(cartItem);
        }
        // 9. New item
        else
        {
            cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = dto.Quantity
            };

            await _cartItemRepository.AddAsync(cartItem);
        }

        // 10. Save
        await _unitOfWork.SaveChangesAsync();

        // 11. Return updated cart
        return await GetCartAsync()
            ?? throw new InvalidOperationException(
                "Cart could not be retrieved.");
    }

    public async Task<CartDto?> GetCartAsync()
    {
        var user = await GetCurrentUserAsync();

        var cartSpecification =
            new CartByUserSpecification(user.Id);

        var cart = await _cartRepository
            .GetBySpecificationAsync(cartSpecification);

        if (cart is null)
            return null;

        var itemsSpecification =
            new CartItemsByCartSpecification(cart.Id);

        var items = await _cartItemRepository
            .GetAllBySpecificationAsync(itemsSpecification);

        var itemDtos = items.Select(item => new CartItemDto
        {
            ProductId = item.ProductId,
            ProductName = item.Product.Name,
            UnitPrice = item.Product.Price,
            Quantity = item.Quantity,
            TotalPrice = item.Product.Price * item.Quantity
        }).ToList();

        return new CartDto
        {
            Id = cart.Id,
            Items = itemDtos,
            TotalPrice = itemDtos.Sum(x => x.TotalPrice)
        };
    }

    public async Task ClearCartAsync()
    {
        var user = await GetCurrentUserAsync();

        var cartSpecification =
            new CartByUserSpecification(user.Id);

        var cart = await _cartRepository
            .GetBySpecificationAsync(cartSpecification);

        if (cart is null)
            return;

        var itemsSpecification =
            new CartItemsByCartSpecification(cart.Id);

        var items = await _cartItemRepository
            .GetAllBySpecificationAsync(itemsSpecification);

        foreach (var item in items)
        {
            _cartItemRepository.Delete(item);
        }

        await _unitOfWork.SaveChangesAsync();
    }





    // Helper Method to get current user 
    private async Task<User> GetCurrentUserAsync()
    {
        if (!_currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException();

        if (_currentUser.KeycloakUserId is null)
            throw new UnauthorizedAccessException();

        var specification =
            new UserByKeycloakIdSpecification(
                _currentUser.KeycloakUserId);

        var user = await _userRepository
            .GetBySpecificationAsync(specification);

        if (user is null)
            throw new KeyNotFoundException(
                "User not found.");

        return user;
    }
}