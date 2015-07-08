using System.Collections.Generic;
using System.Linq;
using Seabook.Application.Events;
using Seabook.Application.Events.Reservation;
using Seabook.Application.View;
using Seabook.WebApi.Models;

namespace Seabook.WebApi.Views
{
	public interface IGetAllReservationsView
	{
		List<ReservationViewModel> Reservations { get; set; }
	}

	public class GetAllReservationsView : IGetAllReservationsView, IView, 
		ISubscribeTo<ReservationCreated>, 
		ISubscribeTo<ReservationOutboundDateTimeChanged>,
		ISubscribeTo<ReservationDeleted>
	{
		public List<ReservationViewModel> Reservations { get; set; }

		public GetAllReservationsView()
		{
			Reservations = new List<ReservationViewModel>();
		}

		public void Handle(ReservationCreated domainEvent)
		{
			var reservation = Reservations.SingleOrDefault(r => r.Id == domainEvent.Id);
			if (reservation == null)
			{
				reservation = new ReservationViewModel();
				Reservations.Add(reservation);
			}

			reservation.Id = domainEvent.Id;
		}

		public void Handle(ReservationOutboundDateTimeChanged domainEvent)
		{
			var reservation = Reservations.SingleOrDefault(r => r.Id == domainEvent.AggregateRootId);
			if (reservation != null)
			{
				reservation.OutboundDateTime = domainEvent.DateTime;
			}
		}

		public void Handle(ReservationDeleted domainEvent)
		{
			Reservations.RemoveAll(r => r.Id == domainEvent.Id);
		}
	}
}