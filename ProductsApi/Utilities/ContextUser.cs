using System.Security.Claims;

namespace ProductsApi.Utilities
{
    public class ContextUser : IContextUser
    {
        private readonly IHttpContextAccessor _context;

        public ContextUser(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string? UserId => _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public string? Email => _context.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public List<string> Roles => _context.HttpContext?.User?
            .FindAll("role")
            .Select(r => r.Value)
            .ToList() ?? new();
    }
}
