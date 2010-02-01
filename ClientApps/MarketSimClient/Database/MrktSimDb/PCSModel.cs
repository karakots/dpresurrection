using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace MrktSimDb
{
    public class PCSModel : Database
    {
        private int modId = Database.AllID;
        public int ModelID
        {
            get
            {
                return modId;
            }

            set
            {
                modId = value;

                // update adapters
                InitializeSelectQuery();
            }
        }

        private MrktSimDBSchema.Model_infoRow model = null;
        public MrktSimDBSchema.Model_infoRow Model
        {
            get
            {
               // if (model == null)
                    model = (MrktSimDBSchema.Model_infoRow)Data.Model_info.Rows.Find(ModelID);

                return model;
            }
        }

        public bool IsNimo {
            get { 
                return Model.app_code == Database.AppCode; 
            }
            set {
                if( value == true ) {
                    Model.app_code = Database.AppCode;
                }
                else {
                    Model.app_code = "";
                }
            }
        }

        public DateTime StartDate
		{
			get
			{
				if (Model != null)
					return Model.start_date;

				return DateTime.MinValue.Date;
			}

			set
			{

                if (Model != null)
				{
					Model.start_date = value;
				}
			}

        }

        public DateTime EndDate
        {
            get
            {
                if (Model != null)
                    return Model.end_date;

                return DateTime.MaxValue.Date;
            }

            set
            {
                if (Model != null)
                {
                    Model.end_date = value;
                }
            }
        }

        protected override bool TableInSchema(DataTable table)
        {
            if (base.TableInSchema(table))
                return true;

            if (table == Data.product_type ||
               table == Data.product ||
               table == Data.product_tree ||
               table == Data.segment ||
               table == Data.channel ||
               table == Data.pack_size)
                return true;

            return false;
        }

        protected override void InitializeSelectQuery()
        {
            base.InitializeSelectQuery();

            // add addtional filters for other tables
            OleDbDataAdapter adapter = null;

            adapter = getAdapter(Data.project);
            adapter.SelectCommand.CommandText += " WHERE id IN (SELECT project_id FROM Model_Info WHERE model_id = " + ModelID + ")";

            adapter = getAdapter(Data.Model_info);
            adapter.SelectCommand.CommandText += " WHERE model_id = " + ModelID;

            adapter = getAdapter(Data.product);
            adapter.SelectCommand.CommandText += " WHERE model_id = " + ModelID;

            adapter = getAdapter(Data.product_type);
            adapter.SelectCommand.CommandText += " WHERE model_id = " + ModelID;

            adapter = getAdapter(Data.product_tree);
            adapter.SelectCommand.CommandText += " WHERE model_id = " + ModelID;

            adapter = getAdapter(Data.segment);
            adapter.SelectCommand.CommandText += " WHERE model_id = " + ModelID;

            adapter = getAdapter(Data.channel);
            adapter.SelectCommand.CommandText += " WHERE model_id = " + ModelID;

            adapter = getAdapter( Data.pack_size );
            adapter.SelectCommand.CommandText += " WHERE model_id = " + ModelID;
        }
    }
}
