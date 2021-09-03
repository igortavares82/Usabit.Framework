using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usabit.Framework.Data.Abstractions.UnitOfWork;
using Usabit.Framework.Seedworks.Concrete.Enums;
using Usabit.Framework.Seedworks.Domains.Abstraction;

namespace Usabit.Framework.Data.Concrete.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext DbContext { get; }
        private IMediator Mediator { get; }

        public UnitOfWork(DbContext dbContext) 
        {
            DbContext = dbContext;
        }

        public UnitOfWork(DbContext dbContext, IMediator mediator)
        {
            DbContext = dbContext;
            Mediator = mediator;
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        public async Task CommitAsync(EventDispatchMode? dispatchTime)
        {
            if (dispatchTime == EventDispatchMode.BeforeSaveChanges)
                await DispatchNotioficationsAsync();

            await DbContext.SaveChangesAsync();

            if (dispatchTime == EventDispatchMode.AfterSaveChanges)
                await DispatchNotioficationsAsync();
        }

        public async Task BulkInsertAsync(IEnumerable<object> entities) 
        {
            if (entities == null || entities.Count() == 0)
                return;

            Type type = entities.First().GetType(); 
            await DbContext.BulkInsertAsync(type, entities);
        }

        public async Task BulkUpdateAsync(IEnumerable<object> entities)
        {
            if (entities == null || entities.Count() == 0)
                return;

            Type type = entities.First().GetType();
            await DbContext.BulkUpdateAsync(type, entities);
        }

        private async Task DispatchNotioficationsAsync() 
        {
            if (Mediator != null)
            {
                List<INotification> notifications = await GetNotificationsAsync();
                notifications.ToList().ForEach(it => Mediator.Publish(it));
            }
        }


        private async Task<List<INotification>> GetNotificationsAsync() 
        {
            return DbContext.ChangeTracker
                            .Entries()
                            .Select(it => it.Entity as Entity)
                            .ToList()
                            .Where(it => it.DomainEvents != null)
                            .SelectMany(it => it.DomainEvents)
                            .ToList();

        }

        public async Task CleanEntriesAsync() => this.DbContext.ChangeTracker?.Clear();

        public async Task DetachEntriesAsync() 
        {
            var entries = this.DbContext
                              .ChangeTracker?
                              .Entries();

            
            entries.ToList().ForEach(it => DbContext.Entry(it).State = EntityState.Detached);
        }
    }
}
