using System;

namespace DPLicense
{

	// base class holds utilities
	public class LicComputer
	{
		// fields
		private static DateTime startDate = new DateTime(1922, 2, 2, 0, 0, 0);
		private string userName = "";
		private string key = "";

		// properties
		public virtual string UserName
		{
			get
			{
				return userName;
			}

			set
			{
				userName = value;
			}
		}

		
		public virtual string LicenseKey
		{
			get
			{
				return key;
			}

			set 
			{
				key = value;
			}
		}

		public DateTime ExpirationDate
		{
			get
			{				
				if (!ValidKey)
					return startDate;

				int first;
				int second;

				computeKeyNums(out first, out second);
					
				int xdate = (first + second)/2;

				DateTime expiresOn = startDate + new TimeSpan(xdate, 0, 0, 0);

				return expiresOn;
			}

			set
			{
				// compute a new key based on current userName
				// not to be used by user code
				// this is internal tool for generating keys
				computeKey(value);
			}
		}


		
		public bool ValidKey
		{
			get
			{
				int code = computeCode();

				int first;
				int second;

				computeKeyNums(out first, out second);

				int computekey = first - second;

				if (computekey == 2 * code)
					return true;

				return false;
			}
		}


		// constructor
		public LicComputer() {}

		// first and seecond should be positive
		private void computeKeyNums(out int first, out int second)
		{
			first = 0;
			second = 0;

			if (key == null)
				return;

			if (key.Length != 8)
				return;

			string firstString = key.Substring(0,4).Replace("H", "7");
			string secondString = key.Substring(4,4).Replace("G", "7");

			try
			{
				first = System.Convert.ToInt32(firstString,16);
				second = System.Convert.ToInt32(secondString, 16);
			}
			catch(Exception e) 
			{
				string message = e.Message;
			}
		}

		private int computeCode()
		{
			if (userName == null)
				return 0;

			// compute from key if possible
			int code = 0;

			for(int ii = 0; ii < userName.Length; ++ii)
			{
				code += (int) userName[ii];
			}

			return code;
		}

		private void computeKey(DateTime expireDate)
		{
			int code = computeCode();

			TimeSpan span = expireDate - startDate;

			int numDays = span.Days;

			int firstNum = numDays + code;
			int secondNum = numDays - code;

			if (firstNum < 0 || secondNum < 0)
			{
				key = "NOTAVALIDKEY";
			}
			else
			{
				string first;
				string second;

				if (numDays % 2 == 0)
					first = firstNum.ToString("X").Replace("7", "H");
				else
					first = firstNum.ToString("X");

				if (numDays % 3 == 0)
					second = secondNum.ToString("X").Replace("7", "G");
				else
					second = secondNum.ToString("X");

				key = first + second;
			}
		}
	}
}
