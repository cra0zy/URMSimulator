using Xwt;
using XwtPlus.TextEditor;

namespace URMSimulator
{
    public partial class MainWindow
    {
        VPaned vpanned1;
        Label label1, label2, label3;
        VBox vbox1, vbox2;
        TextEditor te1;
        TextEntry entry1;
        WebView web1;
        ScrollView scroll1;
        Button button1;
        HBox hbox1;
        SpinButton sb1;

        MenuItem newMenuItem, openMenuItem, saveMenuItem, saveAsMenuItem, exitMenuItem;
        MenuItem undoMenuItem, redoMenuItem, cutMenuItem, copyMenuItem, pasteMenuItem, selectAllMenuItem;
        MenuItem runMenuItem;
        CheckBoxMenuItem debugMenuItem;
        MenuItem aboutMenuItem;

        private void Build()
        {
            this.Icon = Xwt.Drawing.Image.FromResource("URMSimulator.Resources.urm.png");
            this.Title = "URM Simulator";
            this.Width = 800;
            this.Height = 600;
            this.Padding = 0;

            vbox1 = new VBox();

            vpanned1 = new VPaned();

            te1 = new TextEditor();
            te1.Document.MimeType = "text/x-urm";
            te1.Document.Text = savedContent;
            te1.Document.LineChanged += (sender, e) => ReloadTitleEnding();
            te1.Document.TextSet += (sender, e) => ReloadTitleEnding();

            vpanned1.Panel1.Content = te1;
            vpanned1.Panel1.Resize = true;

            vbox2 = new VBox();

            label1 = new Label("Input:");
            label1.MarginLeft = 5;
            vbox2.PackStart(label1, false);

            entry1 = new TextEntry();
            entry1.PlaceholderText = "0 0 0 0 0 0 0 0 0 0";
            vbox2.PackStart(entry1, false);

            hbox1 = new HBox();

            label2 = new Label("Output:");
            label2.MarginLeft = 5;
            hbox1.PackStart(label2, true);

            label3 = new Label("Register length:");
            hbox1.PackStart(label3, false);

            sb1 = new SpinButton();
            sb1.MinimumValue = 1;
            sb1.Digits = 0;
            sb1.IncrementValue = 1;
            sb1.MaximumValue = 100;
            sb1.Value = regCount;
            sb1.ValueChanged += (sender, e) => regCount = (int)sb1.Value;
            sb1.TextInput += (sender, e) => regCount = (int)sb1.Value;
            hbox1.PackStart(sb1, false);

            vbox2.PackStart(hbox1, false);

            scroll1 = new ScrollView();
            scroll1.HeightRequest = 80;
            scroll1.VerticalScrollPolicy = ScrollPolicy.Never;
            scroll1.BorderVisible = true;

            web1 = new WebView();
            web1.WidthRequest = 2000;
            scroll1.Content = web1;

            vbox2.PackStart(scroll1, true);

            vpanned1.Panel2.Content = vbox2;
            vpanned1.Panel2.Resize = false;

            if (Toolkit.CurrentEngine.Type == ToolkitType.Wpf)
                vpanned1.Position = this.Height - 120;

            vbox1.PackStart(vpanned1, true);

            button1 = new Button("Continue Debugging");
            button1.Visible = false;
            button1.Clicked += (sender, e) => ContinueDebugging();
            vbox1.PackStart(button1, false);

            this.Content = vbox1;
            te1.SetFocus();

            BuildMenu();
        }

        private void BuildMenu()
        {
            var menu = new Menu();

            var fileMenu = new MenuItem("File");
            fileMenu.SubMenu = new Menu();
            menu.Items.Add(fileMenu);

            newMenuItem = new MenuItem("New");
            newMenuItem.Clicked += (sender, e) => New();
            fileMenu.SubMenu.Items.Add(newMenuItem);

            openMenuItem = new MenuItem("Open");
            openMenuItem.Clicked += (sender, e) => Open();
            fileMenu.SubMenu.Items.Add(openMenuItem);

            fileMenu.SubMenu.Items.Add(new SeparatorMenuItem());

            saveMenuItem = new MenuItem("Save");
            saveMenuItem.Clicked += (sender, e) => Save(false);
            fileMenu.SubMenu.Items.Add(saveMenuItem);

            saveAsMenuItem = new MenuItem("Save As");
            saveAsMenuItem.Clicked += (sender, e) => Save(true);
            fileMenu.SubMenu.Items.Add(saveAsMenuItem);

            fileMenu.SubMenu.Items.Add(new SeparatorMenuItem());

            exitMenuItem = new MenuItem("Exit");
            exitMenuItem.Clicked += (sender, e) => this.Close();
            fileMenu.SubMenu.Items.Add(exitMenuItem);

            var editMenu = new MenuItem("Edit");
            editMenu.SubMenu = new Menu();
            editMenu.Clicked += delegate
            {
                undoMenuItem.Sensitive = te1.CanUndo();
                redoMenuItem.Sensitive = te1.CanRedo();

                cutMenuItem.Sensitive = te1.CanCopy();
                copyMenuItem.Sensitive = te1.CanCopy();
                pasteMenuItem.Sensitive = te1.CanPaste();
            };
            menu.Items.Add(editMenu);

            undoMenuItem = new MenuItem("Undo");
            undoMenuItem.Clicked += (sender, e) => te1.Undo();
            editMenu.SubMenu.Items.Add(undoMenuItem);

            redoMenuItem = new MenuItem("Redo");
            redoMenuItem.Clicked += (sender, e) => te1.Redo();
            editMenu.SubMenu.Items.Add(redoMenuItem);

            editMenu.SubMenu.Items.Add(new SeparatorMenuItem());

            cutMenuItem = new MenuItem("Cut");
            cutMenuItem.Clicked += (sender, e) => te1.Cut();
            editMenu.SubMenu.Items.Add(cutMenuItem);

            copyMenuItem = new MenuItem("Copy");
            copyMenuItem.Clicked += (sender, e) => te1.Copy();
            editMenu.SubMenu.Items.Add(copyMenuItem);

            pasteMenuItem = new MenuItem("Paste");
            pasteMenuItem.Clicked += (sender, e) => te1.Paste();
            editMenu.SubMenu.Items.Add(pasteMenuItem);

            editMenu.SubMenu.Items.Add(new SeparatorMenuItem());

            selectAllMenuItem = new MenuItem("Select All");
            selectAllMenuItem.Clicked += (sender, e) => te1.SelectAll();
            editMenu.SubMenu.Items.Add(selectAllMenuItem);

            var buildMenu = new MenuItem("Build");
            buildMenu.SubMenu = new Menu();
            menu.Items.Add(buildMenu);

            runMenuItem = new MenuItem("Run");
            runMenuItem.Clicked += (sender, e) => Run();
            buildMenu.SubMenu.Items.Add(runMenuItem);

            debugMenuItem = new CheckBoxMenuItem("Debug");
            debugMenuItem.Checked = true;
            buildMenu.SubMenu.Items.Add(debugMenuItem);

            var helpMenu = new MenuItem("Help");
            helpMenu.SubMenu = new Menu();
            menu.Items.Add(helpMenu);

            aboutMenuItem = new MenuItem("About");
            aboutMenuItem.Clicked += (sender, e) => ShowAboutDialog();
            helpMenu.SubMenu.Items.Add(aboutMenuItem);

            MainMenu = menu;
        }
    }
}

