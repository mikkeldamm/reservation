using System;
using System.Collections.Generic;
using Seabook.Application.Events;

namespace Seabook.Application.Store
{
	public interface IRegisterEvents
	{
		void Register<TDomainEvent>(Action<TDomainEvent> handler) where TDomainEvent : DomainEvent;
	}

	public interface IPublishEvents
	{
		void Publish<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent;
	}

	public class EventDelegator : IRegisterEvents, IPublishEvents
	{
		private readonly Dictionary<Type, List<Action<DomainEvent>>> _handlers = new Dictionary<Type, List<Action<DomainEvent>>>();

		public void Register<TDomainEvent>(Action<TDomainEvent> handler) where TDomainEvent : DomainEvent
		{
			List<Action<DomainEvent>> handlers;

			if (!_handlers.TryGetValue(typeof(TDomainEvent), out handlers))
			{
				handlers = new List<Action<DomainEvent>>();
				_handlers.Add(typeof(TDomainEvent), handlers);
			}

			handlers.Add((x => handler((TDomainEvent)x)));
		}

		public void Publish<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent
		{
			List<Action<DomainEvent>> handlers;

			if (!_handlers.TryGetValue(domainEvent.GetType(), out handlers))
			{
				return;
			}

			foreach (var handler in handlers)
			{
				handler(domainEvent);
			}
		}
	}
}