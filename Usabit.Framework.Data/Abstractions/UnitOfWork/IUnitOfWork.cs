using System.Collections.Generic;
using System.Threading.Tasks;
using Usabit.Framework.Seedworks.Concrete.Enums;

namespace Usabit.Framework.Data.Abstractions.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();
        Task CommitAsync(EventDispatchMode? dispatchMode = EventDispatchMode.None);
        Task BulkInsertAsync(IEnumerable<object> entities);
        Task BulkUpdateAsync(IEnumerable<object> entities);
        Task DetachEntriesAsync();
        Task CleanEntriesAsync();
    }
}
