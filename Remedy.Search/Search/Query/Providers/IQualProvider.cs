namespace Remedy.Search.Query.Providers
{
    using Remedy.Search.Query;

    public interface IQualProvider
    {
        string BuildQual(IQueryBuilder builder);
    }
}
