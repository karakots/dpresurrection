using System;
using System.Collections;
using System.Text;

using NitroReader.Library;

namespace NitroReader.Output
{
    public interface IMarketPlanSheetWriter
    {
        void WriteData( string sheetName, int startDataRow, int startDataColumn, MarketPlan marketPlan, Settings settings, ExcelWriter2 writer );
    }
}
