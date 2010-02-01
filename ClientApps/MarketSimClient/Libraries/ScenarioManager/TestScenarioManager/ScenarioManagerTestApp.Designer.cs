namespace TestScenarioManager
{
    partial class ScenarioManagerTestApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.testConnectionButton = new System.Windows.Forms.Button();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.projectsButton = new System.Windows.Forms.Button();
            this.modelsButton = new System.Windows.Forms.Button();
            this.testGroup = new System.Windows.Forms.GroupBox();
            this.nextModelButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.projectLabel = new System.Windows.Forms.Label();
            this.nextProjectButton = new System.Windows.Forms.Button();
            this.modelGroupBox = new System.Windows.Forms.GroupBox();
            this.AddDataButton = new System.Windows.Forms.Button();
            this.compPropsButton = new System.Windows.Forms.Button();
            this.planPropsButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.planInOutButton = new System.Windows.Forms.Button();
            this.compInOutButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.copyPlanDeepButton = new System.Windows.Forms.Button();
            this.copyScenarioDeepButton = new System.Windows.Forms.Button();
            this.segmentsButton = new System.Windows.Forms.Button();
            this.productsButton = new System.Windows.Forms.Button();
            this.channelsButton = new System.Windows.Forms.Button();
            this.deleteCompButton = new System.Windows.Forms.Button();
            this.renameCompButton = new System.Windows.Forms.Button();
            this.deletePlanButton = new System.Windows.Forms.Button();
            this.renamePlanButton = new System.Windows.Forms.Button();
            this.deleteScenarioButton = new System.Windows.Forms.Button();
            this.renameScenarioButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.copyPlanButton = new System.Windows.Forms.Button();
            this.copyScenarioButton = new System.Windows.Forms.Button();
            this.copyCompButton = new System.Windows.Forms.Button();
            this.dataButton = new System.Windows.Forms.Button();
            this.bextDataButton = new System.Windows.Forms.Button();
            this.dataLabel = new System.Windows.Forms.Label();
            this.nextCompButton = new System.Windows.Forms.Button();
            this.nextPlanButton = new System.Windows.Forms.Button();
            this.componentsLabel = new System.Windows.Forms.Label();
            this.plansLabel = new System.Windows.Forms.Label();
            this.componentsButton = new System.Windows.Forms.Button();
            this.plansButton = new System.Windows.Forms.Button();
            this.scenariosButton = new System.Windows.Forms.Button();
            this.testModelLabel = new System.Windows.Forms.Label();
            this.openModelButton = new System.Windows.Forms.Button();
            this.priceTypesBut = new System.Windows.Forms.Button();
            this.testGroup.SuspendLayout();
            this.modelGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // testConnectionButton
            // 
            this.testConnectionButton.Location = new System.Drawing.Point( 29, 11 );
            this.testConnectionButton.Name = "testConnectionButton";
            this.testConnectionButton.Size = new System.Drawing.Size( 98, 23 );
            this.testConnectionButton.TabIndex = 0;
            this.testConnectionButton.Text = "Connect";
            this.testConnectionButton.UseVisualStyleBackColor = true;
            this.testConnectionButton.Click += new System.EventHandler( this.testConnectionButton_Click );
            // 
            // databaseLabel
            // 
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.databaseLabel.Location = new System.Drawing.Point( 133, 16 );
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size( 146, 13 );
            this.databaseLabel.TabIndex = 1;
            this.databaseLabel.Text = "Database: Not conected";
            // 
            // projectsButton
            // 
            this.projectsButton.Location = new System.Drawing.Point( 18, 16 );
            this.projectsButton.Name = "projectsButton";
            this.projectsButton.Size = new System.Drawing.Size( 121, 23 );
            this.projectsButton.TabIndex = 2;
            this.projectsButton.Text = "Show Projects";
            this.projectsButton.UseVisualStyleBackColor = true;
            this.projectsButton.Click += new System.EventHandler( this.projectsButton_Click );
            // 
            // modelsButton
            // 
            this.modelsButton.Location = new System.Drawing.Point( 18, 47 );
            this.modelsButton.Name = "modelsButton";
            this.modelsButton.Size = new System.Drawing.Size( 121, 23 );
            this.modelsButton.TabIndex = 3;
            this.modelsButton.Text = "Show Models";
            this.modelsButton.UseVisualStyleBackColor = true;
            this.modelsButton.Click += new System.EventHandler( this.modelsButton_Click );
            // 
            // testGroup
            // 
            this.testGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.testGroup.Controls.Add( this.nextModelButton );
            this.testGroup.Controls.Add( this.label5 );
            this.testGroup.Controls.Add( this.label1 );
            this.testGroup.Controls.Add( this.projectLabel );
            this.testGroup.Controls.Add( this.nextProjectButton );
            this.testGroup.Controls.Add( this.modelGroupBox );
            this.testGroup.Controls.Add( this.testModelLabel );
            this.testGroup.Controls.Add( this.openModelButton );
            this.testGroup.Controls.Add( this.projectsButton );
            this.testGroup.Controls.Add( this.modelsButton );
            this.testGroup.Enabled = false;
            this.testGroup.Location = new System.Drawing.Point( 12, 40 );
            this.testGroup.Name = "testGroup";
            this.testGroup.Size = new System.Drawing.Size( 763, 425 );
            this.testGroup.TabIndex = 4;
            this.testGroup.TabStop = false;
            this.testGroup.Text = "Tests";
            // 
            // nextModelButton
            // 
            this.nextModelButton.Location = new System.Drawing.Point( 226, 78 );
            this.nextModelButton.Name = "nextModelButton";
            this.nextModelButton.Size = new System.Drawing.Size( 24, 23 );
            this.nextModelButton.TabIndex = 12;
            this.nextModelButton.Text = "+";
            this.nextModelButton.UseVisualStyleBackColor = true;
            this.nextModelButton.Click += new System.EventHandler( this.nextModelButton_Click );
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 148, 84 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 72, 13 );
            this.label5.TabIndex = 11;
            this.label5.Text = "Active Model:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 148, 52 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 76, 13 );
            this.label1.TabIndex = 10;
            this.label1.Text = "Active Project:";
            // 
            // projectLabel
            // 
            this.projectLabel.AutoSize = true;
            this.projectLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.projectLabel.Location = new System.Drawing.Point( 257, 52 );
            this.projectLabel.Name = "projectLabel";
            this.projectLabel.Size = new System.Drawing.Size( 0, 13 );
            this.projectLabel.TabIndex = 9;
            // 
            // nextProjectButton
            // 
            this.nextProjectButton.Location = new System.Drawing.Point( 226, 47 );
            this.nextProjectButton.Name = "nextProjectButton";
            this.nextProjectButton.Size = new System.Drawing.Size( 24, 23 );
            this.nextProjectButton.TabIndex = 8;
            this.nextProjectButton.Text = "+";
            this.nextProjectButton.UseVisualStyleBackColor = true;
            this.nextProjectButton.Click += new System.EventHandler( this.nextProjectButton_Click );
            // 
            // modelGroupBox
            // 
            this.modelGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.modelGroupBox.Controls.Add( this.priceTypesBut );
            this.modelGroupBox.Controls.Add( this.AddDataButton );
            this.modelGroupBox.Controls.Add( this.compPropsButton );
            this.modelGroupBox.Controls.Add( this.planPropsButton );
            this.modelGroupBox.Controls.Add( this.button1 );
            this.modelGroupBox.Controls.Add( this.planInOutButton );
            this.modelGroupBox.Controls.Add( this.compInOutButton );
            this.modelGroupBox.Controls.Add( this.label4 );
            this.modelGroupBox.Controls.Add( this.label3 );
            this.modelGroupBox.Controls.Add( this.label2 );
            this.modelGroupBox.Controls.Add( this.copyPlanDeepButton );
            this.modelGroupBox.Controls.Add( this.copyScenarioDeepButton );
            this.modelGroupBox.Controls.Add( this.segmentsButton );
            this.modelGroupBox.Controls.Add( this.productsButton );
            this.modelGroupBox.Controls.Add( this.channelsButton );
            this.modelGroupBox.Controls.Add( this.deleteCompButton );
            this.modelGroupBox.Controls.Add( this.renameCompButton );
            this.modelGroupBox.Controls.Add( this.deletePlanButton );
            this.modelGroupBox.Controls.Add( this.renamePlanButton );
            this.modelGroupBox.Controls.Add( this.deleteScenarioButton );
            this.modelGroupBox.Controls.Add( this.renameScenarioButton );
            this.modelGroupBox.Controls.Add( this.saveButton );
            this.modelGroupBox.Controls.Add( this.copyPlanButton );
            this.modelGroupBox.Controls.Add( this.copyScenarioButton );
            this.modelGroupBox.Controls.Add( this.copyCompButton );
            this.modelGroupBox.Controls.Add( this.dataButton );
            this.modelGroupBox.Controls.Add( this.bextDataButton );
            this.modelGroupBox.Controls.Add( this.dataLabel );
            this.modelGroupBox.Controls.Add( this.nextCompButton );
            this.modelGroupBox.Controls.Add( this.nextPlanButton );
            this.modelGroupBox.Controls.Add( this.componentsLabel );
            this.modelGroupBox.Controls.Add( this.plansLabel );
            this.modelGroupBox.Controls.Add( this.componentsButton );
            this.modelGroupBox.Controls.Add( this.plansButton );
            this.modelGroupBox.Controls.Add( this.scenariosButton );
            this.modelGroupBox.Enabled = false;
            this.modelGroupBox.Location = new System.Drawing.Point( 14, 107 );
            this.modelGroupBox.Name = "modelGroupBox";
            this.modelGroupBox.Size = new System.Drawing.Size( 734, 305 );
            this.modelGroupBox.TabIndex = 7;
            this.modelGroupBox.TabStop = false;
            this.modelGroupBox.Text = "Model";
            // 
            // AddDataButton
            // 
            this.AddDataButton.Location = new System.Drawing.Point( 8, 265 );
            this.AddDataButton.Name = "AddDataButton";
            this.AddDataButton.Size = new System.Drawing.Size( 114, 23 );
            this.AddDataButton.TabIndex = 35;
            this.AddDataButton.Text = "Add Data";
            this.AddDataButton.UseVisualStyleBackColor = true;
            this.AddDataButton.Click += new System.EventHandler( this.AddDataButton_Click );
            // 
            // compPropsButton
            // 
            this.compPropsButton.Location = new System.Drawing.Point( 606, 212 );
            this.compPropsButton.Name = "compPropsButton";
            this.compPropsButton.Size = new System.Drawing.Size( 114, 23 );
            this.compPropsButton.TabIndex = 34;
            this.compPropsButton.Text = "Properties";
            this.compPropsButton.UseVisualStyleBackColor = true;
            this.compPropsButton.Click += new System.EventHandler( this.compPropsButton_Click );
            // 
            // planPropsButton
            // 
            this.planPropsButton.Location = new System.Drawing.Point( 606, 133 );
            this.planPropsButton.Name = "planPropsButton";
            this.planPropsButton.Size = new System.Drawing.Size( 114, 23 );
            this.planPropsButton.TabIndex = 33;
            this.planPropsButton.Text = "Properties";
            this.planPropsButton.UseVisualStyleBackColor = true;
            this.planPropsButton.Click += new System.EventHandler( this.planPropsButton_Click );
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point( 255, 110 );
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size( 131, 23 );
            this.button1.TabIndex = 32;
            this.button1.Text = "Copy (deep)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler( this.button1_Click_1 );
            // 
            // planInOutButton
            // 
            this.planInOutButton.Location = new System.Drawing.Point( 606, 162 );
            this.planInOutButton.Name = "planInOutButton";
            this.planInOutButton.Size = new System.Drawing.Size( 114, 23 );
            this.planInOutButton.TabIndex = 31;
            this.planInOutButton.Text = "In/Out";
            this.planInOutButton.UseVisualStyleBackColor = true;
            this.planInOutButton.Click += new System.EventHandler( this.planInOutButton_Click );
            // 
            // compInOutButton
            // 
            this.compInOutButton.Location = new System.Drawing.Point( 489, 236 );
            this.compInOutButton.Name = "compInOutButton";
            this.compInOutButton.Size = new System.Drawing.Size( 114, 23 );
            this.compInOutButton.TabIndex = 30;
            this.compInOutButton.Text = "In/Out";
            this.compInOutButton.UseVisualStyleBackColor = true;
            this.compInOutButton.Click += new System.EventHandler( this.button1_Click );
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 18, 212 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 97, 13 );
            this.label4.TabIndex = 29;
            this.label4.Text = "Active Component:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 13, 137 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 100, 13 );
            this.label3.TabIndex = 28;
            this.label3.Text = "Active Market Plan:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 13, 61 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 85, 13 );
            this.label2.TabIndex = 27;
            this.label2.Text = "Active Scenario:";
            // 
            // copyPlanDeepButton
            // 
            this.copyPlanDeepButton.Location = new System.Drawing.Point( 255, 162 );
            this.copyPlanDeepButton.Name = "copyPlanDeepButton";
            this.copyPlanDeepButton.Size = new System.Drawing.Size( 114, 23 );
            this.copyPlanDeepButton.TabIndex = 26;
            this.copyPlanDeepButton.Text = "Copy (deep)";
            this.copyPlanDeepButton.UseVisualStyleBackColor = true;
            this.copyPlanDeepButton.Click += new System.EventHandler( this.copyPlanDeepButton_Click );
            // 
            // copyScenarioDeepButton
            // 
            this.copyScenarioDeepButton.Location = new System.Drawing.Point( 255, 86 );
            this.copyScenarioDeepButton.Name = "copyScenarioDeepButton";
            this.copyScenarioDeepButton.Size = new System.Drawing.Size( 131, 23 );
            this.copyScenarioDeepButton.TabIndex = 25;
            this.copyScenarioDeepButton.Text = "Copy (deep) NO comps";
            this.copyScenarioDeepButton.UseVisualStyleBackColor = true;
            this.copyScenarioDeepButton.Click += new System.EventHandler( this.copyScenarioDeepButton_Click );
            // 
            // segmentsButton
            // 
            this.segmentsButton.Location = new System.Drawing.Point( 521, 19 );
            this.segmentsButton.Name = "segmentsButton";
            this.segmentsButton.Size = new System.Drawing.Size( 114, 23 );
            this.segmentsButton.TabIndex = 23;
            this.segmentsButton.Text = "Segments";
            this.segmentsButton.UseVisualStyleBackColor = true;
            this.segmentsButton.Click += new System.EventHandler( this.segmentsButton_Click );
            // 
            // productsButton
            // 
            this.productsButton.Location = new System.Drawing.Point( 281, 19 );
            this.productsButton.Name = "productsButton";
            this.productsButton.Size = new System.Drawing.Size( 114, 23 );
            this.productsButton.TabIndex = 22;
            this.productsButton.Text = "Products";
            this.productsButton.UseVisualStyleBackColor = true;
            this.productsButton.Click += new System.EventHandler( this.productsButton_Click );
            // 
            // channelsButton
            // 
            this.channelsButton.Location = new System.Drawing.Point( 401, 19 );
            this.channelsButton.Name = "channelsButton";
            this.channelsButton.Size = new System.Drawing.Size( 114, 23 );
            this.channelsButton.TabIndex = 21;
            this.channelsButton.Text = "Channels";
            this.channelsButton.UseVisualStyleBackColor = true;
            this.channelsButton.Click += new System.EventHandler( this.channelsButton_Click );
            // 
            // deleteCompButton
            // 
            this.deleteCompButton.Location = new System.Drawing.Point( 372, 236 );
            this.deleteCompButton.Name = "deleteCompButton";
            this.deleteCompButton.Size = new System.Drawing.Size( 114, 23 );
            this.deleteCompButton.TabIndex = 20;
            this.deleteCompButton.Text = "Delete";
            this.deleteCompButton.UseVisualStyleBackColor = true;
            this.deleteCompButton.Click += new System.EventHandler( this.deleteCompButton_Click );
            // 
            // renameCompButton
            // 
            this.renameCompButton.Location = new System.Drawing.Point( 255, 236 );
            this.renameCompButton.Name = "renameCompButton";
            this.renameCompButton.Size = new System.Drawing.Size( 114, 23 );
            this.renameCompButton.TabIndex = 19;
            this.renameCompButton.Text = "Rename";
            this.renameCompButton.UseVisualStyleBackColor = true;
            this.renameCompButton.Click += new System.EventHandler( this.renameCompButton_Click );
            // 
            // deletePlanButton
            // 
            this.deletePlanButton.Location = new System.Drawing.Point( 489, 162 );
            this.deletePlanButton.Name = "deletePlanButton";
            this.deletePlanButton.Size = new System.Drawing.Size( 114, 23 );
            this.deletePlanButton.TabIndex = 18;
            this.deletePlanButton.Text = "Delete";
            this.deletePlanButton.UseVisualStyleBackColor = true;
            this.deletePlanButton.Click += new System.EventHandler( this.deletePlanButton_Click );
            // 
            // renamePlanButton
            // 
            this.renamePlanButton.Location = new System.Drawing.Point( 372, 162 );
            this.renamePlanButton.Name = "renamePlanButton";
            this.renamePlanButton.Size = new System.Drawing.Size( 114, 23 );
            this.renamePlanButton.TabIndex = 17;
            this.renamePlanButton.Text = "Rename";
            this.renamePlanButton.UseVisualStyleBackColor = true;
            this.renamePlanButton.Click += new System.EventHandler( this.renamePlanButton_Click );
            // 
            // deleteScenarioButton
            // 
            this.deleteScenarioButton.Location = new System.Drawing.Point( 512, 86 );
            this.deleteScenarioButton.Name = "deleteScenarioButton";
            this.deleteScenarioButton.Size = new System.Drawing.Size( 114, 23 );
            this.deleteScenarioButton.TabIndex = 16;
            this.deleteScenarioButton.Text = "Delete";
            this.deleteScenarioButton.UseVisualStyleBackColor = true;
            this.deleteScenarioButton.Click += new System.EventHandler( this.deleteScenarioButton_Click );
            // 
            // renameScenarioButton
            // 
            this.renameScenarioButton.Location = new System.Drawing.Point( 392, 86 );
            this.renameScenarioButton.Name = "renameScenarioButton";
            this.renameScenarioButton.Size = new System.Drawing.Size( 114, 23 );
            this.renameScenarioButton.TabIndex = 15;
            this.renameScenarioButton.Text = "Rename";
            this.renameScenarioButton.UseVisualStyleBackColor = true;
            this.renameScenarioButton.Click += new System.EventHandler( this.renameScenarioButton_Click );
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point( 246, 276 );
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size( 250, 23 );
            this.saveButton.TabIndex = 14;
            this.saveButton.Text = "Save Changes";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler( this.saveButton_Click );
            // 
            // copyPlanButton
            // 
            this.copyPlanButton.Location = new System.Drawing.Point( 138, 162 );
            this.copyPlanButton.Name = "copyPlanButton";
            this.copyPlanButton.Size = new System.Drawing.Size( 114, 23 );
            this.copyPlanButton.TabIndex = 13;
            this.copyPlanButton.Text = "Copy (shallow)";
            this.copyPlanButton.UseVisualStyleBackColor = true;
            this.copyPlanButton.Click += new System.EventHandler( this.copyPlanButton_Click );
            // 
            // copyScenarioButton
            // 
            this.copyScenarioButton.Location = new System.Drawing.Point( 138, 86 );
            this.copyScenarioButton.Name = "copyScenarioButton";
            this.copyScenarioButton.Size = new System.Drawing.Size( 114, 23 );
            this.copyScenarioButton.TabIndex = 12;
            this.copyScenarioButton.Text = "Copy (shallow)";
            this.copyScenarioButton.UseVisualStyleBackColor = true;
            this.copyScenarioButton.Click += new System.EventHandler( this.copyScenarioButton_Click );
            // 
            // copyCompButton
            // 
            this.copyCompButton.Location = new System.Drawing.Point( 138, 236 );
            this.copyCompButton.Name = "copyCompButton";
            this.copyCompButton.Size = new System.Drawing.Size( 114, 23 );
            this.copyCompButton.TabIndex = 11;
            this.copyCompButton.Text = "Copy";
            this.copyCompButton.UseVisualStyleBackColor = true;
            this.copyCompButton.Click += new System.EventHandler( this.copyCompButton_Click );
            // 
            // dataButton
            // 
            this.dataButton.Location = new System.Drawing.Point( 8, 236 );
            this.dataButton.Name = "dataButton";
            this.dataButton.Size = new System.Drawing.Size( 114, 23 );
            this.dataButton.TabIndex = 10;
            this.dataButton.Text = "Data";
            this.dataButton.UseVisualStyleBackColor = true;
            this.dataButton.Click += new System.EventHandler( this.dataButton_Click );
            // 
            // bextDataButton
            // 
            this.bextDataButton.Location = new System.Drawing.Point( 119, 207 );
            this.bextDataButton.Name = "bextDataButton";
            this.bextDataButton.Size = new System.Drawing.Size( 24, 23 );
            this.bextDataButton.TabIndex = 9;
            this.bextDataButton.Text = "+";
            this.bextDataButton.UseVisualStyleBackColor = true;
            this.bextDataButton.Click += new System.EventHandler( this.bextDataButton_Click );
            // 
            // dataLabel
            // 
            this.dataLabel.AutoSize = true;
            this.dataLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.dataLabel.Location = new System.Drawing.Point( 150, 213 );
            this.dataLabel.Name = "dataLabel";
            this.dataLabel.Size = new System.Drawing.Size( 11, 13 );
            this.dataLabel.TabIndex = 8;
            this.dataLabel.Text = "-";
            // 
            // nextCompButton
            // 
            this.nextCompButton.Location = new System.Drawing.Point( 117, 132 );
            this.nextCompButton.Name = "nextCompButton";
            this.nextCompButton.Size = new System.Drawing.Size( 24, 23 );
            this.nextCompButton.TabIndex = 7;
            this.nextCompButton.Text = "+";
            this.nextCompButton.UseVisualStyleBackColor = true;
            this.nextCompButton.Click += new System.EventHandler( this.nextCompButton_Click );
            // 
            // nextPlanButton
            // 
            this.nextPlanButton.Location = new System.Drawing.Point( 101, 56 );
            this.nextPlanButton.Name = "nextPlanButton";
            this.nextPlanButton.Size = new System.Drawing.Size( 24, 23 );
            this.nextPlanButton.TabIndex = 6;
            this.nextPlanButton.Text = "+";
            this.nextPlanButton.UseVisualStyleBackColor = true;
            this.nextPlanButton.Click += new System.EventHandler( this.nextPlanButton_Click );
            // 
            // componentsLabel
            // 
            this.componentsLabel.AutoSize = true;
            this.componentsLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.componentsLabel.Location = new System.Drawing.Point( 148, 137 );
            this.componentsLabel.Name = "componentsLabel";
            this.componentsLabel.Size = new System.Drawing.Size( 11, 13 );
            this.componentsLabel.TabIndex = 5;
            this.componentsLabel.Text = "-";
            // 
            // plansLabel
            // 
            this.plansLabel.AutoSize = true;
            this.plansLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.plansLabel.Location = new System.Drawing.Point( 131, 61 );
            this.plansLabel.Name = "plansLabel";
            this.plansLabel.Size = new System.Drawing.Size( 11, 13 );
            this.plansLabel.TabIndex = 4;
            this.plansLabel.Text = "-";
            // 
            // componentsButton
            // 
            this.componentsButton.Location = new System.Drawing.Point( 8, 162 );
            this.componentsButton.Name = "componentsButton";
            this.componentsButton.Size = new System.Drawing.Size( 114, 23 );
            this.componentsButton.TabIndex = 2;
            this.componentsButton.Text = "Components";
            this.componentsButton.UseVisualStyleBackColor = true;
            this.componentsButton.Click += new System.EventHandler( this.componentsButton_Click );
            // 
            // plansButton
            // 
            this.plansButton.Location = new System.Drawing.Point( 8, 86 );
            this.plansButton.Name = "plansButton";
            this.plansButton.Size = new System.Drawing.Size( 114, 23 );
            this.plansButton.TabIndex = 1;
            this.plansButton.Text = "Market Plans";
            this.plansButton.UseVisualStyleBackColor = true;
            this.plansButton.Click += new System.EventHandler( this.plansButton_Click );
            // 
            // scenariosButton
            // 
            this.scenariosButton.Location = new System.Drawing.Point( 8, 17 );
            this.scenariosButton.Name = "scenariosButton";
            this.scenariosButton.Size = new System.Drawing.Size( 114, 23 );
            this.scenariosButton.TabIndex = 0;
            this.scenariosButton.Text = "Scenarios";
            this.scenariosButton.UseVisualStyleBackColor = true;
            this.scenariosButton.Click += new System.EventHandler( this.scenariosButton_Click );
            // 
            // testModelLabel
            // 
            this.testModelLabel.AutoSize = true;
            this.testModelLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.testModelLabel.Location = new System.Drawing.Point( 257, 83 );
            this.testModelLabel.Name = "testModelLabel";
            this.testModelLabel.Size = new System.Drawing.Size( 0, 13 );
            this.testModelLabel.TabIndex = 6;
            // 
            // openModelButton
            // 
            this.openModelButton.Location = new System.Drawing.Point( 18, 79 );
            this.openModelButton.Name = "openModelButton";
            this.openModelButton.Size = new System.Drawing.Size( 121, 23 );
            this.openModelButton.TabIndex = 5;
            this.openModelButton.Text = "Open Model";
            this.openModelButton.UseVisualStyleBackColor = true;
            this.openModelButton.Click += new System.EventHandler( this.openModelButton_Click );
            // 
            // priceTypesBut
            // 
            this.priceTypesBut.Location = new System.Drawing.Point( 645, 20 );
            this.priceTypesBut.Name = "priceTypesBut";
            this.priceTypesBut.Size = new System.Drawing.Size( 75, 23 );
            this.priceTypesBut.TabIndex = 36;
            this.priceTypesBut.Text = "Price Types";
            this.priceTypesBut.UseVisualStyleBackColor = true;
            this.priceTypesBut.Click += new System.EventHandler( this.priceTypesBut_Click );
            // 
            // ScenarioManagerTestApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 787, 486 );
            this.Controls.Add( this.testGroup );
            this.Controls.Add( this.databaseLabel );
            this.Controls.Add( this.testConnectionButton );
            this.Name = "ScenarioManagerTestApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scenario Managar Library Test Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.ScenarioManagerTestApp_FormClosing );
            this.testGroup.ResumeLayout( false );
            this.testGroup.PerformLayout();
            this.modelGroupBox.ResumeLayout( false );
            this.modelGroupBox.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button testConnectionButton;
        private System.Windows.Forms.Label databaseLabel;
        private System.Windows.Forms.Button projectsButton;
        private System.Windows.Forms.Button modelsButton;
        private System.Windows.Forms.GroupBox testGroup;
        private System.Windows.Forms.Button openModelButton;
        private System.Windows.Forms.Label testModelLabel;
        private System.Windows.Forms.GroupBox modelGroupBox;
        private System.Windows.Forms.Label componentsLabel;
        private System.Windows.Forms.Label plansLabel;
        private System.Windows.Forms.Button componentsButton;
        private System.Windows.Forms.Button plansButton;
        private System.Windows.Forms.Button scenariosButton;
        private System.Windows.Forms.Button nextCompButton;
        private System.Windows.Forms.Button nextPlanButton;
        private System.Windows.Forms.Button nextProjectButton;
        private System.Windows.Forms.Button dataButton;
        private System.Windows.Forms.Button bextDataButton;
        private System.Windows.Forms.Label dataLabel;
        private System.Windows.Forms.Label projectLabel;
        private System.Windows.Forms.Button copyCompButton;
        private System.Windows.Forms.Button copyPlanButton;
        private System.Windows.Forms.Button copyScenarioButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button renameScenarioButton;
        private System.Windows.Forms.Button deleteCompButton;
        private System.Windows.Forms.Button renameCompButton;
        private System.Windows.Forms.Button deletePlanButton;
        private System.Windows.Forms.Button renamePlanButton;
        private System.Windows.Forms.Button deleteScenarioButton;
        private System.Windows.Forms.Button segmentsButton;
        private System.Windows.Forms.Button productsButton;
        private System.Windows.Forms.Button channelsButton;
        private System.Windows.Forms.Button copyPlanDeepButton;
        private System.Windows.Forms.Button copyScenarioDeepButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button nextModelButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button compInOutButton;
        private System.Windows.Forms.Button planInOutButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button compPropsButton;
        private System.Windows.Forms.Button planPropsButton;
        private System.Windows.Forms.Button AddDataButton;
        private System.Windows.Forms.Button priceTypesBut;
    }
}

