namespace Eshop.WebApi.Infrastructure
{
    public interface IUserContext
    {
        Guid? GetUserId();
    }
}
