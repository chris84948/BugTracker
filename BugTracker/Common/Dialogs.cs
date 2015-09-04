using BugTracker.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Common
{
    class Dialogs
    {
        public static async Task<MessageDialogResult> GetUserConfirmation(DialogCoordinator dialogCoordinator, ScreenBase tab)
        {
            MetroDialogSettings settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Save",
                NegativeButtonText = "Discard",
                FirstAuxiliaryButtonText = "Cancel",
            };
            MessageDialogResult result = await dialogCoordinator.
                    ShowMessageAsync(tab, "What do you want to do with your changes?", "", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, settings);

            return result;
        }
    }
}
