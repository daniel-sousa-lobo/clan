namespace ConsoleClan.Interfaces
{
	public interface IClash
	{
		Task ProcessAsync(AuthenticationTokeReference authenticationToke, string? fileName);
	}
}
