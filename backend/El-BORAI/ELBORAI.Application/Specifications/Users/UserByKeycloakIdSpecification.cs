using ELBORAI.Domain.Entities;

namespace ELBORAI.Application.Specifications.Users;

public class UserByKeycloakIdSpecification
    : BaseSpecification<User>
{
    public UserByKeycloakIdSpecification(string keycloakUserId)
        : base(u => u.KeycloakUserId == keycloakUserId)
    {
    }
}