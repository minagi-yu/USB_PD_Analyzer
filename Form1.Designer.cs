namespace USB_PD_Analyzer
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.toolBar = new System.Windows.Forms.ToolStrip();
			this.comPortSelecter = new System.Windows.Forms.ToolStripComboBox();
			this.comPortConnectButton = new System.Windows.Forms.ToolStripButton();
			this.comPortDisonnectButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.clearButton = new System.Windows.Forms.ToolStripButton();
			this.statusBar = new System.Windows.Forms.StatusStrip();
			this.statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.timeLine = new System.Windows.Forms.ListView();
			this.timeLineHeader_N = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeLineHeader_Preamble = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeLineHeader_SOP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeLineHeader_SOPx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeLineHeader_Header = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeLineHeader_MsgID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeLineHeader_PowerRole = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeLineHeader_DataRole = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeLineHeader_MsgType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeLineHeader_DataObjs = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeLineHeader_Data = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeLineHeader_EOP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.detailView = new System.Windows.Forms.ListView();
			this.detailViewHeader_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.detailViewHeader_Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.serialConsole = new System.Windows.Forms.TextBox();
			this.serialPort = new System.IO.Ports.SerialPort(this.components);
			this.toolBar.SuspendLayout();
			this.statusBar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolBar
			// 
			this.toolBar.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.comPortSelecter,
            this.comPortConnectButton,
            this.comPortDisonnectButton,
            this.toolStripSeparator1,
            this.clearButton});
			this.toolBar.Location = new System.Drawing.Point(0, 0);
			this.toolBar.Name = "toolBar";
			this.toolBar.Size = new System.Drawing.Size(800, 34);
			this.toolBar.TabIndex = 0;
			this.toolBar.Text = "toolStrip1";
			// 
			// comPortSelecter
			// 
			this.comPortSelecter.Name = "comPortSelecter";
			this.comPortSelecter.Size = new System.Drawing.Size(121, 34);
			this.comPortSelecter.DropDown += new System.EventHandler(this.ComPortSelecter_DropDown);
			// 
			// comPortConnectButton
			// 
			this.comPortConnectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.comPortConnectButton.Image = ((System.Drawing.Image)(resources.GetObject("comPortConnectButton.Image")));
			this.comPortConnectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.comPortConnectButton.Name = "comPortConnectButton";
			this.comPortConnectButton.Size = new System.Drawing.Size(81, 29);
			this.comPortConnectButton.Text = "Connect";
			this.comPortConnectButton.ToolTipText = "Connect";
			this.comPortConnectButton.Click += new System.EventHandler(this.ComPortConnectButton_Click);
			// 
			// comPortDisonnectButton
			// 
			this.comPortDisonnectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.comPortDisonnectButton.Image = ((System.Drawing.Image)(resources.GetObject("comPortDisonnectButton.Image")));
			this.comPortDisonnectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.comPortDisonnectButton.Name = "comPortDisonnectButton";
			this.comPortDisonnectButton.RightToLeftAutoMirrorImage = true;
			this.comPortDisonnectButton.Size = new System.Drawing.Size(103, 29);
			this.comPortDisonnectButton.Text = "Disconnect";
			this.comPortDisonnectButton.Click += new System.EventHandler(this.ComPortDisonnectButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 34);
			// 
			// clearButton
			// 
			this.clearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.clearButton.Image = ((System.Drawing.Image)(resources.GetObject("clearButton.Image")));
			this.clearButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(55, 29);
			this.clearButton.Text = "Clear";
			this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
			// 
			// statusBar
			// 
			this.statusBar.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel1});
			this.statusBar.Location = new System.Drawing.Point(0, 428);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(800, 22);
			this.statusBar.TabIndex = 1;
			this.statusBar.Text = "statusStrip1";
			// 
			// statusLabel1
			// 
			this.statusLabel1.Name = "statusLabel1";
			this.statusLabel1.Size = new System.Drawing.Size(0, 15);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 34);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.serialConsole);
			this.splitContainer1.Size = new System.Drawing.Size(800, 394);
			this.splitContainer1.SplitterDistance = 252;
			this.splitContainer1.TabIndex = 2;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.timeLine);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.detailView);
			this.splitContainer2.Size = new System.Drawing.Size(800, 252);
			this.splitContainer2.SplitterDistance = 436;
			this.splitContainer2.TabIndex = 0;
			// 
			// timeLine
			// 
			this.timeLine.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timeLineHeader_N,
            this.timeLineHeader_Preamble,
            this.timeLineHeader_SOP,
            this.timeLineHeader_SOPx,
            this.timeLineHeader_Header,
            this.timeLineHeader_MsgID,
            this.timeLineHeader_PowerRole,
            this.timeLineHeader_DataRole,
            this.timeLineHeader_MsgType,
            this.timeLineHeader_DataObjs,
            this.timeLineHeader_Data,
            this.timeLineHeader_EOP});
			this.timeLine.Dock = System.Windows.Forms.DockStyle.Fill;
			this.timeLine.HideSelection = false;
			this.timeLine.Location = new System.Drawing.Point(0, 0);
			this.timeLine.MultiSelect = false;
			this.timeLine.Name = "timeLine";
			this.timeLine.Size = new System.Drawing.Size(436, 252);
			this.timeLine.TabIndex = 0;
			this.timeLine.UseCompatibleStateImageBehavior = false;
			this.timeLine.View = System.Windows.Forms.View.Details;
			// 
			// timeLineHeader_N
			// 
			this.timeLineHeader_N.Text = "#";
			this.timeLineHeader_N.Width = 38;
			// 
			// timeLineHeader_Preamble
			// 
			this.timeLineHeader_Preamble.Text = "Preamble";
			// 
			// timeLineHeader_SOP
			// 
			this.timeLineHeader_SOP.Text = "SOP";
			// 
			// timeLineHeader_SOPx
			// 
			this.timeLineHeader_SOPx.Text = "SOP*";
			this.timeLineHeader_SOPx.Width = 56;
			// 
			// timeLineHeader_Header
			// 
			this.timeLineHeader_Header.Text = "Header";
			// 
			// timeLineHeader_MsgID
			// 
			this.timeLineHeader_MsgID.Text = "Message ID";
			this.timeLineHeader_MsgID.Width = 97;
			// 
			// timeLineHeader_PowerRole
			// 
			this.timeLineHeader_PowerRole.Text = "Power Role";
			this.timeLineHeader_PowerRole.Width = 97;
			// 
			// timeLineHeader_DataRole
			// 
			this.timeLineHeader_DataRole.Text = "Data Role";
			this.timeLineHeader_DataRole.Width = 86;
			// 
			// timeLineHeader_MsgType
			// 
			this.timeLineHeader_MsgType.Text = "Message Type";
			this.timeLineHeader_MsgType.Width = 119;
			// 
			// timeLineHeader_DataObjs
			// 
			this.timeLineHeader_DataObjs.Text = "Data Objects";
			this.timeLineHeader_DataObjs.Width = 111;
			// 
			// timeLineHeader_Data
			// 
			this.timeLineHeader_Data.Text = "Data";
			this.timeLineHeader_Data.Width = 48;
			// 
			// timeLineHeader_EOP
			// 
			this.timeLineHeader_EOP.Text = "EOP";
			// 
			// detailView
			// 
			this.detailView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.detailViewHeader_Name,
            this.detailViewHeader_Value});
			this.detailView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.detailView.HideSelection = false;
			this.detailView.Location = new System.Drawing.Point(0, 0);
			this.detailView.Name = "detailView";
			this.detailView.Size = new System.Drawing.Size(360, 252);
			this.detailView.TabIndex = 0;
			this.detailView.UseCompatibleStateImageBehavior = false;
			this.detailView.View = System.Windows.Forms.View.Details;
			// 
			// detailViewHeader_Name
			// 
			this.detailViewHeader_Name.Text = "Name";
			this.detailViewHeader_Name.Width = 55;
			// 
			// detailViewHeader_Value
			// 
			this.detailViewHeader_Value.Text = "Value";
			this.detailViewHeader_Value.Width = 113;
			// 
			// serialConsole
			// 
			this.serialConsole.BackColor = System.Drawing.Color.Black;
			this.serialConsole.Dock = System.Windows.Forms.DockStyle.Fill;
			this.serialConsole.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.serialConsole.ForeColor = System.Drawing.Color.LightGray;
			this.serialConsole.Location = new System.Drawing.Point(0, 0);
			this.serialConsole.Multiline = true;
			this.serialConsole.Name = "serialConsole";
			this.serialConsole.ReadOnly = true;
			this.serialConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.serialConsole.Size = new System.Drawing.Size(800, 138);
			this.serialConsole.TabIndex = 0;
			this.serialConsole.WordWrap = false;
			// 
			// serialPort
			// 
			this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort_DataReceived);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.toolBar);
			this.Name = "Form1";
			this.Text = "USB Power Delivery Analyzer";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.toolBar.ResumeLayout(false);
			this.toolBar.PerformLayout();
			this.statusBar.ResumeLayout(false);
			this.statusBar.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.ToolStrip toolBar;
		private System.Windows.Forms.ToolStripComboBox comPortSelecter;
		private System.Windows.Forms.ToolStripButton comPortConnectButton;
		private System.Windows.Forms.StatusStrip statusBar;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.ListView timeLine;
		private System.Windows.Forms.ListView detailView;
		private System.Windows.Forms.TextBox serialConsole;
		private System.IO.Ports.SerialPort serialPort;
		private System.Windows.Forms.ToolStripButton comPortDisonnectButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton clearButton;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel1;
		private System.Windows.Forms.ColumnHeader timeLineHeader_N;
		private System.Windows.Forms.ColumnHeader timeLineHeader_SOPx;
		private System.Windows.Forms.ColumnHeader timeLineHeader_MsgID;
		private System.Windows.Forms.ColumnHeader timeLineHeader_PowerRole;
		private System.Windows.Forms.ColumnHeader timeLineHeader_DataRole;
		private System.Windows.Forms.ColumnHeader timeLineHeader_MsgType;
		private System.Windows.Forms.ColumnHeader timeLineHeader_DataObjs;
		private System.Windows.Forms.ColumnHeader timeLineHeader_Data;
		private System.Windows.Forms.ColumnHeader detailViewHeader_Name;
		private System.Windows.Forms.ColumnHeader detailViewHeader_Value;
		private System.Windows.Forms.ColumnHeader timeLineHeader_Preamble;
		private System.Windows.Forms.ColumnHeader timeLineHeader_SOP;
		private System.Windows.Forms.ColumnHeader timeLineHeader_Header;
		private System.Windows.Forms.ColumnHeader timeLineHeader_EOP;
	}
}

