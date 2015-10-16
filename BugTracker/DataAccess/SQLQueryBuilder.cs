using BugTracker.Common;
using BugTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BugTracker.DataAccess
{
    public class SQLQueryBuilder
    {

        // (id:(?<ID>\d+))?(issuetype:(?<IssueType>\w+))?(createdby:(?<UserCreated>\w+))?(closedby:(?<UserClosed>\w+))?((description:|desc:)(?<Description>\w+))?(state:(?<State>\w+))?(priority:(?<Priority>\w+))?(severity:(?<Severity>\w+))?(contact:(?<Contact>\w+))?
        private static string groupMatch = @"(id:(?<ID>\d+))?" +
                                           @"(type:(?<IssueType>\w+))?" +
                                           @"(createdby:(?<UserCreated>\w+))?" +
                                           @"(closedby:(?<UserClosed>\w+))?" +
                                           @"((description:|desc:)(?<Description>\w+))?" +
                                           @"(state:(?<IssueState>\w+))?" +
                                           @"(priority:(?<Priority>\w+))?" +
                                           @"(severity:(?<Severity>\w+))?" +
                                           @"(contact:(?<PlantContact>\w+))?";

        public static string BuildSQLQuery(string filter)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.AppendLine("SELECT * FROM AllIssues ");

            if (!String.IsNullOrWhiteSpace(filter))
                sqlQuery.AppendLine(GetWhereClauseInQuery(filter));

            return sqlQuery.ToString();
        }

        private static string GetWhereClauseInQuery(string filter)
        {
            StringBuilder sqlQuery = new StringBuilder();

            MatchCollection match = Regex.Matches(filter, groupMatch, RegexOptions.IgnoreCase);
            var sqlFields = GetSQLFieldNames();
            string groupedWhereQuery = GetGroupedFilters(sqlFields, match);

            var generalFilters = GetUngroupedFilters(filter);
            string generalWhereQuery = GetGeneralFilters(sqlFields, generalFilters);

            sqlQuery.AppendLine("WHERE ");
            sqlQuery.Append(groupedWhereQuery);
            sqlQuery.Append(groupedWhereQuery.Length > 0 && generalWhereQuery.Length > 0 ? " AND " : "");
            sqlQuery.Append(generalWhereQuery);

            return sqlQuery.ToString();
        }

        private static string GetGroupedFilters(List<string> sqlFields, MatchCollection matchCollection)
        {
            var builder = new StringBuilder();

            foreach (Match match in matchCollection)
            {
                foreach (string field in sqlFields)
                {
                    if (match.Groups[field].Success)
                    {
                        if (builder.Length > 0) builder.Append(" AND ");

                        if (!String.IsNullOrEmpty(match.Groups[field].Value))
                            builder.Append(field + " LIKE '%" + match.Groups[field].Value.Replace("_", " ") + "%'");
                            // Make sure to replace _ with spaces
                    }
                }
            }

            return builder.ToString();
        }

        private static string GetGeneralFilters(List<string> sqlFields, List<string> generalFilters)
        {
            var builder = new StringBuilder();

            if (generalFilters.Count == 0) return "";

            builder.Append("(");

            foreach (string filter in generalFilters)
            {
                foreach (string field in sqlFields)
                {
                    builder.AppendLine(field + " LIKE '%" + filter + "%' OR ");
                }

                builder.Remove(builder.Length - 5, 4).Append(") AND (");
            }

            builder.Length -= 6;

            return builder.ToString();
        }

        private static List<string> GetSQLFieldNames()
        {
            var sqlFields = new List<string>();

            foreach (FieldInfo field in typeof(Issue).GetFields())
            {
                // Get all custom attribute to a dictionary
                var attributes = typeof(Issue).GetField(field.Name)
                                              .GetCustomAttributes(false)
                                              .ToDictionary(a => a.GetType().Name, a => a);

                // If this property is searchable, add to the list of fields
                if (IsFieldSearchable(attributes))
                {
                    sqlFields.Add(GetFieldName(attributes));
                }
            }

            return sqlFields;
        }

        private static List<string> GetUngroupedFilters(string filter)
        {
            return Regex.Replace(filter, groupMatch, "", RegexOptions.IgnoreCase).
                            Trim().Split(' ').Where((x) => !String.IsNullOrWhiteSpace(x)).ToList();
        }

        private static bool IsFieldSearchable(Dictionary<string, object> attr)
        {
            return ((SearchAttribute)attr["SearchAttribute"]).Search;
        }

        private static string GetFieldName(Dictionary<string, object> attr)
        {
            return ((SQLFieldAttribute)attr["SQLFieldAttribute"]).SQLField;
        }
    }
}
