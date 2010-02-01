using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace MrktSimDb
{
    public class SimLogDb : SimulationDb
    {

        protected override bool TableInSchema(DataTable table)
        {
            if (base.TableInSchema(table))
                return true;

            if (table == Data.product_type ||
                table == Data.run_log ||
                table == Data.product ||
                table == Data.segment ||
                table == Data.channel ||
                table == Data.pack_size)
                return true;

            return false;
        }

        private int run_id = -1;
        public int RunID
        {
            set
            {
                run_id = value;
            }
        }

        protected override void InitializeSelectQuery()
        {
            base.InitializeSelectQuery();

            OleDbDataAdapter adapter = null;

            // model data

            if (run_id >= 0)
            {
                adapter = getAdapter(Data.product_type);
                adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                    "(SELECT model_id FROM sim_queue WHERE run_id = " + run_id + ")";

                adapter = getAdapter(Data.product);
                adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                    "(SELECT model_id FROM sim_queue WHERE run_id = " + run_id + ")";

                adapter = getAdapter( Data.pack_size );
                adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                    "(SELECT model_id FROM sim_queue WHERE run_id = " + run_id + ")";

                adapter = getAdapter(Data.segment);
                adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                    "(SELECT model_id FROM sim_queue WHERE run_id = " + run_id + ")";

                adapter = getAdapter(Data.channel);
                adapter.SelectCommand.CommandText += " WHERE model_id IN " +
                    "(SELECT model_id FROM sim_queue WHERE run_id = " + run_id + ")";

                adapter = getAdapter(Data.run_log);
                adapter.SelectCommand.CommandText += " WHERE run_id = " + run_id;
            }
        }

    }
}
