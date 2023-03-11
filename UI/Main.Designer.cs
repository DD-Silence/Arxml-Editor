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
            components = new System.ComponentModel.Container();
            tvContent = new TreeView();
            cmMember = new ContextMenuStrip(components);
            sc = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)sc).BeginInit();
            sc.Panel1.SuspendLayout();
            sc.SuspendLayout();
            SuspendLayout();
            // 
            // tvContent
            // 
            tvContent.Dock = DockStyle.Fill;
            tvContent.Location = new Point(0, 0);
            tvContent.Name = "tvContent";
            tvContent.Size = new Size(335, 721);
            tvContent.TabIndex = 0;
            tvContent.BeforeExpand += tvContent_BeforeExpand;
            tvContent.MouseClick += tvContent_MouseClick;
            // 
            // cmMember
            // 
            cmMember.ImageScalingSize = new Size(20, 20);
            cmMember.Name = "cmMember";
            cmMember.Size = new Size(61, 4);
            // 
            // sc
            // 
            sc.Dock = DockStyle.Fill;
            sc.Location = new Point(0, 0);
            sc.Name = "sc";
            // 
            // sc.Panel1
            // 
            sc.Panel1.Controls.Add(tvContent);
            sc.Size = new Size(1006, 721);
            sc.SplitterDistance = 335;
            sc.TabIndex = 1;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1006, 721);
            Controls.Add(sc);
            Name = "Main";
            Text = "ArxmlEditor";
            Load += Main_Load;
            sc.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)sc).EndInit();
            sc.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TreeView tvContent;
        private ContextMenuStrip cmMember;
        private SplitContainer sc;
    }
}