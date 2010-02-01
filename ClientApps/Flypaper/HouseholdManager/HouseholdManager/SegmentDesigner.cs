using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HouseholdLibrary;
using DemographicLibrary;

namespace HouseholdManager
{
    public partial class SegmentDesigner : Form
    {
        public Demographic Demographic { get; set; }

        public SegmentDesigner()
        {
            InitializeComponent();

            Demographic = new Demographic();

            set_up();
        }

        public SegmentDesigner(Demographic demographic)
        {
            InitializeComponent();

            Demographic = demographic;

            set_up();

            RaceCombo.SelectedItem = Demographic.Race.ToString();
            GenderCombo.SelectedItem = Demographic.Gender.ToString();
            KidsCombo.SelectedItem = Demographic.Kids.ToString();
            HomeownerCombo.SelectedItem = Demographic.Homeowner.ToString();

            MinAge.Value = Demographic.AgeMin;
            MaxAge.Value = Demographic.AgeMax;
            MinIncome.Value = Demographic.IncomeMin;
            MaxIncome.Value = Demographic.IncomeMax;

        }

        private void set_up()
        {
            RaceCombo.Items.Add("ANY");
            RaceCombo.Items.AddRange((new Race()).GetNames());
            GenderCombo.Items.Add("ANY");
            GenderCombo.Items.AddRange((new Gender()).GetNames());
            KidsCombo.Items.Add("ANY");
            KidsCombo.Items.AddRange((new ChildStatus()).GetNames());
            HomeownerCombo.Items.Add("ANY");
            HomeownerCombo.Items.AddRange((new HomeOwner()).GetNames());


            RaceCombo.SelectedIndex = 0;
            GenderCombo.SelectedIndex = 0;
            KidsCombo.SelectedIndex = 0;
            HomeownerCombo.SelectedIndex = 0;

            MaxIncome.Value = 1000000;
            MaxAge.Value = 100;
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            if (RaceCombo.SelectedItem.ToString() != "ANY")
            {
                Demographic.Race = RaceCombo.SelectedItem.ToString();
            }
            if (GenderCombo.SelectedItem.ToString() != "ANY")
            {
                Demographic.Gender = GenderCombo.SelectedItem.ToString();
            }
            if (KidsCombo.SelectedItem.ToString() != "ANY")
            {
                Demographic.Kids = KidsCombo.SelectedItem.ToString();
            }
            if (HomeownerCombo.SelectedItem.ToString() != "ANY")
            {
                Demographic.Homeowner = HomeownerCombo.SelectedItem.ToString();
            }

            Demographic.Income.Clear();
            Demographic.AddIncome( (int)MinIncome.Value, (int)MaxIncome.Value);
            Demographic.Age.Clear();
            Demographic.AddAge( (int)MinAge.Value, (int)MaxAge.Value );

            Demographic.Name = NameTextBox.Text;

            DialogResult = DialogResult.OK;
        }

        private void SegmentDesigner_Shown(object sender, EventArgs e)
        {
            if (Demographic.Name.Length < 1)
            {
                return;
            }

            if( Demographic.Race < DemographicType<Race>.ANY )
            {
                RaceCombo.SelectedItem = Demographic.Race;
            }
            if( Demographic.Gender < DemographicType<Gender>.ANY )
            {
                GenderCombo.SelectedItem = Demographic.Gender;
            }
            if( Demographic.Kids < DemographicType<ChildStatus>.ANY )
            {
                KidsCombo.SelectedItem = Demographic.Kids;
            }
            if( Demographic.Homeowner < DemographicType<HomeOwner>.ANY )
            {
                HomeownerCombo.SelectedItem = Demographic.Homeowner;
            }

            MinIncome.Value = Demographic.IncomeMin;
            MaxIncome.Value = Demographic.IncomeMax;
            MinAge.Value = Demographic.AgeMin;
            MaxAge.Value = Demographic.AgeMax;
        }


    }
}
