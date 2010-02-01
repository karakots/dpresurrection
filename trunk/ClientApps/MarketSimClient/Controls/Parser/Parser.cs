using System;

namespace EquationParser
{
	/// <summary>
	/// We have three Objects here
	/// Variables
	/// Expressions
	/// Parser
	/// The Parser is given a string and a list of variables to look for
	/// Expressions are what we evaluate
	/// Variables can be turned into evaluatable expressions
	/// This was implemented so that a token string could represent an expression
	/// </summary>

    [SerializableAttribute]
    public abstract class Variable
	{

        public abstract string Token
        {

            get;

        }


		public abstract bool IsSimple
		{
			get;
		}

		public abstract string Equation
		{
			get;
		}
	}




	public abstract class ExpressionTree
	{
		
		public delegate double Functor(double val);
		public delegate double BiFunctor(double left, double right);

		public virtual ExpressionTree[] Variables
		{
			get
			{
				return new ExpressionTree[] {this};
			}
		}

		public abstract int NumTerms();

		public abstract double Evaluate();
		public abstract double Evaluate(Object obj);
		public abstract bool Valid();
	}


	public class ConstantNode : ExpressionTree
	{
		private double val = 0.0;

		public ConstantNode(double x)
		{
			val = x;
		}

		public override ExpressionTree[] Variables
		{
			get
			{
				return null;
			}
		}

		public override int NumTerms()
		{
			return 0;
		}

		public override double Evaluate()
		{
			return val;
		}

		public override double Evaluate(Object obj)
		{
			return val;
		}

		public override bool Valid()
		{
			return true;
		}

	}

	public class FactorNode : ExpressionTree
	{
		private ExpressionTree left;
		private ExpressionTree right;
		private BiFunctor functor;

		public override bool Valid()
		{
			if (left == null || !left.Valid())
				return false;

			
			if (right == null || !right.Valid())
				return false;
		
			return true;
		}

		public override ExpressionTree[] Variables
		{
			get
			{
				if (left == null && right == null)
					return null;

				if (left == null)
					return right.Variables;

				if (right == null)
					return left.Variables;

				ExpressionTree[] varsLeft = left.Variables;
				ExpressionTree[] varsRight = right.Variables;

				if (varsLeft == null)
					return varsRight;

				if (varsRight == null)
					return varsLeft;

				ExpressionTree[] vars = new ExpressionTree[varsLeft.Length + varsRight.Length];

				varsLeft.CopyTo(vars, 0);
				varsRight.CopyTo(vars, varsLeft.Length);

				return vars;
			}
		}

		public override int NumTerms()
		{
			int num = 0;

			if (left != null)
				num =  left.NumTerms();

			if (right != null)
				num += right.NumTerms();

			return num;
		}

		public override double Evaluate()
		{
			double	leftVal = 0.0;
			double rightval = 0.0;

			if (left != null)
				leftVal = left.Evaluate();

			if (right != null)
				rightval = right.Evaluate();

			return functor(leftVal, rightval);
		}

		public override double Evaluate(Object obj)
		{
			double	leftVal = 0.0;
			double rightval = 0.0;

			if (left != null)
				leftVal = left.Evaluate(obj);

			if (right != null)
				rightval = right.Evaluate(obj);

			return functor(leftVal, rightval);
		}

		public FactorNode(BiFunctor eval, ExpressionTree leftExpression, ExpressionTree rightExpression)
		{
			functor = eval;

			left = leftExpression;
			right = rightExpression;
		}
	}

	public class FunctorNode : ExpressionTree
	{
		private ExpressionTree subTree;
		private Functor functor = null;

		public override bool Valid()
		{
			if (subTree == null || !subTree.Valid())
				return false;
		
			return true;
		}

		public override ExpressionTree[] Variables
		{
			get
			{
				if (subTree == null)
					return null;

				return subTree.Variables;
			}
		}

		public override int NumTerms()
		{
			if (subTree != null)
				return subTree.NumTerms();

			return 0;
		}


		public override double Evaluate()
		{
			double val = 0.0;

			if (subTree != null)
				val = subTree.Evaluate();

			return functor(val);
		}

		public override double Evaluate(Object obj)
		{
			double val = 0.0;

			if (subTree != null)
				val = subTree.Evaluate(obj);

			return functor(val);
		}

		public FunctorNode(Functor eval, ExpressionTree expression)
		{
			functor = eval;
			subTree = expression;
		}
	}
	
	public class Parser
	{
		/// <summary>
		/// We need a method for turning a variable into an evaluatable expression
		/// Implemented this way rather then have the variables be self evaluating
		/// because there may be a context that the variable is being evaluated in
		/// </summary>
		public delegate ExpressionTree VarToExpression(Variable val);

		/// <summary>
		/// This must be set in order to parse variables
		/// </summary>
		public VarToExpression CreateExpression = null;

		/// <summary>
		/// The list of variables that we look for in an expression
		/// </summary>
		public Variable[] Variables = null;

		// Functors
		private static double negate(double val)
		{
			return -val;
		}

		// binary functors
		private static double add(double left, double right)
		{
			return left + right;
		}

		private static double subtract(double left, double right)
		{
			return left - right;
		}

		private static double multiply(double left, double right)
		{
			return left * right;
		}

		private static double divide(double left, double right)
		{
//			if (right == 0.0)
//			{
//				if (left == 0.0)
//					return 0.0;
//
//				if (left > 0.0)
//					return Double.PositiveInfinity;
//
//				if (left < 0.0)
//					return Double.NegativeInfinity;
//			}

			// yes this is not right but it avoids downstream complications
			// we will deal with this some other way
			// TODO fix this!
			if (right == 0.0)
				return 0.0;

			return left / right;
		}

		// string operations
		private static char[] whiteSpace = {' '};
		private static char[] doubleChars = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'};

		private string expressionString;
		
		/// <summary>
		/// FILL ME IN!!!
		/// </summary>
		/// <param name="stringToParse"></param>
		
		public Parser()
		{
			expressionString = null;
		}

		public Parser(string stringToParse)
		{
			expressionString = stringToParse;
		}

		public Parser(Variable var)
		{
			expressionString = var.Equation;
		}

		private ExpressionTree ParseUnaryOperator()
		{
			ExpressionTree.Functor func = null;
		
			if (expressionString.StartsWith("LOG"))
			{
				func = new ExpressionTree.Functor(Math.Log);
				// remove Log from beginning
				expressionString = expressionString.Remove(0,3).Trim(whiteSpace);
			}
			else if (expressionString.StartsWith("EXP"))
			{
				func = new ExpressionTree.Functor(Math.Exp);
				// remove exp from beginning
				expressionString = expressionString.Remove(0,3).Trim(whiteSpace);
			}

			if (func != null)
				return new FunctorNode(func, Factor());

			return null;
		}

		private ExpressionTree ParseVariable()
		{
			// look for basic evaluator
			foreach(Variable variable in Variables)
			{
				if (variable.Token == null)
					continue;

				if(expressionString.StartsWith(variable.Token))
				{	
					expressionString = expressionString.Remove(0, variable.Token.Length).Trim(whiteSpace);
					
					if (variable.IsSimple)
					{
						return CreateExpression(variable);
					}
					else
					{
						// create a new Parser and parse this string
						Parser variableParse = new Parser(variable);
						variableParse.CreateExpression = CreateExpression;
						variableParse.Variables = Variables;
						return variableParse.Parse();
					}
				}
			}

			return null;
		}

		private ExpressionTree ParseNumeric()
		{
			if (expressionString.IndexOfAny(doubleChars, 0, 1) != 0)
				return null;

			// find index of last integer
			int index = 1;

			while (index < expressionString.Length && expressionString.IndexOfAny(doubleChars, index, 1) == index)
				index++;

			string numString = expressionString.Substring(0, index);

			double val = Double.Parse(numString);

			expressionString = expressionString.Substring(index).Trim(whiteSpace);

			return new ConstantNode(val);
		}

		private ExpressionTree Factor()
		{
			ExpressionTree expression = ParseNumeric();

			if (expression != null)
				return expression;

			expression = ParseVariable();

			if (expression != null)
				return expression;

			if (expressionString.StartsWith("("))
			{
				expressionString = expressionString.Remove(0,1).Trim(whiteSpace);

				expression = Parse();

				if (expressionString.StartsWith(")"))
					expressionString = expressionString.Remove(0,1).Trim(whiteSpace);

				return expression;
			}

			return ParseUnaryOperator();
		}

		private ExpressionTree SignFactor()
		{
			if (expressionString.StartsWith("-"))
			{
				ExpressionTree.Functor func = new ExpressionTree.Functor(negate);

				expressionString = expressionString.Remove(0,1).Trim(whiteSpace);

				return new FunctorNode(func, Factor());
			}

			return Factor();
		}

		private ExpressionTree Term()
		{
			ExpressionTree expression = SignFactor();

			while (expressionString.StartsWith("^"))
			{
				ExpressionTree.BiFunctor func = new ExpressionTree.BiFunctor(Math.Pow);

				expressionString = expressionString.Remove(0,1).Trim(whiteSpace);

				expression = new FactorNode(func, expression, SignFactor());
			}
		
			return expression;
		}

		private ExpressionTree SimpleExpression()
		{
			ExpressionTree expression = Term();

			bool keepParsing = expressionString.StartsWith("*") || expressionString.StartsWith("/");

			while (keepParsing)
			{
				ExpressionTree.BiFunctor func = null;

				if (expressionString.StartsWith("*"))
					func  = new ExpressionTree.BiFunctor(multiply);
				else if (expressionString.StartsWith("/"))
					func  = new ExpressionTree.BiFunctor(divide);
				else
					keepParsing = false;

				if (keepParsing)
				{
					expressionString = expressionString.Remove(0,1).Trim(whiteSpace);

					expression = new FactorNode(func, expression, Term());
				}
			}

			return expression;
		}

		public ExpressionTree Parse()
		{
			// strip off white space from expressionString
			expressionString = expressionString.Trim(whiteSpace);

			if (expressionString == null)
				return null;

			if (expressionString.Length == 0)
				return null;

			ExpressionTree expression = SimpleExpression();

			bool keepParsing = expressionString.StartsWith("+") || expressionString.StartsWith("-");

			while (keepParsing)
			{
				ExpressionTree.BiFunctor func = null;

				if (expressionString.StartsWith("+"))
					func  = new ExpressionTree.BiFunctor(add);
				else if (expressionString.StartsWith("-"))
					func  = new ExpressionTree.BiFunctor(subtract);
				else
					keepParsing = false;

				if (keepParsing)
				{
					expressionString = expressionString.Remove(0,1).Trim(whiteSpace);

					expression = new FactorNode(func, expression, SimpleExpression());
				}
			}

			return expression;
		}
		public ExpressionTree Parse(string stringToParse)
		{
			expressionString = stringToParse;
			return this.Parse();
		}
	}

	public class SimpleParser : Parser
	{
		// Included for simplicity
		private class SimpleVariable : Variable
		{
		
			public override string Token
			{
				get
				{
					return token;
				}
			}


			public override bool IsSimple
			{
				get
				{
					return true;
				}
			}

			public override string Equation
			{
				get
				{
					return null;
				}
			}

			public SimpleVariable(string aToken)
			{
				token = aToken;
				val = 0.0;
			}

			private string token;
			private double val;

			public double Value
			{
				get
				{
					return val;
				}

				set
				{
					val = value;
				}
			}
		}

		public SimpleParser(string[] tokens)
		{
			CreateExpression = new VarToExpression(ExpressionFromSimpleVariable);

			SimpleVariable[] variables = new SimpleVariable[tokens.Length];

			for(int ii = 0; ii < tokens.Length; ++ii)
			{
				variables[ii] = new SimpleVariable(tokens[ii]);
			}

			this.Variables = variables; 
		}

		public double ParseEquation(string equation)
		{
			ExpressionTree expression = Parse(equation);

			if (expression == null)
				throw(new Exception("Term not recognized"));

			return expression.Evaluate();
		}

		public void updateValue(string token, double val)
		{
			for(int ii = Variables.Length - 1; ii >= 0 ; --ii)
			{
				if (((SimpleVariable) Variables[ii]).Token == token)
				{
					((SimpleVariable) Variables[ii]).Value = val;
					break;
				}
			}
		}

		public void updateValues(double[] vals)
		{
			for(int ii = 0; ii < vals.Length; ++ii)
			{
				((SimpleVariable) Variables[ii]).Value = vals[ii];
			}
		}

		static private ExpressionTree ExpressionFromSimpleVariable(Variable var)
		{
			SimpleVariable simple = var as SimpleVariable;

			if (simple == null)
				return null;

			return new ConstantNode(simple.Value);
		}
	}
}
