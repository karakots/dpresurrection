namespace HouseholdManager
{
    partial class MediaBuilder
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.updateMedia = new System.Windows.Forms.Button();
            this.mediaTypeBox = new System.Windows.Forms.ListView();
            this.readFile = new System.Windows.Forms.Button();
            this.RefreshAdOptions = new System.Windows.Forms.Button();
            this.build_options = new System.Windows.Forms.Button();
            this.calVehicleButton = new System.Windows.Forms.Button();
            this.calOptionsButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.OptionWrite = new System.Windows.Forms.Button();
            this.mediaToFIleButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.regionView = new System.Windows.Forms.TreeView();
            this.updateDPMButton = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // updateMedia
            // 
            this.updateMedia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.updateMedia.Enabled = false;
            this.updateMedia.Location = new System.Drawing.Point( 7, 196 );
            this.updateMedia.Name = "updateMedia";
            this.updateMedia.Size = new System.Drawing.Size( 116, 23 );
            this.updateMedia.TabIndex = 0;
            this.updateMedia.Text = "Update from db";
            this.updateMedia.UseVisualStyleBackColor = true;
            this.updateMedia.Click += new System.EventHandler( this.updateMedia_Click );
            // 
            // mediaTypeBox
            // 
            this.mediaTypeBox.CheckBoxes = true;
            this.mediaTypeBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.mediaTypeBox.Location = new System.Drawing.Point( 0, 0 );
            this.mediaTypeBox.Name = "mediaTypeBox";
            this.mediaTypeBox.Size = new System.Drawing.Size( 131, 185 );
            this.mediaTypeBox.TabIndex = 5;
            this.mediaTypeBox.UseCompatibleStateImageBehavior = false;
            this.mediaTypeBox.View = System.Windows.Forms.View.List;
            // 
            // readFile
            // 
            this.readFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.readFile.Enabled = false;
            this.readFile.Location = new System.Drawing.Point( 7, 225 );
            this.readFile.Name = "readFile";
            this.readFile.Size = new System.Drawing.Size( 116, 23 );
            this.readFile.TabIndex = 6;
            this.readFile.Text = "Update from file";
            this.readFile.UseVisualStyleBackColor = true;
            this.readFile.Click += new System.EventHandler( this.readFile_Click );
            // 
            // RefreshAdOptions
            // 
            this.RefreshAdOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshAdOptions.Enabled = false;
            this.RefreshAdOptions.Location = new System.Drawing.Point( 7, 254 );
            this.RefreshAdOptions.Name = "RefreshAdOptions";
            this.RefreshAdOptions.Size = new System.Drawing.Size( 116, 23 );
            this.RefreshAdOptions.TabIndex = 7;
            this.RefreshAdOptions.Text = "Refresh AdOptions";
            this.RefreshAdOptions.UseVisualStyleBackColor = true;
            this.RefreshAdOptions.Click += new System.EventHandler( this.RefreshAdOptions_Click );
            // 
            // build_options
            // 
            this.build_options.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.build_options.Location = new System.Drawing.Point( 7, 283 );
            this.build_options.Name = "build_options";
            this.build_options.Size = new System.Drawing.Size( 116, 23 );
            this.build_options.TabIndex = 8;
            this.build_options.Text = "Fix AdOptions";
            this.build_options.UseVisualStyleBackColor = true;
            this.build_options.Click += new System.EventHandler( this.build_options_Click );
            // 
            // calVehicleButton
            // 
            this.calVehicleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.calVehicleButton.Location = new System.Drawing.Point( 12, 14 );
            this.calVehicleButton.Name = "calVehicleButton";
            this.calVehicleButton.Size = new System.Drawing.Size( 124, 23 );
            this.calVehicleButton.TabIndex = 9;
            this.calVehicleButton.Text = "Vehicle Calibrate";
            this.calVehicleButton.UseVisualStyleBackColor = true;
            this.calVehicleButton.Click += new System.EventHandler( this.calVehicleButton_Click );
            // 
            // calOptionsButton
            // 
            this.calOptionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.calOptionsButton.Location = new System.Drawing.Point( 13, 73 );
            this.calOptionsButton.Name = "calOptionsButton";
            this.calOptionsButton.Size = new System.Drawing.Size( 122, 23 );
            this.calOptionsButton.TabIndex = 10;
            this.calOptionsButton.Text = "Import Options";
            this.calOptionsButton.UseVisualStyleBackColor = true;
            this.calOptionsButton.Click += new System.EventHandler( this.calOptionsButton_Click );
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.OptionWrite );
            this.splitContainer1.Panel1.Controls.Add( this.mediaToFIleButton );
            this.splitContainer1.Panel1.Controls.Add( this.mediaTypeBox );
            this.splitContainer1.Panel1.Controls.Add( this.readFile );
            this.splitContainer1.Panel1.Controls.Add( this.updateMedia );
            this.splitContainer1.Panel1.Controls.Add( this.build_options );
            this.splitContainer1.Panel1.Controls.Add( this.RefreshAdOptions );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.groupBox1 );
            this.splitContainer1.Size = new System.Drawing.Size( 571, 375 );
            this.splitContainer1.SplitterDistance = 131;
            this.splitContainer1.TabIndex = 11;
            // 
            // OptionWrite
            // 
            this.OptionWrite.Location = new System.Drawing.Point( 7, 341 );
            this.OptionWrite.Name = "OptionWrite";
            this.OptionWrite.Size = new System.Drawing.Size( 116, 23 );
            this.OptionWrite.TabIndex = 10;
            this.OptionWrite.Text = "convert";
            this.OptionWrite.UseVisualStyleBackColor = true;
            this.OptionWrite.Click += new System.EventHandler( this.OptionWrite_Click );
            // 
            // mediaToFIleButton
            // 
            this.mediaToFIleButton.Location = new System.Drawing.Point( 7, 312 );
            this.mediaToFIleButton.Name = "mediaToFIleButton";
            this.mediaToFIleButton.Size = new System.Drawing.Size( 116, 23 );
            this.mediaToFIleButton.TabIndex = 9;
            this.mediaToFIleButton.Text = "Write Media to File";
            this.mediaToFIleButton.UseVisualStyleBackColor = true;
            this.mediaToFIleButton.Click += new System.EventHandler( this.mediaToFIleButton_Click );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.splitContainer2 );
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point( 0, 0 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 436, 375 );
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Calibration";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point( 3, 16 );
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.regionView );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.updateDPMButton );
            this.splitContainer2.Panel2.Controls.Add( this.calVehicleButton );
            this.splitContainer2.Panel2.Controls.Add( this.calOptionsButton );
            this.splitContainer2.Size = new System.Drawing.Size( 430, 356 );
            this.splitContainer2.SplitterDistance = 275;
            this.splitContainer2.TabIndex = 11;
            // 
            // regionView
            // 
            this.regionView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regionView.Location = new System.Drawing.Point( 0, 0 );
            this.regionView.Name = "regionView";
            this.regionView.Size = new System.Drawing.Size( 275, 356 );
            this.regionView.TabIndex = 1;
            // 
            // updateDPMButton
            // 
            this.updateDPMButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updateDPMButton.Location = new System.Drawing.Point( 13, 167 );
            this.updateDPMButton.Name = "updateDPMButton";
            this.updateDPMButton.Size = new System.Drawing.Size( 124, 23 );
            this.updateDPMButton.TabIndex = 11;
            this.updateDPMButton.Text = "Update Media CPM";
            this.updateDPMButton.UseVisualStyleBackColor = true;
            this.updateDPMButton.Click += new System.EventHandler( this.updateDPMButton_Click );
            // 
            // MediaBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainer1 );
            this.Name = "MediaBuilder";
            this.Size = new System.Drawing.Size( 571, 375 );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.groupBox1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button updateMedia;
        private System.Windows.Forms.ListView mediaTypeBox;
        private System.Windows.Forms.Button readFile;
        private System.Windows.Forms.Button RefreshAdOptions;
        private System.Windows.Forms.Button build_options;
        private System.Windows.Forms.Button calVehicleButton;
        private System.Windows.Forms.Button calOptionsButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView regionView;
        private System.Windows.Forms.Button mediaToFIleButton;
        private System.Windows.Forms.Button updateDPMButton;
        private System.Windows.Forms.Button OptionWrite;
    }
}
