using MediatR;
using System;
using System.Collections.Generic;
using Usabit.Framework.Seedworks.Concrete.Enums;

namespace Usabit.Framework.Seedworks.Domains.Abstraction
{
    public abstract class Entity
    {
        public int? RequestedHashCode { get; private set; }

        private int id;
        public virtual int Id { get => id; protected set => id = value; }

        private List<INotification> domainEvents;
        public List<INotification> DomainEvents => domainEvents;
        public OperationMode? Operation { get; private set; }

        public bool IsTransient() => this.Id == default(int);

        public void AddDomainEvent(INotification @event)
        {
            domainEvents = domainEvents ?? new List<INotification>();
            domainEvents.Add(@event);
        }

        public void RemoveDomainEvent(INotification @event)
        {
            domainEvents?.Remove(@event);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!RequestedHashCode.HasValue)
                    RequestedHashCode = this.Id.GetHashCode() ^ 31;

                return RequestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        public void SetOperationMode(OperationMode opration) => Operation = opration;

        public void SetCreateOperationMode() => Operation = OperationMode.Create;
        public void SetRemoveOperationMode() => Operation = OperationMode.Remove;
        public void SetUpdateOperationMode() => Operation = OperationMode.Update;
        public void SetNoChangesOperationMode() => Operation = OperationMode.NoChanges;
    }
}
