using Core.Entities;

namespace Core.Specifications.UserSpecs;

public class PaginatedUsersSpec : BaseSpecification<User>
{
    public PaginatedUsersSpec(UserSpecParams specParams)
    {
        if(!string.IsNullOrWhiteSpace(specParams.Search))
        {
            AddCriteria(u =>
                u.FirstName.ToLower().Contains(specParams.Search.ToLower()) ||
                u.LastName.ToLower().Contains(specParams.Search.ToLower()) ||
                u.Email.ToLower().Contains(specParams.Search.ToLower())
            );

            if (!string.IsNullOrWhiteSpace(specParams.Sort))
            {
                switch (specParams.Sort.ToLower())
                {
                    case "firstname":
                        if (specParams.SortDesc)
                            AddOrderByDescending(u => u.FirstName);
                        else
                            AddOrderBy(u => u.FirstName);
                            break;
                    case "createdat":
                    default:
                        if (specParams.SortDesc)
                            AddOrderByDescending(u => u.CreatedAt);
                        else
                            AddOrderBy(u => u.CreatedAt);
                        break;
                }
            }
        }
    }
}
