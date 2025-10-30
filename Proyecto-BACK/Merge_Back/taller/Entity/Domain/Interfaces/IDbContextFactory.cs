namespace Entity.Domain.Interfaces
{
    public interface IDbContextFactory
    {
        IApplicationDbContext CreateDbContext();
    }

}
