using System;
using System.Reflection;
using System.Windows.Forms;
using System.Resources;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.HiDpi;
using WinForms.Ribbon;
using Be.Windows.Forms;

namespace Be.HexEditor
{
    public partial class MainForm : Core.FormEx
    {
        FormFind _formFind;
        FindOptions _findOptions = new FindOptions();
        FormGoTo _formGoto = new FormGoTo();
        string _fileName;
        OpenFileDialog openFileDialog;
        private RibbonItems _ribbonItems;

        public MainForm()
        {
            InitializeComponent();
            FormClosing += MainForm_FormClosing;
            if (components == null)
                components = new System.ComponentModel.Container();
            hexBox.Font = Program.MonospacedFont;
            components.Add(this.hexBox);

            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            _ribbonItems = new RibbonItems(_ribbon);
            _ribbonItems.Init(this);
            Init();
            Load += Form1_Load;
            RibbonContextElements elements = new RibbonContextElements(_ribbon, _ribbonItems.ContextId,
                _ribbonItems.CutContext, _ribbonItems.CopyContext, _ribbonItems.PasteContext, _ribbonItems.SelectAllContext);
            _ribbonContextMenu = new HexBoxContextMenu(hexBox, elements);
            components.Add(_ribbonContextMenu);
            hexBox.SuppressBuiltInContextMenu = true;
            hexBox.ByteProviderChanged += HexBox_ByteProviderChanged;
        }

        private ContextMenuStrip _ribbonContextMenu;

        internal RecentFileHandler RecentFileHandler { get { return _ribbonItems.RecentFileHandler; } }

        private void HexBox_ByteProviderChanged(object sender, EventArgs e)
        {
            if (hexBox.ByteProvider != null)
                hexBox.ContextMenuStrip = _ribbonContextMenu;
            else
                hexBox.ContextMenuStrip = null;
        }

        internal RibbonItems RibbonItems { get { return _ribbonItems; } }

        public IContainer Components { get { return components; } }

        private unsafe void Form1_Load(object sender, EventArgs e)
        {
            _ribbonItems.Load();
        }

        internal HexBox HexBox { get { return hexBox; } }

        /// <summary>
        /// Initializes the hex editor´s main form
        /// </summary>
        void Init()
        {
            DisplayText();

            ManageAbility();

            UpdateBitControlVisibility();

            //var selected = ;
            //var defConverter = new DefaultByteCharConverter();
            //ToolStripMenuItem miDefault = new ToolStripMenuItem();
            //miDefault.Text = defConverter.ToString();
            //miDefault.Tag = defConverter;
            //miDefault.Click += new EventHandler(encodingMenuItem_Clicked);

            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //var ebcdicConverter = new EbcdicByteCharProvider();
            //ToolStripMenuItem miEbcdic = new ToolStripMenuItem();
            //miEbcdic.Text = ebcdicConverter.ToString();
            //miEbcdic.Tag = ebcdicConverter;
            //miEbcdic.Click += new EventHandler(encodingMenuItem_Clicked);

            //encodingToolStripComboBox.Items.Add(defConverter);
            //encodingToolStripComboBox.Items.Add(ebcdicConverter);

            //encodingToolStripMenuItem.DropDownItems.Add(miDefault);
            //encodingToolStripMenuItem.DropDownItems.Add(miEbcdic);
            //encodingToolStripComboBox.SelectedIndex = 0;

            UpdateFormWidth();
        }

        /// <summary>
        /// Updates the File size status label
        /// </summary>
        void UpdateFileSizeStatus()
        {
            if (this.hexBox.ByteProvider == null)
                this.fileSizeToolStripStatusLabel.Text = string.Empty;
            else
                this.fileSizeToolStripStatusLabel.Text = Util.GetDisplayBytes(this.hexBox.ByteProvider.Length);
        }

        /// <summary>
        /// Displays the file name in the Form´s text property
        /// </summary>
        /// <param name="fileName">the file name to display</param>
        void DisplayText()
        {
            if (_fileName != null && _fileName.Length > 0)
            {
                string textFormat = "{0}{1}";
                string readOnly = ((DynamicFileByteProvider)hexBox.ByteProvider).ReadOnly
                    ? ResStrings.Readonly : "";
                string text = Path.GetFileName(_fileName);
                this.Text = string.Format(textFormat, text, readOnly);
            }
            else
            {
                this.Text = "";
            }
        }

        /// <summary>
        /// Manages enabling or disabling of menu items and toolstrip buttons.
        /// </summary>
        void ManageAbility()
        {
            if (hexBox.ByteProvider == null)
            {
                _ribbonItems.Save.Enabled = false;

                _ribbonItems.Find.Enabled = false;
                _ribbonItems.FindNext.Enabled = false;
                _ribbonItems.FindPrevious.Enabled = false;
                _ribbonItems.GoTo.Enabled = false;

                _ribbonItems.SelectAll.Enabled = false;
            }
            else
            {
                _ribbonItems.Save.Enabled = hexBox.ByteProvider.HasChanges();

                _ribbonItems.Find.Enabled = true;
                _ribbonItems.FindNext.Enabled = true;
                _ribbonItems.FindPrevious.Enabled = true;
                _ribbonItems.GoTo.Enabled = true;

                _ribbonItems.SelectAll.Enabled = true;
            }

            ManageAbilityForCopyAndPaste();
        }

        /// <summary>
        /// Manages enabling or disabling of menustrip items and toolstrip buttons for copy and paste
        /// </summary>
        void ManageAbilityForCopyAndPaste()
        {
            _ribbonItems.CopyHex.Enabled =
                _ribbonItems.Copy.Enabled = hexBox.CanCopy();

            _ribbonItems.Cut.Enabled = hexBox.CanCut();
            _ribbonItems.Paste.Enabled = hexBox.CanPaste();
            _ribbonItems.PasteHex.Enabled = hexBox.CanPasteHex();
        }

        /// <summary>
        /// Shows the open file dialog.
        /// </summary>
        internal void OpenFile()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenFile(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Opens a file.
        /// </summary>
        /// <param name="fileName">the file name of the file to open</param>
        public void OpenFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Program.ShowMessage(ResStrings.FileDoesNotExist);
                return;
            }

            if (CloseFile() == DialogResult.Cancel)
                return;

            try
            {
                DynamicFileByteProvider dynamicFileByteProvider;
                try
                {
                    // try to open in write mode
                    dynamicFileByteProvider = new DynamicFileByteProvider(fileName);
                    dynamicFileByteProvider.Changed += new EventHandler(byteProvider_Changed);
                    dynamicFileByteProvider.LengthChanged += new EventHandler(byteProvider_LengthChanged);
                }
                catch (IOException) // write mode failed
                {
                    try
                    {
                        // try to open in read-only mode
                        dynamicFileByteProvider = new DynamicFileByteProvider(fileName, true);
                        if (Program.ShowQuestion(ResStrings.OpenReadonly) == DialogResult.No)
                        {
                            dynamicFileByteProvider.Dispose();
                            return;
                        }
                    }
                    catch (IOException) // read-only also failed
                    {
                        // file cannot be opened
                        Program.ShowError(ResStrings.OpenFailed);
                        return;
                    }
                }

                hexBox.ByteProvider = dynamicFileByteProvider;
                _fileName = fileName;

                DisplayText();

                UpdateFileSizeStatus();

                _ribbonItems.RecentFileHandler.AddFile(fileName);
            }
            catch (Exception ex1)
            {
                Program.ShowError(ex1);
                return;
            }
            finally
            {
                ManageAbility();
            }
        }

        /// <summary>
        /// Saves the current file.
        /// </summary>
        internal void SaveFile()
        {
            if (hexBox.ByteProvider == null)
                return;

            try
            {
                DynamicFileByteProvider dynamicFileByteProvider = hexBox.ByteProvider as DynamicFileByteProvider;
                dynamicFileByteProvider!.ApplyChanges();
            }
            catch (Exception ex1)
            {
                Program.ShowError(ex1);
            }
            finally
            {
                ManageAbility();
            }
        }
        /// <summary>
        /// Closes the current file
        /// </summary>
        /// <returns>OK, if the current file was closed.</returns>
        DialogResult CloseFile()
        {
            if (hexBox.ByteProvider == null)
                return DialogResult.OK;
            try
            {
                if (hexBox.ByteProvider != null && hexBox.ByteProvider.HasChanges())
                {
                    DialogResult res = MessageBox.Show(ResStrings.SaveChangesQuestion,
                        Program.SoftwareName,
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning);

                    if (res == DialogResult.Yes)
                    {
                        SaveFile();
                        CleanUp();
                    }
                    else if (res == DialogResult.No)
                    {
                        CleanUp();
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        return res;
                    }

                    return res;
                }
                else
                {
                    CleanUp();
                    return DialogResult.OK;
                }
            }
            finally
            {
                ManageAbility();
            }
        }

        void CleanUp()
        {
            if (hexBox.ByteProvider != null)
            {
                IDisposable byteProvider = hexBox.ByteProvider as IDisposable;
                if (byteProvider != null)
                    byteProvider.Dispose();
                hexBox.ByteProvider = null;
            }
            _fileName = null;
            DisplayText();
        }

        /// <summary>
        /// Opens the Find dialog
        /// </summary>
        internal void Find()
        {
            ShowFind();
        }

        /// <summary>
        /// Creates a new FormFind dialog
        /// </summary>
        /// <returns>the form find dialog</returns>
        FormFind ShowFind()
        {
            if (_formFind == null || _formFind.IsDisposed)
            {
                _formFind = new FormFind();
                _formFind.HexBox = this.hexBox;
                _formFind.FindOptions = _findOptions;
                _formFind.Show(this);
            }
            else
            {
                _formFind.Focus();
            }
            return _formFind;
        }

        /// <summary>
        /// Find next match
        /// </summary>
        internal void FindNext()
        {
            FormFind formFind = ShowFind();
            formFind.FindOptions.FindDirection = Direction.Forward;
            formFind.FindNext();
        }

        /// <summary>
        /// Find previous match
        /// </summary>
        internal void FindPrevious()
        {
            FormFind formFind = ShowFind();
            formFind.FindOptions.FindDirection = Direction.Backward;
            formFind.FindNext();
        }

        /// <summary>
        /// Aborts the current find process
        /// </summary>
        void FormFindCancel_Closed(object sender, EventArgs e)
        {
            hexBox.AbortFind();
        }

        /// <summary>
        /// Displays the goto byte dialog.
        /// </summary>
        internal void Goto()
        {
            _formGoto.SetMaxByteIndex(hexBox.ByteProvider.Length - 1); //@??
            _formGoto.SetDefaultValue(hexBox.SelectionStart);
            if (_formGoto.ShowDialog() == DialogResult.OK)
            {
                hexBox.SelectionStart = _formGoto.GetByteIndex();
                hexBox.SelectionLength = 1;
                hexBox.Focus();
            }
        }

        /// <summary>
        /// Enables drag&drop
        /// </summary>
        void hexBox_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        /// <summary>
        /// Processes a file drop
        /// </summary>
        void hexBox_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            object oFileNames = e.Data?.GetData(DataFormats.FileDrop);
            if (oFileNames != null)
            {
                string[] fileNames = (string[])oFileNames;
                if (fileNames?.Length == 1)
                {
                    OpenFile(fileNames[0]);
                }
            }
        }

        void hexBox_Copied(object sender, EventArgs e)
        {
            ManageAbilityForCopyAndPaste();
        }

        void hexBox_CopiedHex(object sender, EventArgs e)
        {
            ManageAbilityForCopyAndPaste();
        }

        void hexBox_SelectionLengthChanged(object sender, System.EventArgs e)
        {
            ManageAbilityForCopyAndPaste();
        }

        void hexBox_SelectionStartChanged(object sender, System.EventArgs e)
        {
            ManageAbilityForCopyAndPaste();
        }

        void Position_Changed(object sender, EventArgs e)
        {
            this.toolStripStatusLabel.Text = string.Format("Ln {0}    Col {1}",
                hexBox.CurrentLine, hexBox.CurrentPositionInLine);

            string bitPresentation = string.Empty;

            byte? currentByte = hexBox.ByteProvider != null && hexBox.ByteProvider.Length > hexBox.SelectionStart
                ? hexBox.ByteProvider.ReadByte(hexBox.SelectionStart)
                : (byte?)null;

            BitInfo bitInfo = currentByte != null ? new BitInfo((byte)currentByte, hexBox.SelectionStart) : null;

            if (bitInfo != null)
            {
                byte currentByteNotNull = (byte)currentByte!;
                bitPresentation = string.Format("Bits of Byte {0}: {1}"
                    , hexBox.SelectionStart
                    , bitInfo.ToString()
                    );
            }

            this.bitToolStripStatusLabel.Text = bitPresentation;

            this.bitControl1.BitInfo = bitInfo;
        }

        void byteProvider_Changed(object sender, EventArgs e)
        {
            ManageAbility();
        }

        void byteProvider_LengthChanged(object sender, EventArgs e)
        {
            UpdateFileSizeStatus();
        }


        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = CloseFile();
            if (result == DialogResult.Cancel)
                e.Cancel = true;
        }

        internal void UpdateBitControlVisibility()
        {
            if (Util.DesignMode)
                return;
            //if (this.bitControl1.Visible == _ribbonItems.Bits.BooleanValue)
            //{
            //    return;
            //}
            if (_ribbonItems.Bits.BooleanValue)
            {
                hexBox.Height -= bitControl1.Height;
                bitControl1.Visible = true;
            }
            else
            {
                hexBox.Height += bitControl1.Height;
                bitControl1.Visible = false;
            }
        }

        void bitControl1_BitChanged(object sender, EventArgs e)
        {
            hexBox.ByteProvider.WriteByte(bitControl1.BitInfo.Position, bitControl1.BitInfo.Value);
            hexBox.Invalidate();
        }


        void hexBox_RequiredWidthChanged(object sender, EventArgs e)
        {
            UpdateFormWidth();
        }

        void UpdateFormWidth()
        {
            this.Width = this.hexBox.RequiredWidth + 70;
        }

    }
}
