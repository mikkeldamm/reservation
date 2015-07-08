using System.Collections.Generic;

namespace Seabook.Application.Domain
{
	public interface IAggregateFactory
	{
		TAggregate Get<TAggregate>(string aggregateId) where TAggregate : AggregateRoot;
		TAggregate Create<TAggregate>(string aggregateId) where TAggregate : AggregateRoot, new();
	}

	public class AggregateFactory : IAggregateFactory
	{
		private readonly Dictionary<string, object> _aggregates = new Dictionary<string, object>();

		public TAggregate Get<TAggregate>(string aggregateId) where TAggregate : AggregateRoot
		{
			if (_aggregates.ContainsKey(aggregateId))
			{
				return _aggregates[aggregateId] as TAggregate;
			}

			return null;
		}

		public TAggregate Create<TAggregate>(string aggregateId) where TAggregate : AggregateRoot, new()
		{
			var newAggregate = new TAggregate();
			newAggregate.Create(aggregateId);
			_aggregates.Add(aggregateId, newAggregate);

			return newAggregate;
		}
	}
}