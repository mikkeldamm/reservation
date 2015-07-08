namespace Seabook.Application.Events.Reservation
{
	public class ReservationCreated : DomainEvent
	{
		public string Id { get; set; }
	}
}