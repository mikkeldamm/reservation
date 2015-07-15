using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Seabook.Application.Commands;
using Seabook.Application.Commands.Reservation;
using Seabook.WebApi.Models;
using Seabook.WebApi.Views;

namespace Seabook.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
	    private readonly IProcessCommands _commandProcessor;
	    private readonly IGetAllReservationsView _getAllReservationsView;

	    public ReservationController(IProcessCommands commandProcessor, IGetAllReservationsView getAllReservationsView)
	    {
		    _commandProcessor = commandProcessor;
		    _getAllReservationsView = getAllReservationsView;
	    }

        [HttpGet]
        public IEnumerable<ReservationViewModel> Get()
        {
	        return _getAllReservationsView.Reservations;
        }
		
        [HttpGet("{id}")]
        public ReservationViewModel Get(string id)
        {
			var reservations = _getAllReservationsView.Reservations;

			return reservations.SingleOrDefault(r => r.Id == id);
        }
        
        [HttpPost("{id}")]
        public void Post(string id, DateTimeOffset outboundDateTime)
        {
			_commandProcessor.Process(new CreateReservationCommand(id));
			_commandProcessor.Process(new ChangeReservationOutboundDateCommand(id) { DateTime = outboundDateTime.DateTime });
		}

		[HttpPut("{id}")]
		public void Put(string id, DateTimeOffset outboundDateTime)
		{
			_commandProcessor.Process(new ChangeReservationOutboundDateCommand(id) { DateTime = outboundDateTime.DateTime });
		}

		[HttpDelete("{id}")]
        public void Delete(string id)
		{
			_commandProcessor.Process(new DeleteReservationCommand(id));
		}
    }
}
