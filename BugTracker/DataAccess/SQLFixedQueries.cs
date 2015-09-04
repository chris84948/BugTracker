using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.DataAccess
{
    class SQLFixedQueries
    {
        public static string CreateTables()
        {
            return
                @"CREATE TABLE Severity (
                    ID INTEGER PRIMARY KEY,
                    Description TEXT
                );
                                 
                CREATE TABLE Priority (
                    ID INTEGER PRIMARY KEY,
                    Description TEXT
                );
                                 
                CREATE TABLE IssueType (
                    ID INTEGER PRIMARY KEY,
                    Description TEXT
                );
                                 
                CREATE TABLE IssueState (
                    ID INTEGER PRIMARY KEY,
                    Description TEXT
                );
                                 
                CREATE TABLE IssueResolution (
                    ID INTEGER PRIMARY KEY,
                    Description TEXT
                );
                                 
                CREATE TABLE Users (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT
                );
                                 
                CREATE TABLE Issues (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Description TEXT,
                    UserCreatedID INTEGER,
                    UserClosedID INTEGER,
                    DateCreated TEXT,
                    DateClosed TEXT,
                    PlantContact TEXT,
                    IssueTypeID INTEGER,
                    IssueStateID INTEGER,
                    IssueResolutionID INTEGER,
                    PriorityID INTEGER,
                    SeverityID INTEGER,
                                 
                    FOREIGN KEY (UserCreatedID) REFERENCES Users(ID),
                    FOREIGN KEY (UserClosedID) REFERENCES Users(ID),
                    FOREIGN KEY (IssueTypeID) REFERENCES IssueType(ID),
                    FOREIGN KEY (IssueStateID) REFERENCES IssueState(ID),
                    FOREIGN KEY (IssueResolutionID) REFERENCES IssueResolution(ID),
                    FOREIGN KEY (PriorityID) REFERENCES Priority(ID),
                    FOREIGN KEY (SeverityID) REFERENCES Severity(ID)
                );
                                 
                CREATE TABLE Bugs (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    IssueID INTEGER,
                    VersionFound TEXT,
                    VersionFixed TEXT,
                    DetailedDescription TEXT,
                    StepsToReproduce TEXT,
                    Workaround TEXT,
                    Fix TEXT,
                                 
                    FOREIGN KEY (IssueID) REFERENCES Issues(ID)
                );
                                 
                CREATE TABLE ChangeRequests (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    IssueID INTEGER,
                    VersionImplemented TEXT,
                    DetailedDescription TEXT,
                    Justification TEXT,
                                 
                    FOREIGN KEY (IssueID) REFERENCES Issues(ID)
                );";
        } 

        public static string CreateViews()
        {
            return
                @"CREATE VIEW AllIssues AS
                     SELECT  
                         issues.ID AS ID, 
                         issues.Description AS Description,
                         userCreated.Username AS UserCreated, 
                         userClosed.Username AS UserClosed,
                         issues.DateCreated AS DateCreated,
                         issues.DateClosed AS DateClosed,
                         issues.PlantContact AS PlantContact,
                         issueType.ID As IssueTypeID, 
                         issueState.ID AS IssueStateID,
                         issueResolution.ID AS IssueResolutionID,
                         priority.ID AS PriorityID,
                         severity.ID AS SeverityID,
                         issueType.Description AS IssueType,
                         issueState.Description AS IssueState,
                         issueResolution.Description AS IssueResolution,
                         priority.Description AS Priority,
                         severity.Description AS Severity

                     FROM
                         Issues issues
                         INNER JOIN Users userCreated ON (issues.UserCreatedID = userCreated.ID)
                         INNER JOIN Users userClosed ON (issues.UserClosedID = userClosed.ID)
                         INNER JOIN IssueType issueType ON (issues.IssueTypeID = issueType.ID)
                         INNER JOIN IssueState issueState ON (issues.IssueStateID = issueState.ID)
                         INNER JOIN Priority priority ON (issues.PriorityID = priority.ID)
                         INNER JOIN Severity severity ON (issues.SeverityID = severity.ID)
                         INNER JOIN IssueResolution issueResolution ON (issues.IssueResolutionID = issueResolution.ID);";
        }

        public static string CreateDefaultData()
        {
            return
                @"INSERT INTO Severity
                  VALUES (1, 'Critical'),
                         (2, 'High'),
                         (3, 'Medium'),
                         (4, 'Low');
             
                  INSERT INTO Priority
                  VALUES (1, 'High - Must be fixed'),
                         (2, 'Medium - Will be fixed, but not a priority'),
                         (3, 'Low - May be fixed if there is time');
             
                  INSERT INTO IssueType
                  VALUES (1, 'Bug'),
                         (2, 'Change Request');
             
                  INSERT INTO IssueState
                  VALUES (1, 'Open'),
                         (2, 'Closed');
             
                  INSERT INTO IssueResolution
                  VALUES (1, 'NA'),
                         (2, 'Created in error'),
                         (3, 'Closed, rejected'),
                         (4, 'Completed');";
        }

        public static string DoesIssueExist(int id)
        {
            return String.Format(
                @"SELECT COUNT(*) FROM Issues
                  WHERE (ID = {0})",
            id);
        }

        public static string SelectIssue(int id)
        {
            return String.Format(
                @"SELECT * FROM AllIssues
                  WHERE (ID = {0})",
            id);
        }

        public static string InsertIssue(string description, int userCreated, string dateCreated, string plantContact,
                                         int issueType, int issueState, int issueResolution, int priority, int severity)
        {
            return String.Format(
                @"INSERT INTO Issues (Description, UserCreatedID, DateCreated, PlantContact, IssueTypeID, 
                                      IssueStateID, IssueResolutionID, PriorityID, SeverityID)
                  VALUES ('{0}', {1}, '{2}', '{3}', '{4}', {5}, {6}, {7}, {8})",
            description.EscapeChars(), userCreated, dateCreated.EscapeChars(), plantContact.EscapeChars(), issueType, 
            issueState, issueResolution, priority, severity);
        }

        public static string UpdateIssue(int id, string description, int userClosed, string dateClosed, string plantContact,
                                         int issueState, int issueResolution, int priority, int severity)
        {
            return String.Format(
                @"UPDATE Issues
                  SET Description = '{1}', UserClosedID = {2}, DateClosed = '{3}', PlantContact = '{4}', IssueStateID = {5},
                      IssueResolutionID = {6}, PriorityID = {7}, SeverityID = {8}
                  WHERE ID = {0}",
            id, description.EscapeChars(), userClosed, dateClosed.EscapeChars(), plantContact.EscapeChars(), issueState, 
            issueResolution, priority, severity);
        }

        public static string SelectBug(int id)
        {
            return String.Format(
                @"SELECT * FROM Bugs
                  WHERE (IssueID = {0})",
            id);
        }

        public static string InsertBug(int issueId, string versionFound, string versionFixed, string detailedDescription,
                                       string stepsToReproduce, string workaround, string fix)
        {
            return String.Format(
                @"INSERT INTO Bugs (IssueID, VersionFound, VersionFixed, DetailedDescription, StepsToReproduce, Workaround, Fix)
                  VALUES ({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
            issueId, versionFound.EscapeChars(), versionFixed.EscapeChars(), detailedDescription.EscapeChars(),
            stepsToReproduce.EscapeChars(), workaround.EscapeChars(), fix.EscapeChars());
        }

        public static string UpdateBug(int issueId, string versionFound, string versionFixed, string detailedDescription,
                                       string stepsToReproduce, string workaround, string fix)
        {
            return String.Format(
                @"UPDATE Bugs
                  SET VersionFound = '{1}', VersionFixed = '{2}', DetailedDescription = '{3}', StepsToReproduce = '{4}',
                      Workaround = '{5}', Fix = '{6}'
                  WHERE IssueID = {0}",
            issueId, versionFound.EscapeChars(), versionFixed.EscapeChars(), detailedDescription.EscapeChars(),
            stepsToReproduce.EscapeChars(), workaround.EscapeChars(), fix.EscapeChars());

        }

        public static string SelectChangeRequest(int issueId)
        {
            return String.Format(
                @"SELECT * FROM ChangeRequests
                  WHERE (IssueID = {0})",
            issueId);
        }

        public static string InsertChangeRequest(int issueId, string versionImplemented, string detailedDescription, string justification)
        {
            return String.Format(
                @"INSERT INTO ChangeRequests (IssueID, VersionImplemented, DetailedDescription, Justification)
                  VALUES ({0}, '{1}', '{2}', '{3}')",
            issueId, versionImplemented.EscapeChars(), detailedDescription.EscapeChars(), justification.EscapeChars());
        }

        public static string UpdateChangeRequest(int issueId, string versionImplemented, string detailedDescription, string justification)
        {
            return String.Format(
                @"UPDATE ChangeRequest
                  SET VersionImplemented = '{1}', DetailedDescription = '{2}', Justification = '{3}'
                  WHERE IssueID = {0}",
            issueId, versionImplemented.EscapeChars(), detailedDescription.EscapeChars(), justification.EscapeChars());
        }

        public static string GetUserIDFromName(string name)
        {
            return String.Format(
                @"SELECT ID FROM Users
                  WHERE Username = '{0}'",
            name.EscapeChars());
        }

        public static string InsertUsername(string name)
        {
            return string.Format(
                @"INSERT INTO Users(Username)
                  VALUES ('{0}')",
            name.EscapeChars());
        }

        public static string GetRowID()
        {
            return "SELECT last_insert_rowid()";
        }






        public static string InsertTestData()
        {
            return
            @"INSERT INTO Users(Username)
              VALUES ('cbjohns'), ('chris johnson'), ('chris84948');
              
              INSERT INTO Issues (Description, UserCreatedID, UserClosedID, DateCreated, PlantContact, IssueTypeID, 
                                      IssueStateID, IssueResolutionID, PriorityID, SeverityID)
                  VALUES ('Some time something crashed', 1, 2, '08/27/2015', 'Jim', 1, 1, 1, 1, 1),
                         ('blue screen of death', 2, 2, '08/27/2015', 'Bob', 1, 1, 1, 1, 2),
                         ('User message popup with wrong text', 3, 1,' 04/21/2015', 'Jay', 1, 1, 1, 3, 3),
                         ('application crashes due to something', 1, 1, '05/17/2015', 'Jeff', 1, 1, 1, 3, 4),
                         ('user wants a better way to do something', 3, 3, '12/27/2014', 'Anna', 2, 2, 1, 1, 1),
                         ('Add a new feature', 1, 3, '01/17/2012', 'Natasha', 2, 2, 2, 1, 2),
                         ('stack overflow', 1, 2, '08/08/2015', 'Greg', 1, 2, 4, 3, 3),
                         ('Customer hates colors, need to change them', 2, 2, '08/12/2014', 'Jim', 2, 2, 3, 2, 3);
              
              INSERT INTO Bugs (IssueID)
                  VALUES (1), (2), (3), (4), (7);

              INSERT INTO ChangeRequests (IssueID)
                  VALUES (5), (6), (8);";
        }
    }
}
