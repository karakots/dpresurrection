using System;
using System.Collections;
using System.Text;

using MrktSimDb;
using DecisionPower.MarketSim.ScenarioManagerLibrary.Data;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary.Components
{
    public class MediaComponent : Component
    {
         static MediaComponent() {
            defaultAwareness = 0.2;       //!!! need to check these values!!!
            defaultPersuasion = 0.35;      
        }

        /// <summary>
        /// Returns an array of items that contain the current data values for this component
        /// </summary>
        /// <returns></returns>
        public MediaComponentData[] GetData() {
            return GetData( DateTime.MinValue, DateTime.MaxValue, Enums.DataIntervalCheckType.DataEndsInInterval, Channel.All );
        }

        /// <summary>
        /// Returns an array of items that contain the current data values for this component
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="dataCheckType"></param>
        /// <returns></returns>
        public MediaComponentData[] GetData( DateTime start, DateTime end, Enums.DataIntervalCheckType dataCheckType ) {
            return GetData( start, end, dataCheckType, Channel.All );
        }

        /// <summary>
        /// Returns an array of items that contain the current data values for this component
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="dataCheckType"></param>
        /// <param name="channel"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        public MediaComponentData[] GetData( DateTime start, DateTime end, Enums.DataIntervalCheckType dataCheckType, Channel channel ) {

            string dataQuery = DataQuery( start, end, dataCheckType, channel );

            MrktSimDBSchema.mass_mediaRow[] rows = (MrktSimDBSchema.mass_mediaRow[])this.dataTable.Select( dataQuery );

            int rowCount = rows.Length;
            MediaComponentData[] data = new MediaComponentData[ rowCount ];
            for( int i = 0; i < rowCount; i++ ) {

                data[ i ] = new MediaComponentData( this.model, rows[ i ] );
            }
            return data;
        }

        /// <summary>
        /// Adds new data to the Component.  The new data is specified by an array of ComponentData objects.
        /// </summary>
        /// <param name="newComponentData"></param>
        /// <returns></returns>
        public int AddData( MediaComponentData[] newComponentData ) {

            for( int i = 0; i < newComponentData.Length; i++ ) {
                MrktSimDBSchema.mass_mediaRow newRow = (MrktSimDBSchema.mass_mediaRow)this.dataTable.NewRow();

                newRow.start_date = newComponentData[ i ].StartDate;
                newRow.end_date = newComponentData[ i ].EndDate;
                newRow.attr_value_G = newComponentData[ i ].TotalGRPs;
                newRow.message_awareness_probability = newComponentData[ i ].Awareness;
                newRow.message_persuation_probability = newComponentData[ i ].Persuasion;

                newRow.model_id = this.Model.ID;
                newRow.market_plan_id = this.componentRow.id;
                newRow.product_id = this.product.ID;
                newRow.channel_id = this.channel.ID;
                newRow.segment_id = this.segment.ID;
                newRow.media_type = "V";

                newComponentData[ i ].DataRow = newRow;
                this.dataTable.Rows.Add( newRow );
            }

            return newComponentData.Length;
        }

        /// <summary>
        /// Returns a copy of the Component.  The component is automatically added to all market plans using the original.
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="newDescription"></param>
        /// <returns></returns>
        public new MediaComponent Copy( string newName, string newDescription ) {
            MrktSimDBSchema.market_planRow copyRow = model.ModelDb.CopyPlan( this.componentRow );
            MediaComponent copyComponent = new MediaComponent( this.model, copyRow );
            copyComponent.Name = newName;
            copyComponent.Description = newDescription;
            return copyComponent;
        }

        /// <summary>
        /// Constructs a new MediaComponent object.  For framework usage only.
        /// </summary>
        /// <param name="marketPlan">Market plan that contains the component</param>
        /// <param name="componentRow">Internal representation of the component</param>
        public MediaComponent( Model model, MrktSimDBSchema.market_planRow componentRow ) : base( model, componentRow ) {

            this.dataTable = theDb.Data.mass_media;
        }
   }
}
