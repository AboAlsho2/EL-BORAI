using ELBORAI.Application.DTOs.Orders;
using ELBORAI.Application.DTOs.Payment;
using ELBORAI.Application.Interfaces;
using ELBORAI.Application.Interfaces.Repositories;
using ELBORAI.Application.Interfaces.Services;
using ELBORAI.Application.Specifications;
using ELBORAI.Application.Specifications.Carts;
using ELBORAI.Application.Specifications.Users;
using ELBORAI.Domain.Entities;
using ELBORAI.Domain.Enums;

namespace ELBORAI.Application.Services;

public class CheckoutService : ICheckoutService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Cart> _cartRepository;
    private readonly IGenericRepository<CartItem> _cartItemRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Order> _orderRepository;
    private readonly IGenericRepository<OrderItem> _orderItemRepository;
    private readonly IGenericRepository<Payment> _paymentRepository;

    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutService(
        IGenericRepository<User> userRepository,
        IGenericRepository<Cart> cartRepository,
        IGenericRepository<CartItem> cartItemRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<Order> orderRepository,
        IGenericRepository<OrderItem> orderItemRepository,
        IGenericRepository<Payment> paymentRepository,
        ICurrentUserService currentUser,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _paymentRepository = paymentRepository;

        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDto> CheckoutAsync(
        CheckoutDto dto)
    {
        // 1. Get current user
        var user = await GetCurrentUserAsync();

        // 2. Get user's cart
        var cartSpecification =
            new CartByUserSpecification(user.Id);

        var cart =
            await _cartRepository
                .GetBySpecificationAsync(cartSpecification);

        if (cart is null)
            throw new InvalidOperationException(
                "Cart is empty.");

        // 3. Get cart items
        var itemsSpecification =
            new CartItemsByCartSpecification(cart.Id);

        var cartItems =
            await _cartItemRepository
                .GetAllBySpecificationAsync(
                    itemsSpecification);

        if (cartItems.Count == 0)
            throw new InvalidOperationException(
                "Cart is empty.");

        // 4. Validate stock
        foreach (var item in cartItems)
        {
            if (item.Product.Stock < item.Quantity)
            {
                throw new InvalidOperationException(
                    $"Not enough stock for product: " +
                    $"{item.Product.Name}");
            }
        }

        // 5. Calculate total
        var totalPrice = cartItems.Sum(
            item => item.Product.Price * item.Quantity);

        // 6. Create order
        var order = new Order
        {
            UserId = user.Id,
            TotalPrice = totalPrice,
            Status = OrderStatus.Pending
        };

        await _orderRepository.AddAsync(order);

        // 7. Create order items
        foreach (var cartItem in cartItems)
        {
            var orderItem = new OrderItem
            {
                Order = order,
                ProductId = cartItem.ProductId,
                Price = cartItem.Product.Price,
                Quantity = cartItem.Quantity
            };

            await _orderItemRepository.AddAsync(orderItem);
        }

        // 8. Decrease stock
        foreach (var cartItem in cartItems)
        {
            cartItem.Product.Stock -= cartItem.Quantity;

            _productRepository.Update(
                cartItem.Product);
        }

        // 9. Create payment
        var payment = new Payment
        {
            Order = order,
            Method = dto.PaymentMethod,
            Status = PaymentStatus.Pending,
            Amount = totalPrice
        };

        await _paymentRepository.AddAsync(payment);

        // 10. Clear cart
        foreach (var cartItem in cartItems)
        {
            _cartItemRepository.Delete(cartItem);
        }

        // 11. Save everything
        await _unitOfWork.SaveChangesAsync();

        // 12. Get saved order with payment
        var orderSpecification =
            new OrderByIdSpecification(order.Id);

        var savedOrder =
            await _orderRepository
                .GetBySpecificationAsync(
                    orderSpecification);

        if (savedOrder is null)
        {
            throw new InvalidOperationException(
                "Order could not be retrieved.");
        }

        // 13. Get order items
        var orderItemsSpecification =
            new OrderItemsByOrderSpecification(
                savedOrder.Id);

        var orderItems =
            await _orderItemRepository
                .GetAllBySpecificationAsync(
                    orderItemsSpecification);

        // 14. Map to DTO
        return MapToDto(
            savedOrder,
            orderItems);
    }

    private async Task<User> GetCurrentUserAsync()
    {
        if (!_currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException();

        if (_currentUser.KeycloakUserId is null)
            throw new UnauthorizedAccessException();

        var specification =
            new UserByKeycloakIdSpecification(
                _currentUser.KeycloakUserId);

        var user =
            await _userRepository
                .GetBySpecificationAsync(
                    specification);

        if (user is null)
            throw new KeyNotFoundException(
                "User not found.");

        return user;
    }

    private OrderDto MapToDto(
        Order order,
        IReadOnlyList<OrderItem> orderItems)
    {
        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalPrice = order.TotalPrice,
            Status = order.Status,

            Payment = new PaymentDto
            {
                Method = order.Payment.Method,
                Status = order.Payment.Status,
                Amount = order.Payment.Amount,
                TransactionId =
                    order.Payment.TransactionId,
                PaidAt = order.Payment.PaidAt
            },

            Items = orderItems.Select(item =>
                new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    TotalPrice =
                        item.Price * item.Quantity
                }).ToList()
        };
    }
}