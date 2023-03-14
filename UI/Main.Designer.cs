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
            fpContent = new FlowLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            scVertical = new SplitContainer();
            scHorizantal2 = new SplitContainer();
            tbBreif = new TextBox();
            tbOutput = new TextBox();
            ((System.ComponentModel.ISupportInitialize)scHorizantal1).BeginInit();
            scHorizantal1.Panel1.SuspendLayout();
            scHorizantal1.Panel2.SuspendLayout();
            scHorizantal1.SuspendLayout();
            fpContent.SuspendLayout();
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
            tvContent.Name = "tvContent";
            tvContent.Size = new Size(700, 900);
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
            scHorizantal1.Name = "scHorizantal1";
            // 
            // scHorizantal1.Panel1
            // 
            scHorizantal1.Panel1.Controls.Add(tvContent);
            // 
            // scHorizantal1.Panel2
            // 
            scHorizantal1.Panel2.Controls.Add(fpContent);
            scHorizantal1.Size = new Size(1582, 900);
            scHorizantal1.SplitterDistance = 700;
            scHorizantal1.TabIndex = 1;
            // 
            // fpContent
            // 
            fpContent.Controls.Add(tableLayoutPanel1);
            fpContent.Dock = DockStyle.Fill;
            fpContent.Location = new Point(0, 0);
            fpContent.Name = "fpContent";
            fpContent.Size = new Size(878, 900);
            fpContent.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(709, 0);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // scVertical
            // 
            scVertical.Dock = DockStyle.Fill;
            scVertical.Location = new Point(0, 0);
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
            scVertical.Size = new Size(1582, 1153);
            scVertical.SplitterDistance = 900;
            scVertical.TabIndex = 2;
            // 
            // scHorizantal2
            // 
            scHorizantal2.Dock = DockStyle.Fill;
            scHorizantal2.Location = new Point(0, 0);
            scHorizantal2.Name = "scHorizantal2";
            // 
            // scHorizantal2.Panel1
            // 
            scHorizantal2.Panel1.Controls.Add(tbBreif);
            // 
            // scHorizantal2.Panel2
            // 
            scHorizantal2.Panel2.Controls.Add(tbOutput);
            scHorizantal2.Size = new Size(1582, 249);
            scHorizantal2.SplitterDistance = 500;
            scHorizantal2.TabIndex = 0;
            // 
            // tbBreif
            // 
            tbBreif.Dock = DockStyle.Fill;
            tbBreif.Enabled = false;
            tbBreif.Location = new Point(0, 0);
            tbBreif.Multiline = true;
            tbBreif.Name = "tbBreif";
            tbBreif.Size = new Size(500, 249);
            tbBreif.TabIndex = 0;
            // 
            // tbOutput
            // 
            tbOutput.Dock = DockStyle.Fill;
            tbOutput.Enabled = false;
            tbOutput.Location = new Point(0, 0);
            tbOutput.Multiline = true;
            tbOutput.Name = "tbOutput";
            tbOutput.Size = new Size(1078, 249);
            tbOutput.TabIndex = 0;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1582, 1153);
            Controls.Add(scVertical);
            Name = "Main";
            Text = "ArxmlEditor";
            Load += Main_Load;
            scHorizantal1.Panel1.ResumeLayout(false);
            scHorizantal1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scHorizantal1).EndInit();
            scHorizantal1.ResumeLayout(false);
            fpContent.ResumeLayout(false);
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
        private FlowLayoutPanel fpContent;
        private TableLayoutPanel tableLayoutPanel1;
    }
}