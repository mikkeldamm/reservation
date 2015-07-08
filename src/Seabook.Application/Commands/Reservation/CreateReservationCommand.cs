namespace Seabook.Application.Commands.Reservation
{
	public class CreateReservationCommand : Command<Domain.Reservation.Reservation>
	{
		public CreateReservationCommand(string id) : base(id)
		{
		}

		public override void Execute(Domain.Reservation.Reservation aggregate)
		{
			aggregate.Create();
		}
	}
}