using Be.HexEditor.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Be.HexEditor
{
    internal static class Program
    {
        public const string SoftwareName = "HexEdit";

        public static MainForm ApplicationForm { get; private set; }
        public static readonly Font MonospacedFont = new Font(new FontFamily("Consolas"), 10f, FontStyle.Regular); // "Courier New", 9f;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!Settings.Default.UseSystemLanguage)
            {
                string l = Settings.Default.SelectedLanguage;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(l);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(l);
            }

            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            ApplicationForm = new MainForm();
            if (args.Length > 0 && System.IO.File.Exists(args[0]))
                ApplicationForm.OpenFile(args[0]);
            Application.Run(ApplicationForm);
        }

        public static DialogResult ShowError(Exception ex)
        {
            return ShowError(ex.Message);
        }

        internal static DialogResult ShowError(string text)
        {
            DialogResult result = MessageBox.Show(text, SoftwareName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return result;
        }

        public static void ShowMessage(string text)
        {
            MessageBox.Show(text, SoftwareName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static DialogResult ShowQuestion(string text)
        {
            DialogResult result = MessageBox.Show(text, SoftwareName,
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
            return result;
        }
    }
}