using System;

namespace BrandManager.Forms
{

	public delegate void Finished();
	/// <summary>
	/// Summary description for Wizard.
	/// </summary>
	public interface Wizard
	{
		bool Next();

		bool Back();

		void Start();

		void End();

		event Finished Done;
	}
}
