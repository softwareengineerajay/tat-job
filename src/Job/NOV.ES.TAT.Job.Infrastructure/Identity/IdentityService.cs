using Microsoft.AspNetCore.Http;

namespace NOV.ES.TAT.Job.Infrastructure;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor context;

    public IdentityService(IHttpContextAccessor context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public string GetUserIdentity()
    {
        return context.HttpContext.User.FindFirst("sub").Value;
    }

    public string GetUserName()
    {
        return string.IsNullOrEmpty(context.HttpContext.User.Identity.Name) ? "API" : context.HttpContext.User.Identity.Name;
    }
}
