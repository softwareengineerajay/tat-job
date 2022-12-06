namespace NOV.ES.TAT.Job.Infrastructure;

public interface IRequestManager
{
    Task<bool> ExistAsync(int id);

    Task CreateRequestForCommandAsync<T>(int id);
}
