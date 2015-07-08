using System;

namespace Seabook.Application.Commands.Reservation
{
	public class ChangeReservationOutboundDateCommand : Command<Domain.Reservation.Reservation>
	{
		public DateTime DateTime { get; set; }

		public ChangeReservationOutboundDateCommand(string id) : base(id)
		{
		}

		public override void Execute(Domain.Reservation.Reservation aggregate)
		{
			aggregate.ChangeOutboundDateTime(DateTime);
		}
	}
}