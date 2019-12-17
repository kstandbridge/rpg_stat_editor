using System.IO;
using System.Threading.Tasks;

namespace StatEditor
{
    public interface IPersistenceManager
    {
        Task<string> LoadEntitiesDataAsync(string uri);
        Task SaveEntitiesAsync(string uri, string data);
    }

    public class PersistenceManager : IPersistenceManager
    {
        public Task<string> LoadEntitiesDataAsync(string uri)
        {
            return Task.FromResult(File.ReadAllText(uri));
        }

        public Task SaveEntitiesAsync(string uri, string data)
        {
            File.WriteAllText(uri, data);

            return Task.CompletedTask;
        }

    }
}