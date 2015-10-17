using System;
using Xwt;

namespace URMSimulator
{
    public partial class AboutDialog: Dialog
    {
        public string ProgramName { set { labelProgramName.Text = value; } }
        public string Comments { set { labelComments.Text = value; } }
        public string Website { set { labelWebsite.Uri = new Uri(value); } }
        public string WebsiteLabel { set { labelWebsite.Text = value; } }
        public Xwt.Drawing.Image Image { set { image1.Image = value; } }

        public AboutDialog()
        {
            Build();
        }
    }
}
