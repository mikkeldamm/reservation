using System.Collections.Generic;
using Seabook.Application.Domain;
using Seabook.Application.Events;
using Seabook.Application.Store;

namespace Seabook.Application.Commands
{
	public interface IProcessCommands
	{
		void Process<TAggregateRoot>(Command<TAggregateRoot> command) where TAggregateRoot : AggregateRoot, new();
	}

	public class CommandProcessor : IProcessCommands
	{
		private readonly IEventStore _eventStore;
		private readonly IAggregateFactory _aggregateFactory;

		public CommandProcessor(IEventStore eventStore, IAggregateFactory aggregateFactory)
		{
			_eventStore = eventStore;
			_aggregateFactory = aggregateFactory;
		}

		public void Process<TAggregateRoot>(Command<TAggregateRoot> command) where TAggregateRoot : AggregateRoot, new()
		{
			var aggregateId = command.AggregateId;
			var aggregate = _aggregateFactory.Get<TAggregateRoot>(aggregateId);
			if (aggregate == null)
			{
				aggregate = _aggregateFactory.Create<TAggregateRoot>(aggregateId);

				var eventHistory = new List<DomainEvent>();

				if (_eventStore.Exists(aggregateId))
				{
					// Get all events for aggregate here from eventstore
					eventHistory = _eventStore.Load(aggregateId);
				}

				// Load all events
				aggregate.LoadsFromHistory(eventHistory);
			}

			// Execute command
			command.Execute(aggregate);
			
			// Get uncommitted events
			var uncommittedEvents = aggregate.GetUncommittedChanges();

			// Save uncommitted events to eventstore
			_eventStore.Save(aggregateId, uncommittedEvents);

			// Clear uncommitted changes
			aggregate.MarkChangesAsCommitted();
		}
	}
}