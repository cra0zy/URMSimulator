using Xwt;
using Xwt.Drawing;

namespace URMSimulator
{
    public partial class AboutDialog
    {
        VBox vbox1;
        HBox hbox1;
        ImageView image1;
        Label labelProgramName, labelComments;
        LinkLabel labelWebsite;

        private void Build()
        {
            this.Icon = Xwt.Drawing.Image.FromResource("URMSimulator.Resources.urm.png");
            this.Title = "About";
            this.Resizable = false;
            this.Buttons.Add(new DialogButton(Command.Close));

            vbox1 = new VBox();

            image1 = new ImageView();
            image1.WidthRequest = 320;
            image1.HeightRequest = 270;
            vbox1.PackStart(image1);

            labelProgramName = new Label();
            labelProgramName.TextAlignment = Alignment.Center;
            vbox1.PackStart(labelProgramName);

            labelComments = new Label();
            labelComments.TextAlignment = Alignment.Center;
            vbox1.PackStart(labelComments);

            hbox1 = new HBox();

            hbox1.PackStart(new HBox(), true);

            labelWebsite = new LinkLabel();
            labelWebsite.TextAlignment = Alignment.Center; //text aligment doesn't work with Xwt.WPF
            hbox1.PackStart(labelWebsite, false);

            hbox1.PackStart(new HBox(), true);
            vbox1.PackStart(hbox1);

            this.Content = vbox1;
        }
    }
}
