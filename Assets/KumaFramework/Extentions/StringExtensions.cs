using System.Collections.Generic;

public static class StringExtensions
{
    public static string FormatBy(this string template, Dictionary<string, object> values)
    {
        foreach (var pair in values)
        {
            template = template.Replace($"{{{pair.Key}}}", pair.Value?.ToString() ?? "");
        }
        return template;
    }
}