using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.DependencyInjection;
using Seabook.Application.Commands;
using Seabook.Application.Domain;
using Seabook.Application.Events.Reservation;
using Seabook.Application.Store;
using Seabook.WebApi.Views;

namespace Seabook.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }
		
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
			
			// Setup factory, delegator (bus) and store
			var aggregateFactory = new AggregateFactory();
			var eventDelegator = new EventDelegator();
			var eventStore = new EventStore(eventDelegator);

			// Create new commandprocessor and add it to IoC
			SetupCommandProcessor(services, eventStore, aggregateFactory);

			// Create new view for getAllReservations and add it to IoC
			SetupViews(services, eventDelegator);
        }

		private static void SetupCommandProcessor(IServiceCollection services, EventStore eventStore, AggregateFactory aggregateFactory)
	    {
		    var commandProcessor = new CommandProcessor(eventStore, aggregateFactory);

		    services.AddSingleton<IProcessCommands>(provider => commandProcessor);
		}

		private static void SetupViews(IServiceCollection services, IRegisterEvents eventDelegator)
		{
			var getAllReservationsView = new GetAllReservationsView();

			services.AddSingleton<IGetAllReservationsView>(provider => getAllReservationsView);
			
			RegisterViewSubscribtions(eventDelegator, getAllReservationsView);
		}

		private static void RegisterViewSubscribtions(IRegisterEvents eventDelegator, GetAllReservationsView getAllReservationsView)
		{
			eventDelegator.Register<ReservationCreated>(getAllReservationsView.Handle);
			eventDelegator.Register<ReservationOutboundDateTimeChanged>(getAllReservationsView.Handle);
			eventDelegator.Register<ReservationDeleted>(getAllReservationsView.Handle);
		}


		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
			app.UseMvc();

            // Add the following route for porting Web API 2 controllers.
            // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
        }
    }
}
