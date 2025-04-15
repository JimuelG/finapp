using Core.Entities;

namespace Core.Specifications.UserSpecs;

public class UserWithEmailSpec : BaseSpecification<User>
{
    public UserWithEmailSpec(string email)
    {
        AddCriteria(u => u.Email == email);
    }
}
