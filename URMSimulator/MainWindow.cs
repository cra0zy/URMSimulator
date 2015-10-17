using System.Collections.Generic;
using System.Threading;
using System.IO;
using System;

using Xwt;

namespace URMSimulator
{
    public partial class MainWindow : Window
    {
        FileDialogFilter _urmFileFilter;
        FileDialogFilter _txtFileFilter;
        FileDialogFilter _anyFilesFilter;

        string fileName = "";
        string savedContent = "S(1)\r\nS(1)\r\nS(1)";

        public MainWindow()
        {
            Build();

            _urmFileFilter = new FileDialogFilter("URM Files (*.urm)", "*.urm");
            _txtFileFilter = new FileDialogFilter("Text Files (*.txt)", "*.txt");
            _anyFilesFilter = new FileDialogFilter("All Files (*.*)", "*");

            MessageDialog.RootWindow = this;
            ReloadMenuAndTitle();
        }

        protected override void OnShown()
        {
            base.OnShown();
            ShowMessage("");
        }

        protected override bool OnCloseRequested()
        {
            if (!MaybeSave())
                return false;

            Application.Exit();
            return true;
        }

        private void ReloadMenuAndTitle()
        {
            bool projectOpen = !string.IsNullOrEmpty(fileName);

            string fname = "Untitled";
            if (projectOpen)
                fname = Path.GetFileName(fileName);
            this.Title = "URM Simulator - " + fname;

            ReloadMenuAndTitleEnding();
        }

        private void ReloadMenuAndTitleEnding()
        {
            if (savedContent != te1.Document.Text && !this.Title.EndsWith("*"))
                this.Title = this.Title + "*";
            else if (savedContent == te1.Document.Text && this.Title.EndsWith("*"))
                this.Title = this.Title.Remove(this.Title.Length - 1);
            
            undoMenuItem.Sensitive = te1.CanUndo();
            redoMenuItem.Sensitive = te1.CanRedo();

            cutMenuItem.Sensitive = te1.CanCopy();
            copyMenuItem.Sensitive = te1.CanCopy();
            pasteMenuItem.Sensitive = te1.CanPaste();
        }

        private bool MaybeSave()
        {
            if (savedContent != te1.Document.Text)
            {
                var result = MessageDialog.AskQuestion("Do you want to save?", 0, Command.Yes, Command.No, Command.Cancel);

                if (result == Command.Cancel || (result == Command.Yes && !Save(false)))
                    return false;
            }

            return true;
        }

        private void New()
        {
            if (!MaybeSave())
                return;

            fileName = "";
            te1.Document.Text = "";
            te1.ClearUndoRedoStack();
            savedContent = te1.Document.Text;

            ReloadMenuAndTitle();
        }

        private void Open()
        {
            var ofdialog = new OpenFileDialog();
            ofdialog.Filters.Add(_urmFileFilter);
            ofdialog.Filters.Add(_txtFileFilter);
            ofdialog.Filters.Add(_anyFilesFilter);

            if (ofdialog.Run(this))
            {
                fileName = ofdialog.FileName;
                te1.Document.Text = File.ReadAllText(fileName);
                te1.ClearUndoRedoStack();
                savedContent = te1.Document.Text;

                ReloadMenuAndTitle();
            }
        }

        private bool Save(bool saveas)
        {
            if (saveas || string.IsNullOrEmpty(fileName))
            {
                var sfdialog = new SaveFileDialog();
                sfdialog.Filters.Add(_urmFileFilter);
                sfdialog.Filters.Add(_txtFileFilter);
                sfdialog.Filters.Add(_anyFilesFilter);

                if (sfdialog.Run(this))
                {
                    fileName = sfdialog.FileName;

                    var e = sfdialog.ActiveFilter.Patterns.GetEnumerator();
                    e.MoveNext();
                    var pattern = e.Current;

                    if (pattern != "*")
                        if (!fileName.EndsWith(pattern.Substring(1)))
                            fileName += pattern.Substring(1);
                }
                else
                    return false;
            }

            savedContent = te1.Document.Text;
            File.WriteAllText(fileName, te1.Document.Text);
            ReloadMenuAndTitle();

            return true;
        }

        private void ShowAboutDialog()
        {
            var adialog = new AboutDialog();

            adialog.TransientFor = this;
            adialog.ProgramName = "URM Simulator v1.0.0";
            adialog.Comments = "Unlimited Register Machine SImulator";
            adialog.Website = "https://github.com/cra0zy/URMSimulator";
            adialog.WebsiteLabel = "Project Webpage";
            adialog.Image = Xwt.Drawing.Image.FromResource("URMSimulator.Resources.urm.png");

            adialog.Run();
            adialog.Close();
        }
    }
}


