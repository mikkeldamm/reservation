using Seabook.Application.Domain;

namespace Seabook.Application.Commands
{
	public abstract class Command<TAggregateRoot> where TAggregateRoot : AggregateRoot, new()
	{
		public string AggregateId { get; set; }

		protected Command(string aggregateId)
		{
			AggregateId = aggregateId;
		}

		public abstract void Execute(TAggregateRoot aggregate);
	}
}