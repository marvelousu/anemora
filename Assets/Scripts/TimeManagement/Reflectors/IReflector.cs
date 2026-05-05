using System.Collections.Generic;
using Anemora.Data;

namespace Anemora.TimeManagement.Reflectors
{
    public interface IReflector
    {
        bool TryReflect(ActionRecordEntry entry, ActionRecordCatalog catalog);
    }

    public interface IReflectedStateRestorer
    {
        int RestoreReflected(IEnumerable<ActionRecordEntry> entries, ActionRecordCatalog catalog);
    }
}
