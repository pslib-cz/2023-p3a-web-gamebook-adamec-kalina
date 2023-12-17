
namespace Gamebook.Interfaces;

public interface ISessionHelper
{
    string GetString(string key);
    void SetString(string key, string value);
    void SetSessionDefaultState();

}