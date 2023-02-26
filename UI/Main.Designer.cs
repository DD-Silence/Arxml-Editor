namespace ArxmlEditor
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            this.tvContent = new System.Windows.Forms.TreeView();
            this.cmMember = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // tvContent
            // 
            this.tvContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvContent.Location = new System.Drawing.Point(0, 0);
            this.tvContent.Name = "tvContent";
            this.tvContent.Size = new System.Drawing.Size(1006, 721);
            this.tvContent.TabIndex = 0;
            this.tvContent.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvContent_BeforeExpand);
            this.tvContent.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvContent_MouseClick);
            // 
            // cmMember
            // 
            this.cmMember.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmMember.Name = "cmMember";
            this.cmMember.Size = new System.Drawing.Size(61, 4);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 721);
            this.Controls.Add(this.tvContent);
            this.Name = "Main";
            this.Text = "ArxmlEditor";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeView tvContent;
        private ContextMenuStrip cmMember;
    }
}