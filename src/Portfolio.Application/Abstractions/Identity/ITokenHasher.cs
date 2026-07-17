namespace Portfolio.Application.Abstractions.Identity;

public interface ITokenHasher
{
    string Hash(string token);
}
