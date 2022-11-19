using Fluxor;
using Fluxor.Blazor.Web.Components;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Dashboard.Frontend.Stores.States;
using System.Collections.Generic;
using Dashboard.Frontend.Interfaces;
using System.Linq;

namespace Dashboard.Frontend.Pages
{
	public partial class LiveMap : FluxorComponent
	{
		[Inject]
		public IState<DeviceState> DeviceState { get; set; }	
		
		[Inject]
		public IDeviceService DeviceService { get; set; }

		private GoogleMap map;
		private MapOptions mapOptions;
		private DirectionsRenderer dirRend;
		private string durationTotalString;
		private string distanceTotalString;

		private List<DirectionsWaypoint> waypoints = new List<DirectionsWaypoint>();

		protected override void OnInitialized()
		{
			mapOptions = new MapOptions()
			{
				Zoom = 18,
				Center = new LatLngLiteral()
				{
					Lat = 50,
					Lng = 4
				},
				MapTypeId = MapTypeId.Roadmap
			};

			DeviceState.StateChanged += (s, e) =>
			{
				var stateMapped = (DeviceState)s;

				mapOptions = new MapOptions()
				{
					Zoom = 13,
					Center = new LatLngLiteral()
					{
						Lat = stateMapped.DevicePosition.Latitude,
						Lng = stateMapped.DevicePosition.Longitude
					},
					MapTypeId = MapTypeId.Roadmap
				};

				waypoints.Add(new DirectionsWaypoint()
				{
					Location = new LatLngLiteral()
					{
						Lat = stateMapped.DevicePosition.Latitude,
						Lng = stateMapped.DevicePosition.Longitude
					}
				});

				StateHasChanged();
			};
		}

		private async Task OnAfterInitAsync()
		{
			dirRend = await DirectionsRenderer.CreateAsync(map.JsRuntime, new DirectionsRendererOptions()
			{
				Map = map.InteropObject
			});

			var points = await DeviceService.GetDeviceGpsAsync("TB-06-FAF7C630");

			foreach(var pts in points)
			{
				waypoints.Add(new DirectionsWaypoint()
				{
					Location = new LatLngLiteral()
					{
						Lat = pts.Latitude,
						Lng = pts.Longitude
					}
				});
			}

			DirectionsRequest dr = new DirectionsRequest();
			dr.Origin = waypoints.First().Location;
			dr.Destination = waypoints.Last().Location;
			dr.Waypoints = waypoints;
			dr.TravelMode = TravelMode.Driving;
			var directionsResult = await dirRend.Route(dr);

			foreach (var route in directionsResult.Routes.SelectMany(x => x.Legs))
			{
				durationTotalString += route.Duration.Text;
				distanceTotalString += route.Distance.Text;
			}
		}
	}
}
