using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DecisionPower.MarketSim.ScenarioManagerLibrary.Components;
using DecisionPower.MarketSim.ScenarioManagerLibrary.Data;

namespace TestScenarioManager
{
    public partial class DataForm : Form
    {
        private Component comp;
        private ComponentData[] data;

        int dataIndex = 0;

        /// <summary>
        /// Initialuzes the form.
        /// </summary>
        /// <param name="comp"></param>
        public DataForm( Component comp ) {
            InitializeComponent();

            this.comp = comp;

            int count = InitiazeData();

            if( count > 0 ) {
                DisplayDataItem();
            }
            else {
                DisplayNoData();
            }
        }

        /// <summary>
        /// Displays the previous data item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prevButton_Click( object sender, EventArgs e ) {
            if( dataIndex > 0 ) {
                dataIndex -= 1;
            }
            else {
                dataIndex = data.Length - 1;
            }
            DisplayDataItem();
        }

        /// <summary>
        /// Displays the next data item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextButton_Click( object sender, EventArgs e ) {
            if( dataIndex < data.Length - 1 ) {
                dataIndex += 1;
            }
            else {
                dataIndex = 0;
            }
            DisplayDataItem();
        }

        /// <summary>
        /// Saves the UI values into the curent data item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click( object sender, EventArgs e ) {

            ComponentData dataItem = data[ dataIndex ];

            try {
                if( comp is CouponsComponent ) {
                }
                else if( comp is DisplayComponent ) {
                }
                else if( comp is DistributionComponent ) {
                    ((DistributionComponentData)dataItem).Awareness = double.Parse( awarenessTextBox.Text.Trim() );
                    ((DistributionComponentData)dataItem).Persuasion = double.Parse( persuasionTextBox.Text.Trim() );
                    ((DistributionComponentData)dataItem).PercentDistribution = double.Parse( distTextBox.Text.Trim() );
                }
                else if( comp is PriceComponent ) {
                }
                else if( comp is MediaComponent ) {
                }
                else if( comp is MarketUtilityComponent ) {
                }
            }
            catch( Exception ) {
                MessageBox.Show( "Error parsing numeric values!", "Error" );
            }
        }

        /// <summary>
        /// Initilaizes the data array.
        /// </summary>
        /// <returns></returns>
        private int InitiazeData() {
            if( comp is CouponsComponent ) {
                data = ((CouponsComponent)comp).GetData();
            }
            else if( comp is DisplayComponent ) {
                data = ((DisplayComponent)comp).GetData();
            }
            else if( comp is DistributionComponent ) {
                data = ((DistributionComponent)comp).GetData();
            }
            else if( comp is PriceComponent ) {
                data = ((PriceComponent)comp).GetData();
            }
            else if( comp is MediaComponent ) {
                data = ((MediaComponent)comp).GetData();
            }
            else if( comp is MarketUtilityComponent ) {
                data = ((MarketUtilityComponent)comp).GetData();
            }
            return data.Length;
        }

        /// <summary>
        /// Display the current data item
        /// </summary>
        private void DisplayDataItem() {
            DisplayComponentType();
            valueNumLabel.Text = String.Format( "Value {0} of {1}", dataIndex + 1, data.Length );

            ComponentData dataItem = data[ dataIndex ];

            startDateTimePicker.Value = dataItem.StartDate;
            endDateTimePicker.Value = dataItem.EndDate;

            productLabel.Text = comp.Product.Name;
            awareLabel.Enabled = false;
            persLabel.Enabled = false;
            distLabel.Enabled = false;
            postUseUtilLabel.Enabled = false;
            priceLabel.Enabled = false;
            priceTypeLabel.Enabled = false;
            mediaTypeLabel.Enabled = false;
            grpsLabel.Enabled = false;
            marketUtilLabel.Enabled = false;
            displayLabel.Enabled = false;

            awarenessTextBox.Clear();
            persuasionTextBox.Clear();
            distTextBox.Clear();
            postUseUtilityTextBox.Clear();
            priceTextBox.Clear();
            ptypeTextBox.Clear();
            grpsTextBox.Clear();
            mtypeTextBox.Clear();
            utilityTextBox.Clear();
            displayTextBox.Clear();

            if( comp is CouponsComponent ) {
            }
            else if( comp is DisplayComponent ) {
                awarenessTextBox.Text = String.Format( "{0:f2}", ((DisplayComponentData)dataItem).Awareness );
                persuasionTextBox.Text = String.Format( "{0:f2}", ((DisplayComponentData)dataItem).Persuasion );
                displayTextBox.Text = String.Format( "{0:f3}", ((DisplayComponentData)dataItem).PercentDisplay );
                awareLabel.Enabled = true;
                persLabel.Enabled = true;
                displayLabel.Enabled = true;
            }
            else if( comp is DistributionComponent ) {
                awarenessTextBox.Text = String.Format( "{0:f2}", ((DistributionComponentData)dataItem).Awareness );
                persuasionTextBox.Text = String.Format( "{0:f2}", ((DistributionComponentData)dataItem).Persuasion );
                distTextBox.Text = String.Format( "{0:f3}", ((DistributionComponentData)dataItem).PercentDistribution );
                postUseUtilityTextBox.Text = String.Format( "{0:f3}", ((DistributionComponentData)dataItem).PostUseDistribution );
                awareLabel.Enabled = true;
                persLabel.Enabled = true;
                distLabel.Enabled = true;
                postUseUtilLabel.Enabled = true;
            }
            else if( comp is PriceComponent ) {
                awarenessTextBox.Text = String.Format( "{0:f2}", ((PriceComponentData)dataItem).Awareness );
                persuasionTextBox.Text = String.Format( "{0:f2}", ((PriceComponentData)dataItem).Persuasion );
                priceTextBox.Text = String.Format( "{0:f3}", ((PriceComponentData)dataItem).Price );
                ptypeTextBox.Text = ((PriceComponentData)dataItem).PriceType.ToString();
                awareLabel.Enabled = true;
                persLabel.Enabled = true;
                priceLabel.Enabled = true;
                priceTypeLabel.Enabled = true;
            }
            else if( comp is MediaComponent ) {
                awarenessTextBox.Text = String.Format( "{0:f2}", ((MediaComponentData)dataItem).Awareness );
                persuasionTextBox.Text = String.Format( "{0:f2}", ((MediaComponentData)dataItem).Persuasion );
                grpsTextBox.Text = String.Format( "{0:f3}", ((MediaComponentData)dataItem).TotalGRPs );
                awareLabel.Enabled = true;
                persLabel.Enabled = true;
                grpsLabel.Enabled = true;
            }
            else if( comp is MarketUtilityComponent ) {
                awarenessTextBox.Text = String.Format( "{0:f2}", ((MarketUtilityComponentData)dataItem).Awareness );
                persuasionTextBox.Text = String.Format( "{0:f2}", ((MarketUtilityComponentData)dataItem).Persuasion );
                distTextBox.Text = String.Format( "{0:f3}", ((MarketUtilityComponentData)dataItem).PercentDistribution );
                utilityTextBox.Text = String.Format( "{0:f3}", ((MarketUtilityComponentData)dataItem).MarketUtilityValue );
                awareLabel.Enabled = true;
                persLabel.Enabled = true;
                distLabel.Enabled = true;
                marketUtilLabel.Enabled = true;
            }
        }

        /// <summary>
        /// Display a component with no data values.
        /// </summary>
        private void DisplayNoData() {
            DisplayComponentType();

            valueNumLabel.Text = "0 data values loaded";
            productLabel.Text = "";

            saveButton.Enabled = false;
            nextButton.Enabled = false;
            prevButton.Enabled = false;

            awarenessTextBox.Clear();
            persuasionTextBox.Clear();
            distTextBox.Clear();
            postUseUtilityTextBox.Clear();
            priceTextBox.Clear();
            ptypeTextBox.Clear();
            grpsTextBox.Clear();
            mtypeTextBox.Clear();
            utilityTextBox.Clear();
        }

        /// <summary>
        /// Display the type of the component
        /// </summary>
        private void DisplayComponentType() {
            if( comp is CouponsComponent ) {
                typeLabel.Text = "Coupons";
            }
            else if( comp is DisplayComponent ) {
                typeLabel.Text = "Display";
            }
            else if( comp is DistributionComponent ) {
                typeLabel.Text = "Distribution";
            }
            else if( comp is PriceComponent ) {
                typeLabel.Text = "Price";
            }
            else if( comp is MediaComponent ) {
                typeLabel.Text = "Media";
            }
            else if( comp is MarketUtilityComponent ) {
                typeLabel.Text = "Market Utility";
            }

            string dateInfo = String.Format( "  -- {0} to {1}", comp.StartDate.ToShortDateString(), comp.EndDate.ToShortDateString() );
            typeLabel.Text += dateInfo;
        }
    }
}
       