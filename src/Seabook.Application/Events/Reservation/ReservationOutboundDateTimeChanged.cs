using System;

namespace Seabook.Application.Events.Reservation
{
	public class ReservationOutboundDateTimeChanged : DomainEvent
	{
		public DateTime DateTime { get; set; }
	}
}