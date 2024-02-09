namespace IllegalLibAPI.Interfaces
{
    public interface IAdminRepostiory
    {
        Task MarkReqAsCompletedAsync(int requestId);
    }
}