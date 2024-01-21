namespace Gamebook.Custom;

[AttributeUsage(AttributeTargets.Field)]
public class EnumDisplayNameAttribute : Attribute
{
    public string DisplayName { get; private set; }

    public EnumDisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}


public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var enumMember = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
        if (enumMember == null) return enumValue.ToString();
        return enumMember.GetCustomAttributes(typeof(EnumDisplayNameAttribute), false).FirstOrDefault() is EnumDisplayNameAttribute attribute ? attribute.DisplayName :
            // Return the default name if no attribute is found
            enumValue.ToString();
    }
}