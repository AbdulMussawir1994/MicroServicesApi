namespace ProductsApi.Utilities
{
    public interface IContextUser
    {
        string? UserId { get; }
        string? Email { get; }
        List<string> Roles { get; }
    }
}
