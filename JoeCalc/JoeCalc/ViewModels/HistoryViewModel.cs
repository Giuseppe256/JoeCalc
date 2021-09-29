using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Linq;

namespace JoeCalc.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        private IList<HistoryEntry> _historyList;
        private HistoryEntry _selectedEntry;

        public HistoryViewModel()
        {
            HistoryList = _historyList;
        }

        public IList<HistoryEntry> HistoryList
        {
            get { return _historyList; }
            set
            {
                _historyList = value;
                OnPropertyChanged();
            }
        }

        public HistoryEntry SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                _selectedEntry = value;
                OnPropertyChanged();
            }
        }
    }
}
