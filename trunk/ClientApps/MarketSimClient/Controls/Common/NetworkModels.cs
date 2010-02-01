using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Common.Utilities;
using MrktSimDb;

using MarketSimUtilities;
namespace Common
{
	/// <summary>
	/// Summary description for SocialNetworkModels.
	/// </summary>
	public class NetworkModels : MrktSimControl
	{
		public override MrktSimDb.ModelDb Db
		{
			set
			{
				base.Db = value;

				mrktSimGrid.Table = theDb.Data.network_parameter;

				modelType.DataSource = ModelDb.network_type;
				modelType.DisplayMember = "name";
				modelType.ValueMember = "id";

				// for network parameters
				mrktSimGrid.Db = theDb;

				createTableStyle();
			}
		}

		public override bool Suspend
		{
			get
			{
				return base.Suspend;
			}
			set
			{
				base.Suspend = value;

				mrktSimGrid.Suspend = value;
			}
		}

		public override void Flush()
		{
			mrktSimGrid.Flush();
		}


		private System.Windows.Forms.GroupBox groupBox1;
		private MrktSimGrid mrktSimGrid;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown probOfTalkingPre;
		private System.Windows.Forms.Button createParameter;
		private System.Windows.Forms.CheckBox useLocal;
		private System.Windows.Forms.NumericUpDown persuasioPost;
		private System.Windows.Forms.NumericUpDown persuasioPre;
		private System.Windows.Forms.NumericUpDown probOfTalkingPost;
		private System.Windows.Forms.NumericUpDown awareness;
		private System.Windows.Forms.NumericUpDown numContacts;
		private System.Windows.Forms.ComboBox modelType;
		private System.Windows.Forms.TextBox paramName;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NetworkModels()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			createParameter.Enabled = false;


			// for network parameters
			mrktSimGrid.RowID = "id";
			mrktSimGrid.RowName = "name";
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.createParameter = new System.Windows.Forms.Button();
            this.useLocal = new System.Windows.Forms.CheckBox();
            this.awareness = new System.Windows.Forms.NumericUpDown();
            this.numContacts = new System.Windows.Forms.NumericUpDown();
            this.persuasioPost = new System.Windows.Forms.NumericUpDown();
            this.persuasioPre = new System.Windows.Forms.NumericUpDown();
            this.probOfTalkingPost = new System.Windows.Forms.NumericUpDown();
            this.probOfTalkingPre = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.modelType = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.paramName = new System.Windows.Forms.TextBox();
            this.mrktSimGrid = new MarketSimUtilities.MrktSimGrid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.awareness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numContacts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.persuasioPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.persuasioPre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probOfTalkingPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probOfTalkingPre)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.createParameter );
            this.groupBox1.Controls.Add( this.useLocal );
            this.groupBox1.Controls.Add( this.awareness );
            this.groupBox1.Controls.Add( this.numContacts );
            this.groupBox1.Controls.Add( this.persuasioPost );
            this.groupBox1.Controls.Add( this.persuasioPre );
            this.groupBox1.Controls.Add( this.probOfTalkingPost );
            this.groupBox1.Controls.Add( this.probOfTalkingPre );
            this.groupBox1.Controls.Add( this.label4 );
            this.groupBox1.Controls.Add( this.label2 );
            this.groupBox1.Controls.Add( this.modelType );
            this.groupBox1.Controls.Add( this.label10 );
            this.groupBox1.Controls.Add( this.label7 );
            this.groupBox1.Controls.Add( this.label6 );
            this.groupBox1.Controls.Add( this.label5 );
            this.groupBox1.Controls.Add( this.label3 );
            this.groupBox1.Controls.Add( this.label1 );
            this.groupBox1.Controls.Add( this.paramName );
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point( 0, 0 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 504, 160 );
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Network Models";
            // 
            // createParameter
            // 
            this.createParameter.Location = new System.Drawing.Point( 392, 128 );
            this.createParameter.Name = "createParameter";
            this.createParameter.Size = new System.Drawing.Size( 80, 23 );
            this.createParameter.TabIndex = 21;
            this.createParameter.Text = "Create Model";
            this.createParameter.Click += new System.EventHandler( this.createParameter_Click );
            // 
            // useLocal
            // 
            this.useLocal.Location = new System.Drawing.Point( 392, 88 );
            this.useLocal.Name = "useLocal";
            this.useLocal.Size = new System.Drawing.Size( 80, 24 );
            this.useLocal.TabIndex = 20;
            this.useLocal.Text = "Use Local";
            this.useLocal.Visible = false;
            // 
            // awareness
            // 
            this.awareness.DecimalPlaces = 3;
            this.awareness.Increment = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
            this.awareness.Location = new System.Drawing.Point( 392, 56 );
            this.awareness.Maximum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.awareness.Name = "awareness";
            this.awareness.Size = new System.Drawing.Size( 56, 20 );
            this.awareness.TabIndex = 19;
            // 
            // numContacts
            // 
            this.numContacts.Location = new System.Drawing.Point( 392, 24 );
            this.numContacts.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numContacts.Name = "numContacts";
            this.numContacts.Size = new System.Drawing.Size( 56, 20 );
            this.numContacts.TabIndex = 18;
            this.numContacts.Value = new decimal( new int[] {
            10,
            0,
            0,
            0} );
            // 
            // persuasioPost
            // 
            this.persuasioPost.DecimalPlaces = 3;
            this.persuasioPost.Increment = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
            this.persuasioPost.Location = new System.Drawing.Point( 264, 128 );
            this.persuasioPost.Name = "persuasioPost";
            this.persuasioPost.Size = new System.Drawing.Size( 64, 20 );
            this.persuasioPost.TabIndex = 17;
            // 
            // persuasioPre
            // 
            this.persuasioPre.DecimalPlaces = 3;
            this.persuasioPre.Increment = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
            this.persuasioPre.Location = new System.Drawing.Point( 264, 104 );
            this.persuasioPre.Name = "persuasioPre";
            this.persuasioPre.Size = new System.Drawing.Size( 64, 20 );
            this.persuasioPre.TabIndex = 16;
            // 
            // probOfTalkingPost
            // 
            this.probOfTalkingPost.DecimalPlaces = 3;
            this.probOfTalkingPost.Increment = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
            this.probOfTalkingPost.Location = new System.Drawing.Point( 104, 128 );
            this.probOfTalkingPost.Maximum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.probOfTalkingPost.Name = "probOfTalkingPost";
            this.probOfTalkingPost.Size = new System.Drawing.Size( 64, 20 );
            this.probOfTalkingPost.TabIndex = 15;
            // 
            // probOfTalkingPre
            // 
            this.probOfTalkingPre.DecimalPlaces = 3;
            this.probOfTalkingPre.Increment = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
            this.probOfTalkingPre.Location = new System.Drawing.Point( 104, 104 );
            this.probOfTalkingPre.Maximum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.probOfTalkingPre.Name = "probOfTalkingPre";
            this.probOfTalkingPre.Size = new System.Drawing.Size( 64, 20 );
            this.probOfTalkingPre.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point( 16, 128 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 48, 16 );
            this.label4.TabIndex = 13;
            this.label4.Text = "Post Use";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point( 16, 104 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 48, 16 );
            this.label2.TabIndex = 12;
            this.label2.Text = "Pre Use";
            // 
            // modelType
            // 
            this.modelType.Location = new System.Drawing.Point( 72, 56 );
            this.modelType.Name = "modelType";
            this.modelType.Size = new System.Drawing.Size( 136, 21 );
            this.modelType.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point( 16, 56 );
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size( 40, 16 );
            this.label10.TabIndex = 10;
            this.label10.Text = "Type";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point( 104, 88 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 120, 16 );
            this.label7.TabIndex = 7;
            this.label7.Text = "Probability of talking";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point( 264, 24 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 120, 16 );
            this.label6.TabIndex = 6;
            this.label6.Text = "Number of Contacts";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point( 264, 56 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 100, 23 );
            this.label5.TabIndex = 5;
            this.label5.Text = "Awareness Weight";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point( 264, 88 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 64, 16 );
            this.label3.TabIndex = 3;
            this.label3.Text = "Persuasion";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 16, 24 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 40, 23 );
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // paramName
            // 
            this.paramName.Location = new System.Drawing.Point( 72, 24 );
            this.paramName.Name = "paramName";
            this.paramName.Size = new System.Drawing.Size( 136, 20 );
            this.paramName.TabIndex = 0;
            this.paramName.TextChanged += new System.EventHandler( this.paramName_TextChanged );
            // 
            // mrktSimGrid
            // 
            this.mrktSimGrid.DescribeRow = null;
            this.mrktSimGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mrktSimGrid.EnabledGrid = true;
            this.mrktSimGrid.Location = new System.Drawing.Point( 0, 160 );
            this.mrktSimGrid.Name = "mrktSimGrid";
            this.mrktSimGrid.RowFilter = null;
            this.mrktSimGrid.RowID = null;
            this.mrktSimGrid.RowName = null;
            this.mrktSimGrid.Size = new System.Drawing.Size( 504, 136 );
            this.mrktSimGrid.Sort = "";
            this.mrktSimGrid.TabIndex = 1;
            this.mrktSimGrid.Table = null;
            // 
            // NetworkModels
            // 
            this.Controls.Add( this.mrktSimGrid );
            this.Controls.Add( this.groupBox1 );
            this.Name = "NetworkModels";
            this.Size = new System.Drawing.Size( 504, 296 );
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.awareness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numContacts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.persuasioPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.persuasioPre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probOfTalkingPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probOfTalkingPre)).EndInit();
            this.ResumeLayout( false );

		}
		#endregion

		private void createTableStyle()
		{
			mrktSimGrid.Clear();

			mrktSimGrid.AddTextColumn("name");

			mrktSimGrid.AddComboBoxColumn("type", ModelDb.network_type, "name", "id");
			
			mrktSimGrid.AddNumericColumn("persuasion_pre_use");
			mrktSimGrid.AddNumericColumn("persuasion_post_use");

			mrktSimGrid.AddNumericColumn("awareness_weight");

			mrktSimGrid.AddNumericColumn("num_contacts");

			mrktSimGrid.AddNumericColumn("prob_of_talking_pre_use");
			mrktSimGrid.AddNumericColumn("prob_of_talking_post_use");

			// mrktSimGrid.AddCheckBoxColumn("use_local");

			mrktSimGrid.Reset();
		}

		private void createParameter_Click(object sender, System.EventArgs e)
		{
			if (paramName.Text == "")
				return;

			MrktSimDBSchema.network_parameterRow param = theDb.CreateNetworkParameter(paramName.Text);

			param.num_contacts = (double) numContacts.Value;
			param.prob_of_talking_post_use = (double) probOfTalkingPost.Value;
			param.prob_of_talking_pre_use = (double) probOfTalkingPre.Value;
			param.persuasion_post_use = (double) persuasioPost.Value;
			param.persuasion_pre_use = (double) persuasioPre.Value;
			param.awareness_weight = (double) awareness.Value;
			param.use_local = useLocal.Checked;
			param.type = (int) modelType.SelectedValue;

			paramName.Text = "";
			createParameter.Enabled = false;
		}

		private void paramName_TextChanged(object sender, System.EventArgs e)
		{
			if (paramName.Text == "")
			{
				createParameter.Enabled = false;
			}
			else
			{
				createParameter.Enabled = true;
			}
		}
	}
}
