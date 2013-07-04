using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

namespace Aliencube.Utilities.Sql.RegularExpressions
{
    /// <summary>
    /// This represents the SQL regular expression extension class.
    /// </summary>
    public class Regex
    {
        /// <summary>
        /// Indicates whether the specified regular expression finds a match in the specified input string, using the specified matching options.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <param name="option">A combination of bitwise values that provide options for matching.</param>
        /// <returns>Returns <c>True</c>, if the regular expression finds a match; otherwise, returns <c>False</c>.</returns>
        /// <remarks>
        /// option value must be one of the following:
        ///     <list type="bullet">
        ///         <item><term>0</term><description>None</description></item>
        ///         <item><term>1</term><description>IgnoreCase</description></item>
        ///         <item><term>2</term><description>Multiline</description></item>
        ///         <item><term>4</term><description>ExplicitCapture</description></item>
        ///         <item><term>8</term><description>Compiled</description></item>
        ///         <item><term>16</term><description>Singleline</description></item>
        ///         <item><term>32</term><description>IgnorePatternWhitespace</description></item>
        ///         <item><term>64</term><description>RightToLeft</description></item>
        ///         <item><term>256</term><description>ECMAScript</description></item>
        ///         <item><term>512</term><description>CultureInvariant</description></item>
        ///     </list>
        /// </remarks>
        [SqlFunction(Name = "IsMatch", IsDeterministic = true, IsPrecise = true)]
        public static SqlBoolean IsMatch(SqlString input, SqlString pattern, SqlString option)
        {
            if (input.IsNull)
                return SqlBoolean.Null;

            if (pattern.IsNull)
                pattern = String.Empty;

            RegexOptions options;
            if (option.IsNull)
                options = RegexOptions.None;

            try
            {
                var segments = option.Value
                                     .Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(p => (RegexOptions)Convert.ToInt32(p))
                                     .ToList();
                options = segments[0];
                if (segments.Count > 1)
                    foreach (var segment in segments.Skip(1))
                        options |= segment;
            }
            catch
            {
                options = RegexOptions.None;
            }

            SqlBoolean matched = System.Text.RegularExpressions.Regex.IsMatch(input.Value, pattern.Value, options);
            return matched;
        }

        /// <summary>
        /// Replaces all strings that match a specified regular expression with a specified replacement string, within a specified input string. Specified options modify the matching operation.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <param name="replacement">The replacement string.</param>
        /// <param name="option">A combination of bitwise values that provide options for matching.</param>
        /// <returns>A new string that is identical to the input string, except that a replacement string takes the place of each matched string.</returns>
        /// <remarks>
        /// option value must be one of the following:
        ///     <list type="bullet">
        ///         <item><term>0</term><description>None</description></item>
        ///         <item><term>1</term><description>IgnoreCase</description></item>
        ///         <item><term>2</term><description>Multiline</description></item>
        ///         <item><term>4</term><description>ExplicitCapture</description></item>
        ///         <item><term>8</term><description>Compiled</description></item>
        ///         <item><term>16</term><description>Singleline</description></item>
        ///         <item><term>32</term><description>IgnorePatternWhitespace</description></item>
        ///         <item><term>64</term><description>RightToLeft</description></item>
        ///         <item><term>256</term><description>ECMAScript</description></item>
        ///         <item><term>512</term><description>CultureInvariant</description></item>
        ///     </list>
        /// </remarks>
        [SqlFunction(Name = "Replace", IsDeterministic = true, IsPrecise = true)]
        public static SqlString Replace(SqlString input, SqlString pattern, SqlString replacement, SqlString option)
        {
            if (input.IsNull)
                return SqlString.Null;

            if (pattern.IsNull)
                pattern = String.Empty;

            if (replacement.IsNull)
                replacement = String.Empty;

            RegexOptions options;
            if (option.IsNull)
                options = RegexOptions.None;

            try
            {
                var segments = option.Value
                                     .Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(p => (RegexOptions)Convert.ToInt32(p))
                                     .ToList();
                options = segments[0];
                if (segments.Count > 1)
                    foreach (var segment in segments.Skip(1))
                        options |= segment;
            }
            catch
            {
                options = RegexOptions.None;
            }

            SqlString result = System.Text.RegularExpressions.Regex.Replace(input.Value, pattern.Value, replacement.Value, options);
            return result;
        }
    }
}
