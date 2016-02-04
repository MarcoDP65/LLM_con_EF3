using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace MdiHelper
{
    public class MdiParent : Form
    {
        private Form callingsender = null;
        private event EventHandler<DialogResultArgs> DialogReturning;

        public MdiParent()
        {
            this.IsMdiContainer = true;
        }

        public void ShowChildDialog(Form frm, EventHandler<DialogResultArgs> DialogReturnedValue)
        {
            frm.MdiParent = this;
            frm.MaximizeBox = false;
            frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
            callingsender = frm;
            DialogReturning += DialogReturnedValue;
            DisableControls();
            frm.Show();
        }

        private void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((Form)sender).FormClosed -= new FormClosedEventHandler(frm_FormClosed);
            DialogReturned(callingsender, new DialogResultArgs(((Form)sender).DialogResult));
            ForceReleaseOfControls();
        }

        public virtual void DialogReturned(object sender, DialogResultArgs DialogReturnedValue)
        {
            if(DialogReturning != null)
                DialogReturning(sender, DialogReturnedValue);
            DialogReturning = null;
        }

        private void DisableControls()
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i].GetType() != typeof(MdiClient))
                    this.Controls[i].Enabled = false;
            }
            foreach (Form frm in MdiChildren)
                frm.Enabled = false;
            if(callingsender != null)
                callingsender.Enabled = true;
        }

        public void ForceReleaseOfControls()
        {
            for (int i = 0; i < this.Controls.Count; i++)
                this.Controls[i].Enabled = true;
            
            foreach (Form frm in MdiChildren)
                frm.Enabled = true;
        }

        public class DialogResultArgs : EventArgs
        {
            private DialogResult _Result;
            /// <summary>
            /// Returns DialogResult from the dialog form.
            /// </summary>
            [Description("Get DialogResult returned by the dialog form")]
            [Category("Property")]
            public DialogResult Result
            {
                get { return _Result; }
            }
            public DialogResultArgs(DialogResult dr)
            {
                _Result = dr;
            }
        }
    }
}
