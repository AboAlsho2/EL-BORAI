using ELBORAI.Application.DTOs.Orders;
using ELBORAI.Application.DTOs.Payment;
using ELBORAI.Application.Interfaces;
using ELBORAI.Application.Interfaces.Repositories;
using ELBORAI.Application.Interfaces.Services;
using ELBORAI.Application.Specifications.Orders;
using ELBORAI.Application.Specifications.Users;
using ELBORAI.Domain.Entities;
using ELBORAI.Domain.Enums;

namespace ELBORAI.Application.Services;

public class OrderService : IOrderService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Order> _orderRepository;
    private readonly IGenericRepository<OrderItem> _orderItemRepository;
    private readonly IGenericRepository<Product> _productRepository;

    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        IGenericRepository<User> userRepository,
        IGenericRepository<Order> orderRepository,
        IGenericRepository<OrderItem> orderItemRepository,
        IGenericRepository<Product> productRepository,
        ICurrentUserService currentUser,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;

        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CancelOrderAsync(
        int orderId)
    {
        var user = await GetCurrentUserAsync();

        var specification =
            new OrderByUserAndIdSpecification(
                user.Id,
                orderId);

        var order =
            await _orderRepository
                .GetBySpecificationAsync(specification);

        if (order is null)
            return false;

        if (order.Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending orders can be cancelled.");
        }

        var itemsSpecification =
            new OrderItemsByOrderSpecification(order.Id);

        var items =
            await _orderItemRepository
                .GetAllBySpecificationAsync(
                    itemsSpecification);

        foreach (var item in items)
        {
            item.Product.Stock += item.Quantity;

            _productRepository.Update(item.Product);
        }

        order.Status = OrderStatus.Cancelled;

        _orderRepository.Update(order);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<OrderDto?> GetMyOrderByIdAsync(
       int orderId)
    {
        var user = await GetCurrentUserAsync();

        var specification =
            new OrderByUserAndIdSpecification(
                user.Id,
                orderId);

        var order =
            await _orderRepository
                .GetBySpecificationAsync(specification);

        if (order is null)
            return null;

        var itemsSpecification =
            new OrderItemsByOrderSpecification(order.Id);

        var items =
            await _orderItemRepository
                .GetAllBySpecificationAsync(
                    itemsSpecification);

        return MapToDto(order, items);
    }

    public async Task<IReadOnlyList<OrderDto>> GetMyOrdersAsync()
    {
        var user = await GetCurrentUserAsync();

        var specification =
            new OrdersByUserSpecification(user.Id);

        var orders =
            await _orderRepository
                .GetAllBySpecificationAsync(specification);

        var result = new List<OrderDto>();

        foreach (var order in orders)
        {
            var itemsSpecification =
                new OrderItemsByOrderSpecification(order.Id);

            var items =
                await _orderItemRepository
                    .GetAllBySpecificationAsync(
                        itemsSpecification);

            result.Add(MapToDto(order, items));
        }

        return result;
    }

    public async Task<bool> UpdateStatusAsync(
        int orderId,
        UpdateOrderStatusDto dto)
    {
        if (!_currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException();

        if (!_currentUser.IsInRole("ADMIN"))
            throw new UnauthorizedAccessException();

        var order =
            await _orderRepository
                .GetByIdAsync(orderId);

        if (order is null)
            return false;

        order.Status = dto.Status;

        _orderRepository.Update(order);

        await _unitOfWork.SaveChangesAsync();

        return true;
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
                TransactionId = order.Payment.TransactionId,
                PaidAt = order.Payment.PaidAt
            },

            Items = orderItems.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Price = item.Price,
                Quantity = item.Quantity,
                TotalPrice = item.Price * item.Quantity
            }).ToList()
        };
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
                .GetBySpecificationAsync(specification);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User not found.");
        }

        return user;
    }


}
