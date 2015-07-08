using System.Collections.Generic;
using Seabook.Application.Events;

namespace Seabook.Application.Domain
{
	public class AggregateRoot 
	{
		public string Id { get; internal set; }

		private readonly List<DomainEvent> _changes = new List<DomainEvent>();

		public IEnumerable<DomainEvent> GetUncommittedChanges()
		{
			return _changes;
		}

		public void MarkChangesAsCommitted()
		{
			_changes.Clear();
		}

		public void LoadsFromHistory(IEnumerable<DomainEvent> history)
		{
			foreach (var e in history)
			{
				// We only apply emitted event, we don't save it 
				// to changes when loading from history
				ApplyEmittedEvent(e);
			}
		}

		internal void Create(string id)
		{
			Id = id;
		}

		public void Emit<T>(T e) where T: DomainEvent
		{
			e.AggregateRootId = Id;

			ApplyEmittedEvent(e);
			AddEmittedEvent(e);
		}

		private void ApplyEmittedEvent<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent
		{
			var emitter = this as IEmit<TDomainEvent>;
			emitter?.Apply(domainEvent);
		}

		private void AddEmittedEvent(DomainEvent e)
		{
			_changes.Add(e);
		}
	}
}