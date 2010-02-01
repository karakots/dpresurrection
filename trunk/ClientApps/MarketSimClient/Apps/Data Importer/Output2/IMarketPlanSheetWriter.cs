using System;
using System.Collections;
using System.Text;

using DataImporter.Library;
using DataImporter.ImportSettings;

namespace DataImporter.Output2
{
    public interface IMarketPlanSheetWriter
    {
        void WriteData( WorksheetSettings worksheetSettings, ProjectSettings currentProject, ExcelWriter2 writer, int channelIndex );
    }
}
