using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Be.HexEditor.Properties;
using WinForms.Ribbon;

namespace Be.HexEditor
{
    public partial class RecentFileHandler : Component
    {
        public const int MaxRecentFiles = 20;

        public RecentFileHandler()
        {
            InitializeComponent();

            Init();
        }

        public RecentFileHandler(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            Init();
        }

        void Init()
        {
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);
        }

        public void AddFile(string filename)
        {
            if (this._ribbonRecentItems == null)
                throw new OperationCanceledException("_ribbonRecentItems can not be null!");

            // check if the file is already in the collection
            int alreadyIn = GetIndexOfRecentFile(filename);
            if (alreadyIn == 0) // it´s the latest file so return
                return;

            if (alreadyIn != -1) // remove it
            {
                Settings.Default.RecentFiles.RemoveAt(alreadyIn);
                if (_recentItems.Count > alreadyIn)
                    _recentItems.RemoveAt(alreadyIn);
            }

            // insert the file on top of the list
            Settings.Default.RecentFiles.Insert(0, filename);
            var newItem = CreateItem(filename);
            _recentItems.Insert(0, newItem);

            // remove the last one, if max size is reached
            if (Settings.Default.RecentFiles.Count > MaxRecentFiles)
                Settings.Default.RecentFiles.RemoveAt(MaxRecentFiles);
            if (Settings.Default.RecentFiles.Count > Settings.Default.RecentFilesMax)
            {
                _recentItems.RemoveAt(Settings.Default.RecentFilesMax);
                Settings.Default.RecentFiles.RemoveAt(Settings.Default.RecentFilesMax);
            }

            // update recent files in ribbon
            //this._ribbonRecentItems.RecentItems = _recentItems;

            // save the changes
            Settings.Default.Save();
        }

        int GetIndexOfRecentFile(string filename)
        {
            for (int i = 0; i < Settings.Default.RecentFiles.Count; i++)
            {
                string currentFile = Settings.Default.RecentFiles[i];
                if (string.Equals(currentFile, filename, StringComparison.InvariantCultureIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        RibbonRecentItems _ribbonRecentItems;

        public RibbonRecentItems RibbonRecentItems
        {
            get { return _ribbonRecentItems; }
            set
            {
                if (_ribbonRecentItems == value)
                    return;

                _ribbonRecentItems = value;

                ReCreateItems();
            }
        }

        void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RecentFilesMax")
            {
                ReCreateItems();
            }
        }

        IList<RecentItemsPropertySet> _recentItems;

        void ReCreateItems()
        {
            if (_ribbonRecentItems == null)
                return;

            if (Settings.Default.RecentFiles == null)
                Settings.Default.RecentFiles = new StringCollection();

            int fileItemCount = Math.Min(MaxRecentFiles, Settings.Default.RecentFiles.Count);

            //_recentItems = new List<RecentItemsPropertySet>();
            _ribbonRecentItems.RecentItems.Clear();
            _recentItems = _ribbonRecentItems.RecentItems;
            for (int i = 0; i < fileItemCount; i++)
            {
                string file = Settings.Default.RecentFiles[i];
                var item = CreateItem(file);
                _recentItems.Add(item);
            }

            //this._ribbonRecentItems.RecentItems = _recentItems;
        }

        RecentItemsPropertySet CreateItem(string file)
        {
            string label = string.Format("{0} ({1})", Path.GetFileName(file), Path.GetDirectoryName(file));
            var result = new RecentItemsPropertySet()
            {
                Label = label,
                LabelDescription = file
            };
            return result;
        }

        public void Clear()
        {
            Settings.Default.RecentFiles.Clear();
            Settings.Default.Save();
            ReCreateItems();
        }
    }
}
