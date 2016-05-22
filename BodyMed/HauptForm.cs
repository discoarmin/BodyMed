namespace BodyMed
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class HauptForm : RibbonForm
    {
        public HauptForm()
        {
            this.InitializeComponent();
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //}

        private void RibbonButton1Click(object sender, EventArgs e)
        {
            //if (this.ActiveMdiChild != null)
            while (this.ActiveMdiChild != null)
            {
                this.ActiveMdiChild.Close();
            }
        }

        private void RibbonButton2Click(object sender, EventArgs e)
        {
            //foreach (var f in this.MdiChildren.Where(f => f.GetType() == typeof(MdiChild1)))
            //{
            //    f.Activate();
            //    return;
            //}

            //Form form = new MdiChild1();
            //form.MdiParent = this;
            //form.Show();
        }

        private void RibbonButton3Click(object sender, EventArgs e)
        {
            //foreach (Form f in this.MdiChildren)
            //{
            //    if (f.GetType() == typeof(MdiChild2))
            //    {
            //        f.Activate();                         
            //        return;
            //    }
            //}

            //Form form = new MdiChild2();
            //form.MdiParent = this;
            //form.Show();
        }
    }
}