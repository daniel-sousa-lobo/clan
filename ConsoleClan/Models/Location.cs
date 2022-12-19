namespace ConsoleClan.Models
{
	public record Location
	{
		public int? id { get; set; }
		public string? name { get; set; }
		public bool? isCountry { get; set; }
		public string? countryCode { get; set; }
	}
}
