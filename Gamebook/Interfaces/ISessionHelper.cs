
namespace Gamebook.Interfaces;

public interface ISessionHelper
{
    public string GetString(string key);
    public void SetString(string key, string value);
}