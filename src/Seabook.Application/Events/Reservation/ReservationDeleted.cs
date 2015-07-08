namespace Seabook.Application.Events.Reservation
{
	public class ReservationDeleted : DomainEvent
	{
		public string Id { get; set; }
	}
}