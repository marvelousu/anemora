using Anemora.Data;

namespace Anemora.TimeManagement.Reflectors
{
    public interface IReflector
    {
        bool TryReflect(ActionRecordEntry entry, ActionRecordCatalog catalog);
    }
}
