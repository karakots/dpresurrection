using System;
using System.Collections;

namespace ErrorInterface
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class ErrorList : ICollection
	{
		private const int maxErrors = 200;

		private ArrayList myErrors;

		public ErrorList()
		{
			myErrors = new ArrayList();
		}

		public int addError(Object obj, string error_type, string error_desc)
		{
			if (myErrors.Count < maxErrors)
			{

				Error error = new Error(obj, error_type, error_desc);
				myErrors.Add(error);
			}
			else if (myErrors.Count == maxErrors)
			{
				Error error = new Error(obj, "maximum errors reached", "Error count over " + maxErrors);
				myErrors.Add(error);
			}

			return myErrors.Count;
		}

		public int addError(Error error)
		{
			myErrors.Add(error);
			return myErrors.Count;
		}

		public int addErrors(ErrorList errors)
		{
			myErrors.AddRange(errors);
			return myErrors.Count;
		}
		
		public Error getError(int index)
		{
			return (Error)myErrors[index];
		}

		public ErrorList getErrors(string error_type)
		{
			ErrorList errors = new ErrorList();
			foreach(Error error in myErrors)
			{
				if(error.Type == error_type)
				{
					errors.addError(error);
				}
			}
			return errors;
		}

		public ErrorList getErrors(Object obj)
		{
			return this;
		}

		public void Clear()
		{
		}

		public void Display()
		{
			if(Count > 0)
			{
				ErrorDialog dlg = new ErrorDialog(this);
				dlg.ShowDialog();
			}
		}

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return myErrors.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return myErrors.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			myErrors.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return myErrors.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return myErrors.GetEnumerator();
		}

		#endregion
	}

	public class Error
	{
		private object myObject;
		private string myType;
		private string myError;

		public Error()
		{
			Object = null;
			Type = "";
			ErrorDesc = "";
		}

		public Error(object obj, string error_type, string error_desc)
		{
			Object = obj;
			Type = error_type;
			ErrorDesc = error_desc;

			ErrorList myList = new ErrorList();
		}

		public object Object
		{
			get
			{
				return myObject;
			}
			set
			{
				myObject = value;
			}
		}

		public string Type
		{
			get
			{
				return myType;
			}
			set
			{
				myType = value;
			}
		}

		public string ErrorDesc
		{
			get
			{
				return myError;
			}
			set
			{
				myError = value;
			}
		}

		public override string ToString()
		{
			return Type + ": " + ErrorDesc;
		}
		
	}

	
}
