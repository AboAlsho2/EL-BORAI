using ELBORAI.Application.DTOs.Orders;
using ELBORAI.Application.DTOs.Payment;
using ELBORAI.Application.Interfaces;
using ELBORAI.Application.Interfaces.Repositories;
using ELBORAI.Application.Interfaces.Services;
using ELBORAI.Application.Specifications.Payments;
using ELBORAI.Application.Specifications.Users;
using ELBORAI.Domain.Entities;
using ELBORAI.Domain.Enums;

namespace ELBORAI.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IGenericRepository<Payment> _paymentRepository;
    private readonly IGenericRepository<User> _userRepository;

    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(
        IGenericRepository<Payment> paymentRepository,
        IGenericRepository<User> userRepository,
        ICurrentUserService currentUser,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;

        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaymentDto?> GetByOrderIdAsync(
        int orderId)
    {
        var user = await GetCurrentUserAsync();

        var specification =
            new PaymentByOrderSpecification(orderId);

        var payment =
            await _paymentRepository
                .GetBySpecificationAsync(specification);

        if (payment is null)
            return null;

        if (payment.Order.UserId != user.Id)
            throw new UnauthorizedAccessException();

        return MapToDto(payment);
    }

    public async Task<bool> UpdateStatusAsync(
       int orderId,
       PaymentStatus status)
    {
        if (!_currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException();

        if (!_currentUser.IsInRole("ADMIN"))
            throw new UnauthorizedAccessException();

        var specification =
            new PaymentByOrderSpecification(orderId);

        var payment =
            await _paymentRepository
                .GetBySpecificationAsync(specification);

        if (payment is null)
            return false;

        payment.Status = status;

        if (status == PaymentStatus.Succeeded)
        {
            payment.PaidAt = DateTime.UtcNow;
        }

        _paymentRepository.Update(payment);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private PaymentDto MapToDto(Payment payment)
    {
        return new PaymentDto
        {
            Method = payment.Method,
            Status = payment.Status,
            Amount = payment.Amount,
            TransactionId = payment.TransactionId,
            PaidAt = payment.PaidAt
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
            throw new KeyNotFoundException(
                "User not found.");

        return user;
    }
}
