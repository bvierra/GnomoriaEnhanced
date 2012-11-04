using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GnomoriaEnhanced.UI
{
    public partial class TabControl : System.Windows.Forms.TabControl
    {
        protected override void OnHandleCreated(EventArgs e_)
        {
            base.OnHandleCreated(e_);
            foreach (System.Windows.Forms.TabPage tabPage in TabPages)
            {
                InitializeTabPage(tabPage, true, Created);
            }
            Console.WriteLine("Tab Created!");
        }

        protected override void OnControlAdded(ControlEventArgs e_)
        {
            base.OnControlAdded(e_);
            System.Windows.Forms.TabPage page = e_.Control as System.Windows.Forms.TabPage;
            if ((page != null) && (page.Parent == this) && (IsHandleCreated || Created))
            {
                InitializeTabPage(page, IsHandleCreated, Created);
            }
            Console.WriteLine("Tab Created!");
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            foreach (System.Windows.Forms.TabPage tabPage in TabPages)
            {
                InitializeTabPage(tabPage, IsHandleCreated, true);
            }
            Console.WriteLine("Tab Created!");
        }

        //PRB: Exception thrown during Windows Forms data binding if bound control is on a tab page with uncreated handle
        //FIX: Make sure all tab pages are created when the tabcontrol is created.
        //https://connect.microsoft.com/VisualStudio/feedback/details/351177
        private void InitializeTabPage(System.Windows.Forms.TabPage page_, bool createHandle_, bool createControl_)
        {
            if (!createControl_ && !createHandle_)
            {
                return;
            }
            if (createHandle_ && !page_.IsHandleCreated)
            {
                IntPtr handle = page_.Handle;
            }
            if (!page_.Created && createControl_)
            {
                return;
            }
            bool visible = page_.Visible;
            if (!visible)
            {
                page_.Visible = true;
            }
            page_.CreateControl();
            if (!visible)
            {
                page_.Visible = false;
            }
            Console.WriteLine("Tab Initialized!");
        }
    }
}
