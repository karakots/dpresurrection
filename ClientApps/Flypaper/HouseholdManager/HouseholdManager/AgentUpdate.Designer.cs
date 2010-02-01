namespace HouseholdManager
{
    partial class AgentUpdate
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
            this.updateAgents = new System.Windows.Forms.Button();
            this.mediaTypeBox = new System.Windows.Forms.ListView();
            this.saveAgentsBut = new System.Windows.Forms.Button();
            this.testPenetration = new System.Windows.Forms.Button();
            this.loadAgentsButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.agentInfo = new System.Windows.Forms.TextBox();
            this.regUpdate = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // updateAgents
            // 
            this.updateAgents.Location = new System.Drawing.Point( 35, 42 );
            this.updateAgents.Name = "updateAgents";
            this.updateAgents.Size = new System.Drawing.Size( 117, 23 );
            this.updateAgents.TabIndex = 0;
            this.updateAgents.Text = "update agents";
            this.updateAgents.UseVisualStyleBackColor = true;
            this.updateAgents.Click += new System.EventHandler( this.updateAgents_Click );
            // 
            // mediaTypeBox
            // 
            this.mediaTypeBox.CheckBoxes = true;
            this.mediaTypeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaTypeBox.Location = new System.Drawing.Point( 0, 0 );
            this.mediaTypeBox.Name = "mediaTypeBox";
            this.mediaTypeBox.Size = new System.Drawing.Size( 180, 129 );
            this.mediaTypeBox.TabIndex = 6;
            this.mediaTypeBox.UseCompatibleStateImageBehavior = false;
            this.mediaTypeBox.View = System.Windows.Forms.View.List;
            // 
            // saveAgentsBut
            // 
            this.saveAgentsBut.Location = new System.Drawing.Point( 35, 100 );
            this.saveAgentsBut.Name = "saveAgentsBut";
            this.saveAgentsBut.Size = new System.Drawing.Size( 117, 23 );
            this.saveAgentsBut.TabIndex = 7;
            this.saveAgentsBut.Text = "save agents";
            this.saveAgentsBut.UseVisualStyleBackColor = true;
            this.saveAgentsBut.Click += new System.EventHandler( this.saveAgentsBut_Click );
            // 
            // testPenetration
            // 
            this.testPenetration.Location = new System.Drawing.Point( 35, 71 );
            this.testPenetration.Name = "testPenetration";
            this.testPenetration.Size = new System.Drawing.Size( 117, 23 );
            this.testPenetration.TabIndex = 8;
            this.testPenetration.Text = "test penetration files";
            this.testPenetration.UseVisualStyleBackColor = true;
            this.testPenetration.Click += new System.EventHandler( this.testPenetration_Click );
            // 
            // loadAgentsButton
            // 
            this.loadAgentsButton.Location = new System.Drawing.Point( 35, 13 );
            this.loadAgentsButton.Name = "loadAgentsButton";
            this.loadAgentsButton.Size = new System.Drawing.Size( 117, 23 );
            this.loadAgentsButton.TabIndex = 9;
            this.loadAgentsButton.Text = "load agents";
            this.loadAgentsButton.UseVisualStyleBackColor = true;
            this.loadAgentsButton.Click += new System.EventHandler( this.loadAgentsButton_Click );
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.regUpdate );
            this.splitContainer1.Panel1.Controls.Add( this.loadAgentsButton );
            this.splitContainer1.Panel1.Controls.Add( this.updateAgents );
            this.splitContainer1.Panel1.Controls.Add( this.saveAgentsBut );
            this.splitContainer1.Panel1.Controls.Add( this.testPenetration );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.mediaTypeBox );
            this.splitContainer1.Size = new System.Drawing.Size( 180, 305 );
            this.splitContainer1.SplitterDistance = 172;
            this.splitContainer1.TabIndex = 10;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.splitContainer1 );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.agentInfo );
            this.splitContainer2.Size = new System.Drawing.Size( 494, 305 );
            this.splitContainer2.SplitterDistance = 180;
            this.splitContainer2.TabIndex = 11;
            // 
            // agentInfo
            // 
            this.agentInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.agentInfo.Location = new System.Drawing.Point( 0, 0 );
            this.agentInfo.Multiline = true;
            this.agentInfo.Name = "agentInfo";
            this.agentInfo.Size = new System.Drawing.Size( 310, 305 );
            this.agentInfo.TabIndex = 0;
            // 
            // regUpdate
            // 
            this.regUpdate.Location = new System.Drawing.Point( 35, 129 );
            this.regUpdate.Name = "regUpdate";
            this.regUpdate.Size = new System.Drawing.Size( 117, 23 );
            this.regUpdate.TabIndex = 10;
            this.regUpdate.Text = "Update Regions";
            this.regUpdate.UseVisualStyleBackColor = true;
            this.regUpdate.Click += new System.EventHandler( this.regUpdate_Click );
            // 
            // AgentUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainer2 );
            this.Name = "AgentUpdate";
            this.Size = new System.Drawing.Size( 494, 305 );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button updateAgents;
        private System.Windows.Forms.ListView mediaTypeBox;
        private System.Windows.Forms.Button saveAgentsBut;
        private System.Windows.Forms.Button testPenetration;
        private System.Windows.Forms.Button loadAgentsButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox agentInfo;
        private System.Windows.Forms.Button regUpdate;
    }
}
