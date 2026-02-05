using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using Be.Windows.Forms;
using WinForms.Ribbon;

namespace Be.HexEditor
{
    public interface IUserHexBoxContextMenu
    {
        ContextMenuStrip HexBoxContextMenuStrip { get; }

        void SetEnabled(bool cut, bool copy, bool paste, bool selectAll);
    }

    /// <summary>
    /// ContextMenuStrip build from a Ribbon ContextMenu for the HexBox to override the build in ContextMenu
    /// </summary>
    internal class HexBoxContextMenu : ContextMenuStrip, IUserHexBoxContextMenu
    {
        private readonly HexBox _hexBox;
        private readonly RibbonContextElements _elements;

        public HexBoxContextMenu(HexBox hexBox, RibbonContextElements elements) : base()
        {
            _hexBox = hexBox;
            _elements = elements;
            elements.Cut.Click += Cut_Click;
            elements.Copy.Click += Copy_Click;
            elements.Paste.Click += Paste_Click;
            elements.SelectAll.Click += SelectAll_Click;
        }

        public ContextMenuStrip HexBoxContextMenuStrip { get { return this; } }

        public void SetEnabled(bool cut, bool copy, bool paste, bool selectAll)
        {
            _elements.Cut.Enabled = cut;
            _elements.Copy.Enabled = copy;
            _elements.Paste.Enabled = paste;
            _elements.SelectAll.Enabled = selectAll;
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            HexBox hex = this.SourceControl as HexBox;
            _elements.Cut.Enabled = _hexBox.CanCut();
            _elements.Copy.Enabled = _hexBox.CanCopy();
            _elements.Paste.Enabled = _hexBox.CanPaste();
            _elements.SelectAll.Enabled = _hexBox.CanSelectAll();
            _elements.Ribbon.ShowContextPopup(_elements.ContextPopupId, Cursor.Position.X,
                                 Cursor.Position.Y);
            e.Cancel = true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _elements.Cut.Click -= Cut_Click;
            _elements.Copy.Click -= Copy_Click;
            _elements.Paste.Click -= Paste_Click;
            _elements.SelectAll.Click -= SelectAll_Click;
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            if (this.SourceControl is HexBox hexBox)
                hexBox.Cut();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            _hexBox.Copy();
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            _hexBox.Paste();
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            _hexBox.SelectAll();
        }
    }

    internal class RibbonContextElements
    {
        public readonly RibbonStrip Ribbon;
        public readonly uint ContextPopupId;
        public readonly RibbonButton Cut;
        public readonly RibbonButton Copy;
        public readonly RibbonButton Paste;
        public readonly RibbonButton SelectAll;

        public RibbonContextElements(RibbonStrip ribbon, uint contextPopupId, RibbonButton cut, RibbonButton copy, RibbonButton paste, RibbonButton selectAll)
        {
            Ribbon = ribbon;
            ContextPopupId = contextPopupId;
            Cut = cut;
            Copy = copy;
            Paste = paste;
            SelectAll = selectAll;
        }
    }
}
