using System;
using System.Collections.Generic;
using Seabook.Application.Events;

namespace Seabook.Application.Store
{
	public interface IEventStore
	{
		void Save(string aggregateId, IEnumerable<DomainEvent> domainEvents);
		bool Exists(string aggregateId);
		List<DomainEvent> Load(string aggregateId);
	}

	public class EventStore : IEventStore
	{
		private readonly IPublishEvents _eventPublisher;
		private readonly Dictionary<string, List<DomainEvent>> _inMemoryEventStore = new Dictionary<string, List<DomainEvent>>();

		public EventStore(IPublishEvents eventPublisher)
		{
			_eventPublisher = eventPublisher;
		}

		public void Save(string aggregateId, IEnumerable<DomainEvent> domainEvents)
		{
			List<DomainEvent> eventDescriptors;
			
			if (!_inMemoryEventStore.TryGetValue(aggregateId, out eventDescriptors))
			{
				eventDescriptors = new List<DomainEvent>();
				_inMemoryEventStore.Add(aggregateId, eventDescriptors);
			}

			eventDescriptors.AddRange(domainEvents);
			
			PublishDomainEvents(domainEvents);
		}
		
		public List<DomainEvent> Load(string aggregateId)
		{
			if (!Exists(aggregateId))
			{
				throw new ArgumentException($"Could not find aggregate event by id [{aggregateId}]");
			}

			var domainEvents = _inMemoryEventStore[aggregateId];
			
			PublishDomainEvents(domainEvents);

			return domainEvents;
		}
		
		public bool Exists(string aggregateId)
		{
			return _inMemoryEventStore.ContainsKey(aggregateId);
		}
		
		private void PublishDomainEvents(IEnumerable<DomainEvent> domainEvents) 
		{
			foreach (var domainEvent in domainEvents)
			{
				_eventPublisher.Publish(domainEvent);
			}
		}
	}
}