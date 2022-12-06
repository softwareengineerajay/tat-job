namespace NOV.ES.TAT.Job.Infrastructure;

public class RequestManager : IRequestManager
{
    private readonly JobDBContext context;

    public RequestManager(JobDBContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> ExistAsync(int id)
    {
        var request = await context.ClientRequests.FindAsync(id);

        return request != null;
    }

    public async Task CreateRequestForCommandAsync<T>(int id)
    {
        var exists = await ExistAsync(id);

        var request = exists ?
            throw new ArgumentException($"Request with {id} already exists") :
            new ClientRequest()
            {
                Id = id,
                Name = typeof(T).Name,
                Time = DateTime.UtcNow
            };

        context.Add(request);

        await context.SaveChangesAsync();
    }
}
