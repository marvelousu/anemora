using Newtonsoft.Json.Linq;

namespace Anemora.Save.Migration
{
    public interface ISaveMigration
    {
        int FromVersion { get; }
        int ToVersion { get; }
        JObject Migrate(JObject source);
    }
}
