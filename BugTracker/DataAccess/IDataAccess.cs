using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Model;
using BugTracker.ViewModels;

namespace BugTracker.DataAccess
{
    public interface IDataAccess
    {
        void UpdateLocation(string newLocation);
        List<IssueViewModel> GetAllIssues(string filter);
        IssueViewModel GetIssue(int id);
        Bug GetBug(int id);
        ChangeRequest GetChangeRequest(int id);
        int SaveBug(Issue issue, Bug bug);
        int SaveChangeRequest(Issue isse, ChangeRequest changeRequest);
    }
}
