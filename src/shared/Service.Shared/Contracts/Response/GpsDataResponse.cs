using System;
using System.Text.Json.Serialization;

namespace Service.Shared.Contracts.Response
{
	public class GpsDataResponse
	{
		[JsonPropertyName("latitude")]
		public float Latitude { get; set; }

		[JsonPropertyName("longitude")]
		public float Longitude { get; set; }

		[JsonPropertyName("date")]
		public DateTime Date { get; set; }
	}
}
