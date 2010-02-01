using System;
using System.IO;

namespace MarketSimUtilities
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class FileCleaner
	{
		public FileCleaner() {}

		public void clean(string inFile, string outFile)
		{
			StreamWriter writer = new System.IO.StreamWriter(outFile);


			TextReader reader = null;

			reader = File.OpenText(inFile);

			string text = reader.ReadLine();

			while(text != null)
			{
				if(text.IndexOf("<start_date>") > 0 || 
					text.IndexOf("<end_date>") > 0 || 
					text.IndexOf("<metric_start_date>") > 0 || 
					text.IndexOf("<metric_end_date>") > 0 || 
					text.IndexOf("<checkpoint_date>") > 0 || 
					text.IndexOf("<calendar_date>") > 0)
				{
					int tIndex = text.IndexOf("T");
					int endIndex = text.IndexOf("<",tIndex);
					text = text.Remove(tIndex,endIndex-tIndex);
				}
				writer.WriteLine(text);
				text = reader.ReadLine();
			}

			reader.Close();
			writer.Flush();
			writer.Close();
		}
	}
}
