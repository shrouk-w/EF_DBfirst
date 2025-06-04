namespace EF_DBFIRST.Services;

public interface IClientService
{
    public Task DeleteClient(CancellationToken token, int id);
}