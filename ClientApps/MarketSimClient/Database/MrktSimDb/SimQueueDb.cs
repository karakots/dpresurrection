using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace MrktSimDb
{
    public class SimQueueDb : ProjectDb
    {
        protected override bool TableInSchema(DataTable table)
        {
            if (base.TableInSchema(table))
                return true;

            if (table == Data.sim_queue)
                return true;

            return false;
        }

        protected override void InitializeSelectQuery()
        {
            base.InitializeSelectQuery();

            OleDbDataAdapter adapter = null;

            // model data

            if (this.ProjectID >= 0)
            {
                //  sim queue
                adapter = getAdapter(Data.sim_queue);

                if (adapter != null)
                {
                    adapter.SelectCommand.CommandText += " WHERE model_id IN (SELECT model_id FROM Model_info WHERE project_id = " + this.ProjectID + ")";
                }
            }
        }
    }
}
