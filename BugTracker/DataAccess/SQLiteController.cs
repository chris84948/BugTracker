using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using BugTracker.Model;
using BugTracker.ViewModels;

namespace BugTracker.DataAccess
{
    /// <summary>
    /// Controller class for the SQLite database
    /// Exposes the necessary methods
    /// </summary>
    class SQLiteController : IDataAccess
    {
        private const string DB_FILENAME = "bugtracker.db";
        private string CONNECTION_STRING = String.Format("Data Source={0};Version=3;", DB_FILENAME);
        private SQLiteConnection conn;

        /// <summary>
        /// Constructor for controller
        /// </summary>
        public SQLiteController()
        {
            if (!File.Exists(DB_FILENAME))
                CreateDatabase();
        }

        /// <summary>
        /// Creates the database and table if necessary
        /// </summary>
        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(DB_FILENAME);

            using (conn = new SQLiteConnection(CONNECTION_STRING))
            {
                conn.Open();

                new SQLiteCommand(SQLFixedQueries.CreateTables(), conn).ExecuteNonQuery();
                new SQLiteCommand(SQLFixedQueries.CreateViews(), conn).ExecuteNonQuery();
                new SQLiteCommand(SQLFixedQueries.CreateDefaultData(), conn).ExecuteNonQuery();
                new SQLiteCommand(SQLFixedQueries.InsertTestData(), conn).ExecuteNonQuery(); //TODO remove this when releasing
            }
        }

        List<IssueViewModel> IDataAccess.GetAllIssues(string filter)
        {
            using (conn = new SQLiteConnection(CONNECTION_STRING))
            {
                conn.Open();

                var issues = new List<IssueViewModel>();

                var reader = new SQLiteCommand(SQLQueryBuilder.BuildSQLQuery(filter), conn)
                                                    .ExecuteReader();

                while (reader.Read())
                    issues.Add(ReadIssueFromSQL(reader));

                return issues;
            }
        }

        IssueViewModel IDataAccess.GetIssue(int id)
        {
            using (conn = new SQLiteConnection(CONNECTION_STRING))
            {
                conn.Open();

                var issue = new IssueViewModel();

                var reader = new SQLiteCommand(SQLFixedQueries.SelectIssue(id), conn)
                                                    .ExecuteReader();

                while (reader.Read())
                    issue = ReadIssueFromSQL(reader);

                return issue;
            }
        }

        Bug IDataAccess.GetBug(int id)
        {
            using (conn = new SQLiteConnection(CONNECTION_STRING))
            {
                conn.Open();

                var bug = new Bug();

                var reader = new SQLiteCommand(SQLFixedQueries.SelectBug(id), conn)
                                                    .ExecuteReader();

                while (reader.Read())
                    bug = ReadBugFromSQL(reader);

                return bug;
            }
        }

        ChangeRequest IDataAccess.GetChangeRequest(int id)
        {
            using (conn = new SQLiteConnection(CONNECTION_STRING))
            {
                conn.Open();

                var changeRequest = new ChangeRequest();

                var reader = new SQLiteCommand(SQLFixedQueries.SelectChangeRequest(id), conn)
                                                    .ExecuteReader();

                while (reader.Read())
                    changeRequest = ReadChangeRequestFromSQL(reader);

                return changeRequest;
            }
        }

        int IDataAccess.SaveBug(Issue issue, Bug bug)
        {
            using (conn = new SQLiteConnection(CONNECTION_STRING))
            {
                conn.Open();

                if (issue.IssueID == 0 || !IssueExists(issue.IssueID))
                {
                    int ID = InsertIssueInDBAndGetID(issue, GetUserID(issue.UserCreated));
                    InsertBugInDB(bug, ID);
                    return ID;
                }
                else
                {
                    UpdateIssueInDB(issue, GetUserID(issue.UserClosed));
                    UpdateBugInDB(bug);
                    return issue.IssueID;
                }
            }
        }

        int IDataAccess.SaveChangeRequest(Issue issue, ChangeRequest changeRequest)
        {
            using (conn = new SQLiteConnection(CONNECTION_STRING))
            {
                conn.Open();

                if (issue.IssueID == 0 || !IssueExists(issue.IssueID))
                {
                    int ID = InsertIssueInDBAndGetID(issue, GetUserID(issue.UserCreated));
                    InsertChangeRequestInDB(changeRequest, ID);
                    return ID;
                }
                else
                {
                    UpdateIssueInDB(issue, GetUserID(issue.UserClosed));
                    UpdateChangeRequestInDB(changeRequest);
                    return issue.IssueID;
                }
            }
        }

        private int GetUserID(string user)
        {
            object userID = new SQLiteCommand(SQLFixedQueries.GetUserIDFromName(user), conn)
                                        .ExecuteScalar();

            if (userID != null)
            {
                return Convert.ToInt32(userID);
            }
            else
            {
                new SQLiteCommand(SQLFixedQueries.InsertUsername(user), conn)
                            .ExecuteNonQuery();
                return Convert.ToInt32(new SQLiteCommand(SQLFixedQueries.GetRowID(), conn)
                            .ExecuteScalar());
            }
        }

        private int InsertIssueInDBAndGetID(Issue issue, int userID)
        {
            new SQLiteCommand(SQLFixedQueries.InsertIssue(issue.Description,
                                                          userID,
                                                          issue.DateCreated.ToString(),
                                                          issue.PlantContact,
                                                          issue.IssueType,
                                                          issue.IssueState,
                                                          issue.IssueResolution,
                                                          issue.Priority,
                                                          issue.Severity),
                              conn).ExecuteNonQuery();

            return Convert.ToInt32(new SQLiteCommand(SQLFixedQueries.GetRowID(), conn)
                                                .ExecuteScalar());
        }

        private void UpdateIssueInDB(Issue issue, int userID)
        {
            new SQLiteCommand(SQLFixedQueries.UpdateIssue(issue.IssueID,
                                                          issue.Description,
                                                          userID,
                                                          issue.DateClosed.ToString(),
                                                          issue.PlantContact,
                                                          issue.IssueState,
                                                          issue.IssueResolution,
                                                          issue.Priority,
                                                          issue.Severity),
                              conn).ExecuteNonQuery();
        }

        private void InsertBugInDB(Bug bug, int issueID)
        {
            new SQLiteCommand(SQLFixedQueries.InsertBug(issueID,
                                                        bug.VersionFound,
                                                        bug.VersionFixed,
                                                        bug.DetailedDescription,
                                                        bug.StepsToReproduce,
                                                        bug.Workaround,
                                                        bug.Fix),
                              conn).ExecuteNonQuery();
        }

        private void UpdateBugInDB(Bug bug)
        {
            new SQLiteCommand(SQLFixedQueries.UpdateBug(bug.ID,
                                                        bug.VersionFound,
                                                        bug.VersionFixed,
                                                        bug.DetailedDescription,
                                                        bug.StepsToReproduce,
                                                        bug.Workaround,
                                                        bug.Fix),
                              conn).ExecuteNonQuery();
        }

        private void InsertChangeRequestInDB(ChangeRequest changeRequest, int issueID)
        {
            new SQLiteCommand(SQLFixedQueries.InsertChangeRequest(issueID,
                                                                  changeRequest.VersionImplemented,
                                                                  changeRequest.DetailedDescription,
                                                                  changeRequest.Justification),
                                conn).ExecuteNonQuery();
        }

        private void UpdateChangeRequestInDB(ChangeRequest changeRequest)
        {
            new SQLiteCommand(SQLFixedQueries.UpdateChangeRequest(changeRequest.ID,
                                                                  changeRequest.VersionImplemented,
                                                                  changeRequest.DetailedDescription,
                                                                  changeRequest.Justification),
                                conn).ExecuteNonQuery();
        }

        private bool IssueExists(int issueID)
        {
            return Convert.ToInt32(new SQLiteCommand(SQLFixedQueries.DoesIssueExist(issueID), conn)
                                .ExecuteScalar()) > 0;
        }

        private IssueViewModel ReadIssueFromSQL(SQLiteDataReader reader)
        {
            var issue = new IssueViewModel();

            issue.IssueID = Convert.ToInt32(reader[0]);
            issue.Description = reader[1].ToString();
            issue.UserCreated = reader[2].ToString();
            issue.UserClosed = reader[3].ToString();
            issue.DateCreated = ConvertToNullableDateTime(reader[4]);
            issue.DateClosed = ConvertToNullableDateTime(reader[5]);
            issue.PlantContact = reader[6].ToString();
            issue.IssueType = Convert.ToInt32(reader[7]);
            issue.IssueState = Convert.ToInt32(reader[8]);
            issue.IssueResolution = Convert.ToInt32(reader[9]);
            issue.Priority = Convert.ToInt32(reader[10]);
            issue.Severity = Convert.ToInt32(reader[11]);

            return issue;
        }

        private Bug ReadBugFromSQL(SQLiteDataReader reader)
        {
            var bug = new Bug();

            bug.ID = Convert.ToInt32(reader[1]);
            bug.VersionFound = reader[2].ToString();
            bug.VersionFixed = reader[3].ToString();
            bug.DetailedDescription = reader[4].ToString();
            bug.StepsToReproduce = reader[5].ToString();
            bug.Workaround = reader[6].ToString();
            bug.Fix = reader[7].ToString();

            return bug;
        }

        private ChangeRequest ReadChangeRequestFromSQL(SQLiteDataReader reader)
        {
            var changeRequest = new ChangeRequest();

            changeRequest.ID = Convert.ToInt32(reader[1]);
            changeRequest.VersionImplemented = reader[2].ToString();
            changeRequest.DetailedDescription = reader[3].ToString();
            changeRequest.Justification = reader[4].ToString();

            return changeRequest;
        }

        private DateTime? ConvertToNullableDateTime(object datetime)
        {
            try
            {
                return Convert.ToDateTime(datetime);
            }
            catch
            {
                return null;
            }
        }
    }
}