using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for ExtensionMethods
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Remove all invalid Html tags
    /// </summary>
    /// <param name="htmlContent">Html content</param>
    /// <returns>The valid html tags</returns>
    public static string StripInvalidHtml(this string htmlContent)
    {
        var whiteList = new string[] { "br", "p", "span", "i", "em", "u", "strong", "b" };

        htmlContent = Regex.Replace(htmlContent, @"<\/?([a-z][a-z0-9]*)\b[^>]*>", delegate(Match match)
        {
            var tag = match.Groups[1].Value;
            var fullHtml = match.Groups[0].Value;
            if (whiteList.Any(t => t.Equals(tag, StringComparison.OrdinalIgnoreCase)))
            {
                // Igmore close tag
                if (fullHtml.Equals(string.Format("</{0}>", tag), StringComparison.OrdinalIgnoreCase))
                {
                    return fullHtml;
                }

                return StripAttributes(fullHtml, tag);
            }
            return string.Empty;
        }, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        // Remove word comment and mutiple spaces if any
        htmlContent = Regex.Replace(htmlContent, @"<!--.*-->", string.Empty);
        htmlContent = Regex.Replace(htmlContent, @"\s+", " ");

        return htmlContent;
    }

    /// <summary>
    /// Strip invalid attributes
    /// </summary>
    /// <param name="html">The full html tag</param>
    /// <param name="tag">The tag name</param>
    /// <returns>Valid attributes</returns>
    private static string StripAttributes(string html, string tag)
    {
        var whiteListAttStyles = new string[] { "color", "background-color", "text-decoration" };

        var match = Regex.Match(html, @"style=[""'](?<val>.*?)[""']",
                                    RegexOptions.IgnoreCase | RegexOptions.Multiline);

        if (!match.Success)
            return string.Format("<{0}>", tag);

        var styles = string.Empty;
        if (!string.IsNullOrEmpty(match.Groups["val"].Value))
        {
            styles = match.Groups["val"].Value;
            var styleValue = styles.EndsWith(";") ? styles : styles + ";";
            var acceptedStyles = styleValue
                .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => whiteListAttStyles.Any(s1 => s.StartsWith(s1, StringComparison.OrdinalIgnoreCase)));
            if (acceptedStyles.Any())
            {
                styles = string.Format("style=\"{0}\"", string.Join(";", acceptedStyles.ToArray()) + ";");
            }
            else
            {
                styles = string.Empty;
            }
        }
        return string.IsNullOrEmpty(styles)
            ? string.Format("<{0}>", tag)
            : string.Format("<{0} {1}>", tag, styles);
    }
    public static T ToEnum<T>(this string value, T defaultValue) where T : struct
    {
        try
        {
            T enumValue;
            if (!Enum.TryParse(value, true, out enumValue))
            {
                return defaultValue;
            }
            return enumValue;
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public static decimal? SafeGetDecimal(this object value)
    {
        if (value == null || value == DBNull.Value)
            return null;

        decimal number;
        return decimal.TryParse(value.ToString(), out number) ? number : (decimal?)null;
    }
}