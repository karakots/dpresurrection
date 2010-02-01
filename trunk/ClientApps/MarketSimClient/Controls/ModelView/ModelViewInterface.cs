using System;

namespace ModelView
{
	/// <summary>
	/// Summary description for ModelViewInterface.
	/// </summary>
	public interface ModelViewInterface
	{
		event EventHandler Closed;
		void Close();
		void SaveModel();
		bool HasChanges();
	}
}
