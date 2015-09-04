using System.ComponentModel;

namespace BugTracker.Model
{
    enum eIssueResolution
    {
        [Description("N/A - Not Closed")]
        NA = 1,
        [Description("Created in Error")]
        CreatedInError = 2,
        [Description("Rejected and Closed")]
        ClosedRejected = 3,
        [Description("Completed Successfully")]
        Completed = 4
    }
}
