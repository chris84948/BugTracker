using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BugTracker.MVVM;
using System.Windows;
using BugTracker.Model;
using BugTracker.DataAccess;
using MahApps.Metro.Controls.Dialogs;

namespace BugTracker.ViewModels
{
    public class ScreenBase : ObservableObject
    {
        protected Messenger messenger;
        protected DialogCoordinator dialogCoordinator;

        private bool _showCloseButton;
        public bool ShowCloseButton
        {
            get { return _showCloseButton; }
            set
            {
                _showCloseButton = value;
                OnPropertyChanged(() => ShowCloseButton);
            }
        }

        private string _tabHeader;
        public string TabHeader
        {
            get { return _tabHeader != null ? _tabHeader.ToUpper() : ""; }
            set
            {
                _tabHeader = value;
                OnPropertyChanged(() => TabHeader);
            }
        }

        private int _id;
        public virtual int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(() => ID);
            }
        }
        
        private bool _isDirty;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                OnPropertyChanged(() => IsDirty);

                if (messenger != null)
                    messenger.tabSaveStatusChanged();
            }
        }

        public virtual void Save() 
        {
            IsDirty = false;
        }

        public virtual void CloseTab()
        { }
    }
}
