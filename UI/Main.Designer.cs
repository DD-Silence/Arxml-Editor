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
            scHorizantal1 = new SplitContainer();
            tpContent = new TableLayoutPanel();
            scVertical = new SplitContainer();
            scHorizantal2 = new SplitContainer();
            tbBreif = new TextBox();
            tbOutput = new TextBox();
            ((System.ComponentModel.ISupportInitialize)scHorizantal1).BeginInit();
            scHorizantal1.Panel1.SuspendLayout();
            scHorizantal1.Panel2.SuspendLayout();
            scHorizantal1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scVertical).BeginInit();
            scVertical.Panel1.SuspendLayout();
            scVertical.Panel2.SuspendLayout();
            scVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scHorizantal2).BeginInit();
            scHorizantal2.Panel1.SuspendLayout();
            scHorizantal2.Panel2.SuspendLayout();
            scHorizantal2.SuspendLayout();
            SuspendLayout();
            // 
            // tvContent
            // 
            tvContent.Dock = DockStyle.Fill;
            tvContent.Location = new Point(0, 0);
            tvContent.Margin = new Padding(3, 4, 3, 4);
            tvContent.Name = "tvContent";
            tvContent.Size = new Size(699, 883);
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
            // scHorizantal1
            // 
            scHorizantal1.Dock = DockStyle.Fill;
            scHorizantal1.Location = new Point(0, 0);
            scHorizantal1.Margin = new Padding(3, 4, 3, 4);
            scHorizantal1.Name = "scHorizantal1";
            // 
            // scHorizantal1.Panel1
            // 
            scHorizantal1.Panel1.Controls.Add(tvContent);
            // 
            // scHorizantal1.Panel2
            // 
            scHorizantal1.Panel2.Controls.Add(tpContent);
            scHorizantal1.Size = new Size(1581, 883);
            scHorizantal1.SplitterDistance = 699;
            scHorizantal1.TabIndex = 1;
            // 
            // tpContent
            // 
            tpContent.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tpContent.ColumnCount = 2;
            tpContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
            tpContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62.5F));
            tpContent.Dock = DockStyle.Fill;
            tpContent.Location = new Point(0, 0);
            tpContent.Margin = new Padding(3, 4, 3, 4);
            tpContent.Name = "tpContent";
            tpContent.RowCount = 1;
            tpContent.RowStyles.Add(new RowStyle());
            tpContent.Size = new Size(878, 883);
            tpContent.TabIndex = 0;
            // 
            // scVertical
            // 
            scVertical.Dock = DockStyle.Fill;
            scVertical.Location = new Point(0, 0);
            scVertical.Margin = new Padding(3, 4, 3, 4);
            scVertical.Name = "scVertical";
            scVertical.Orientation = Orientation.Horizontal;
            // 
            // scVertical.Panel1
            // 
            scVertical.Panel1.Controls.Add(scHorizantal1);
            // 
            // scVertical.Panel2
            // 
            scVertical.Panel2.Controls.Add(scHorizantal2);
            scVertical.Size = new Size(1581, 1133);
            scVertical.SplitterDistance = 883;
            scVertical.TabIndex = 2;
            // 
            // scHorizantal2
            // 
            scHorizantal2.Dock = DockStyle.Fill;
            scHorizantal2.Location = new Point(0, 0);
            scHorizantal2.Margin = new Padding(3, 4, 3, 4);
            scHorizantal2.Name = "scHorizantal2";
            // 
            // scHorizantal2.Panel1
            // 
            scHorizantal2.Panel1.Controls.Add(tbBreif);
            // 
            // scHorizantal2.Panel2
            // 
            scHorizantal2.Panel2.Controls.Add(tbOutput);
            scHorizantal2.Size = new Size(1581, 246);
            scHorizantal2.SplitterDistance = 498;
            scHorizantal2.TabIndex = 0;
            // 
            // tbBreif
            // 
            tbBreif.Dock = DockStyle.Fill;
            tbBreif.Location = new Point(0, 0);
            tbBreif.Margin = new Padding(3, 4, 3, 4);
            tbBreif.Multiline = true;
            tbBreif.Name = "tbBreif";
            tbBreif.ReadOnly = true;
            tbBreif.ScrollBars = ScrollBars.Vertical;
            tbBreif.Size = new Size(498, 246);
            tbBreif.TabIndex = 0;
            // 
            // tbOutput
            // 
            tbOutput.Dock = DockStyle.Fill;
            tbOutput.Location = new Point(0, 0);
            tbOutput.Margin = new Padding(3, 4, 3, 4);
            tbOutput.Multiline = true;
            tbOutput.Name = "tbOutput";
            tbOutput.ReadOnly = true;
            tbOutput.ScrollBars = ScrollBars.Vertical;
            tbOutput.Size = new Size(1079, 246);
            tbOutput.TabIndex = 0;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1581, 1133);
            Controls.Add(scVertical);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Main";
            Text = "ArxmlEditor";
            Load += Main_Load;
            scHorizantal1.Panel1.ResumeLayout(false);
            scHorizantal1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scHorizantal1).EndInit();
            scHorizantal1.ResumeLayout(false);
            scVertical.Panel1.ResumeLayout(false);
            scVertical.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scVertical).EndInit();
            scVertical.ResumeLayout(false);
            scHorizantal2.Panel1.ResumeLayout(false);
            scHorizantal2.Panel1.PerformLayout();
            scHorizantal2.Panel2.ResumeLayout(false);
            scHorizantal2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)scHorizantal2).EndInit();
            scHorizantal2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TreeView tvContent;
        private ContextMenuStrip cmMember;
        private SplitContainer scHorizantal1;
        private SplitContainer scVertical;
        private SplitContainer scHorizantal2;
        private TextBox tbBreif;
        private TextBox tbOutput;
        private TableLayoutPanel tpContent;
    }
}