namespace Be.HexEditor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            _ribbon = new WinForms.Ribbon.RibbonStrip();
            statusStrip = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            fileSizeToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            bitToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            hexBox = new Windows.Forms.HexBox();
            bitControl1 = new BitControl();
            bodyPanel = new System.Windows.Forms.Panel();
            statusStrip.SuspendLayout();
            bodyPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _ribbon
            // 
            resources.ApplyResources(_ribbon, "_ribbon");
            _ribbon.Name = "_ribbon";
            _ribbon.MarkupHeader = "Be.HexEditor.RibbonItems.h";
            _ribbon.MarkupResource = "Be.HexEditor.RibbonItems.ribbon";
            _ribbon.ShortcutTableResourceName = "Be.HexEditor.RibbonItems.shortcuttable";
            // 
            // statusStrip
            // 
            statusStrip.BackColor = System.Drawing.SystemColors.Control;
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel, fileSizeToolStripStatusLabel, bitToolStripStatusLabel });
            resources.ApplyResources(statusStrip, "statusStrip");
            statusStrip.Name = "statusStrip";
            statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            statusStrip.SizingGrip = false;
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            resources.ApplyResources(toolStripStatusLabel, "toolStripStatusLabel");
            // 
            // fileSizeToolStripStatusLabel
            // 
            fileSizeToolStripStatusLabel.Name = "fileSizeToolStripStatusLabel";
            fileSizeToolStripStatusLabel.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            resources.ApplyResources(fileSizeToolStripStatusLabel, "fileSizeToolStripStatusLabel");
            // 
            // bitToolStripStatusLabel
            // 
            bitToolStripStatusLabel.Name = "bitToolStripStatusLabel";
            resources.ApplyResources(bitToolStripStatusLabel, "bitToolStripStatusLabel");
            // 
            // hexBox
            // 
            hexBox.AllowDrop = true;
            resources.ApplyResources(hexBox, "hexBox");
            hexBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            // 
            // 
            // 
            hexBox.BuiltInContextMenu.CopyMenuItemImage = ResImages.CopyHS;
            hexBox.BuiltInContextMenu.CopyMenuItemText = resources.GetString("hexBox.BuiltInContextMenu.CopyMenuItemText");
            hexBox.BuiltInContextMenu.CutMenuItemImage = ResImages.CutHS;
            hexBox.BuiltInContextMenu.CutMenuItemText = resources.GetString("hexBox.BuiltInContextMenu.CutMenuItemText");
            hexBox.BuiltInContextMenu.PasteMenuItemImage = ResImages.PasteHS;
            hexBox.BuiltInContextMenu.PasteMenuItemText = resources.GetString("hexBox.BuiltInContextMenu.PasteMenuItemText");
            hexBox.BuiltInContextMenu.SelectAllMenuItemText = resources.GetString("hexBox.BuiltInContextMenu.SelectAllMenuItemText");
            hexBox.ColumnInfoVisible = true;
            hexBox.HexCasing = Windows.Forms.HexCasing.Lower;
            hexBox.LineInfoVisible = true;
            hexBox.Name = "hexBox";
            hexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(100, 60, 188, 255);
            hexBox.StringViewVisible = true;
            hexBox.UseFixedBytesPerLine = true;
            hexBox.VScrollBarVisible = true;
            hexBox.SelectionStartChanged += hexBox_SelectionStartChanged;
            hexBox.SelectionLengthChanged += hexBox_SelectionLengthChanged;
            hexBox.CurrentLineChanged += Position_Changed;
            hexBox.CurrentPositionInLineChanged += Position_Changed;
            hexBox.Copied += hexBox_Copied;
            hexBox.CopiedHex += hexBox_CopiedHex;
            hexBox.RequiredWidthChanged += hexBox_RequiredWidthChanged;
            hexBox.DragDrop += hexBox_DragDrop;
            hexBox.DragEnter += hexBox_DragEnter;
            // 
            // bitControl1
            // 
            resources.ApplyResources(bitControl1, "bitControl1");
            bitControl1.Name = "bitControl1";
            bitControl1.BitChanged += bitControl1_BitChanged;
            // 
            // bodyPanel
            // 
            bodyPanel.Controls.Add(hexBox);
            bodyPanel.Controls.Add(bitControl1);
            resources.ApplyResources(bodyPanel, "bodyPanel");
            bodyPanel.Name = "bodyPanel";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(bodyPanel);
            Controls.Add(statusStrip);
            Controls.Add(_ribbon);
            Name = "MainForm";
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            bodyPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private WinForms.Ribbon.RibbonStrip _ribbon;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel fileSizeToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel bitToolStripStatusLabel;
        private System.Windows.Forms.Panel bodyPanel;
        private Be.Windows.Forms.HexBox hexBox;
        private BitControl bitControl1;
    }
}
