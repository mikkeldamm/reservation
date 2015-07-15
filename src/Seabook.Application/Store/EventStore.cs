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

		public EventStore(IPublishEvents eventPublisher)
		{
			_eventPublisher = eventPublisher;
		}

		// Only public to demonstrate events being saved
		private readonly Dictionary<string, List<DomainEvent>> _inMemoryEventStore = new Dictionary<string, List<DomainEvent>>();
//		private readonly Dictionary<string, List<DomainEvent>> _inMemoryEventStore = new Dictionary<string, List<DomainEvent>>
//		{
//			{
//				"reservationId1234",
//				new List<DomainEvent>
//				{
//					new ReservationCreated { Id = "reservationId1234" },
//					new ReservationOutboundDateTimeChanged { DateTime = DateTime.Now.AddDays(1) },
//					new ReservationOutboundDateTimeChanged { DateTime = DateTime.Now.AddDays(2) },
//				}
//			}
//		};

		public void Save(string aggregateId, IEnumerable<DomainEvent> domainEvents)
		{
			List<DomainEvent> eventDescriptors;
			
			if (!_inMemoryEventStore.TryGetValue(aggregateId, out eventDescriptors))
			{
				eventDescriptors = new List<DomainEvent>();
				_inMemoryEventStore.Add(aggregateId, eventDescriptors);
			}

			foreach (var domainEvent in domainEvents)
			{
				eventDescriptors.Add(domainEvent);

				_eventPublisher.Publish(domainEvent);
			}
		}

		public bool Exists(string aggregateId)
		{
			return _inMemoryEventStore.ContainsKey(aggregateId);
		}
		
		public List<DomainEvent> Load(string aggregateId)
		{
			if (!_inMemoryEventStore.ContainsKey(aggregateId))
				throw new ArgumentException($"Could not find aggregate event by id [{aggregateId}]");

			var domainEvents = _inMemoryEventStore[aggregateId];
			
			foreach (var domainEvent in domainEvents)
			{
				_eventPublisher.Publish(domainEvent);
			}

			return domainEvents;
		}
	}
}