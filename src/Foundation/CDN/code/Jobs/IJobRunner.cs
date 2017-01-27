namespace Sitecore.Foundation.CDN.Jobs
{
    using Data;

    public interface IJobRunner
    {
        void Start(string name, string category, string siteName, params object[] parameters);

        void ProcessJob(Database database);
    }
}