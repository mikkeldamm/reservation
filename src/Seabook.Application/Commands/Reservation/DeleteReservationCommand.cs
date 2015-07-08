namespace Seabook.Application.Commands.Reservation
{
	public class DeleteReservationCommand : Command<Domain.Reservation.Reservation>
	{
		public DeleteReservationCommand(string id) : base(id)
		{
		}

		public override void Execute(Domain.Reservation.Reservation aggregate)
		{
			aggregate.Delete();
		}
	}
}