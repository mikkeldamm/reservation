namespace Seabook.Application.Events
{
	public class DomainEvent
	{
		public string AggregateRootId { get; set; }
	}


	public interface IEmit<in TDomainEvent> where TDomainEvent : DomainEvent
	{
		void Apply(TDomainEvent domainEvent);
	}


	public interface ISubscribeTo
	{
	}
	public interface ISubscribeTo<in TDomainEvent> : ISubscribeTo where TDomainEvent : DomainEvent
	{
		void Handle(TDomainEvent domainEvent);
	}
}