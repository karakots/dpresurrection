using System;
using System.Data;

namespace DataImporter.Dialogs
{
    public interface IWorksheetSelectingForm
    {
        DataTable SheetNamesTable { get; }
    }
}
