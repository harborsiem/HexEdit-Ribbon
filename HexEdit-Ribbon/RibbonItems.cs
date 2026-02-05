using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Text.Encodings;
using System.Threading.Tasks;
using Be.HexEditor;
using Be.Windows.Forms;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        private MainForm _mainForm;
        private RecentFileHandler _recentFileHandler;
        public uint ContextId { get { return ContextMenus; } }

        public void Init(MainForm mainForm)
        {
            _mainForm = mainForm;
            Open.Click += Open_Click;
            Save.Click += Save_Click;
            Options.Click += Options_Click;
            About.Click += About_Click;
            Help.Click += About_Click;
            Exit.Click += Exit_Click;
            RecentItems.SelectedChanged += RecentItems_SelectedChanged;
            Paste.Click += Paste_Click;
            PasteHex.Click += PasteHex_Click;
            Cut.Click += Cut_Click;
            Copy.Click += Copy_Click;
            CopyHex.Click += CopyHex_Click;
            Find.Click += Find_Click;
            FindNext.Click += FindNext_Click;
            FindPrevious.Click += FindPrevious_Click;
            GoTo.Click += GoTo_Click;
            SelectAll.Click += SelectAll_Click;
            Encoding.RepresentativeString = "XXXXXXXXXXXXXXXXXXXXXXX";
            Encoding.ItemsSourceReady += Encoding_ItemsSourceReady;
            Encoding.SelectedIndexChanged += Encoding_SelectedIndexChanged;
            Bits.CheckedChanged += Bits_CheckedChanged;
            _recentFileHandler = new RecentFileHandler(_mainForm.Components);
            _recentFileHandler.RibbonRecentItems = RecentItems;
        }

        public RecentFileHandler RecentFileHandler { get { return _recentFileHandler; } }

        public void Load()
        {

        }

        private void Open_Click(object sender, EventArgs e)
        {
            _mainForm.OpenFile();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            _mainForm.SaveFile();
        }

        private void Options_Click(object sender, EventArgs e)
        {
            new FormOptions().ShowDialog();
        }

        private void About_Click(object sender, EventArgs e)
        {
            new FormAbout().ShowDialog();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void RecentItems_SelectedChanged(object sender, SelectedRecentEventArgs e)
        {
            SelectedItem<RecentItemsPropertySet> selected = e.SelectedItem;
            if (selected != null)
            {
                _mainForm.OpenFile(selected.PropertySet.LabelDescription);
            }
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            _mainForm.HexBox.Paste();
        }

        private void PasteHex_Click(object sender, EventArgs e)
        {
            _mainForm.HexBox.PasteHex();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            _mainForm.HexBox.Cut();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            _mainForm.HexBox.Copy();
        }

        private void CopyHex_Click(object sender, EventArgs e)
        {
            _mainForm.HexBox.CopyHex();
        }

        private void Find_Click(object sender, EventArgs e)
        {
            _mainForm.Find();
        }

        private void FindNext_Click(object sender, EventArgs e)
        {
            _mainForm.FindNext();
        }

        private void FindPrevious_Click(object sender, EventArgs e)
        {
            _mainForm.FindPrevious(); 
        }

        private void GoTo_Click(object sender, EventArgs e)
        {
            _mainForm.Goto();
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            _mainForm.HexBox.SelectAll();
        }

        private void Encoding_SelectedIndexChanged(object sender, GalleryItemEventArgs e)
        {
            GalleryItemPropertySet set = e.SelectedItem.PropertySet;
            string label = set.Label;
            _mainForm.HexBox.ByteCharConverter = set.Tag as IByteCharConverter;
        }

        private void Bits_CheckedChanged(object sender, EventArgs e)
        {
            _mainForm.UpdateBitControlVisibility();
        }

        private void Encoding_ItemsSourceReady(object sender, EventArgs e)
        {
            var defConverter = new DefaultByteCharConverter();
            var ebcdicConverter = new EbcdicByteCharProvider();
            int catId = -1;
            Encoding.GalleryItemItemsSource!.Add(new GalleryItemPropertySet() { Label = defConverter.ToString(), CategoryId = catId, Tag = defConverter });
            Encoding.GalleryItemItemsSource.Add(new GalleryItemPropertySet() { Label = ebcdicConverter.ToString(), CategoryId = catId, Tag = ebcdicConverter });
            Encoding.SelectedItem = 0;
        }
    }
}
