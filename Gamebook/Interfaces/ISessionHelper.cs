
namespace Gamebook.Interfaces;

public interface ISessionHelper
{
    string? GetString(string key);
    void SetString(string key, string value);
    int? GetInt(string key);
    void SetInt(string key, int value);
    void SetSessionDefaultState();

}