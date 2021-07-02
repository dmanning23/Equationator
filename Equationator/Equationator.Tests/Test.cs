using Equationator;
using NUnit.Framework;

namespace EquationatorTest
{
	[TestFixture]
	public class EquationatorTest
	{
		#region setup

		/// <summary>
		/// For testing purposes, this returns 1.0 instead of a random number.
		/// </summary>
		/// <returns>The float.</returns>
		static double RandomDouble()
		{
			return 1.0;
		}

		static double GetDifficulty()
		{
			return 2.0;
		}

		static double ParamFunc(int iIndex)
		{
			return iIndex - 1;
		}

		/// <summary>
		/// The equationator item we are going to test
		/// </summary>
		Equation equation;

		[SetUp]
		public void Init()
		{
			//create an equationator
			this.equation = new Equation();

			//add all the functions we will use
			equation.AddFunction("rand", RandomDouble);
			equation.AddFunction("rank", GetDifficulty);
		}

		#endregion //setup

		#region tests

		[Test]
		public void ParseEmpty()
		{
			equation.Parse("");
			Solve(0.0f);
		}

		[Test]
		public void ParseSingleNumber()
		{
			equation.Parse("4");
			Solve(4.0f);
		}

		[Test]
		public void ParseSingleFloatNumber()
		{
			equation.Parse("2.00");
			Solve(2.0f);
		}

		[Test]
		public void ParseSingleSmallNumber()
		{
			equation.Parse("0.1");
			Solve(0.1);
		}

		[Test]
		public void ParseNegativeNumber()
		{
			equation.Parse("-4");
			Solve(-4.0f);
		}

		[Test]
		public void ParseRandFunctionCall()
		{
			equation.Parse("$rand");
			Solve(1.0f);
		}

		[Test]
		public void ParseNegativeFunctionCall()
		{
			equation.Parse("-$rand");
			Solve(-1.0f);
		}

		[Test]
		public void ParseRankFunctionCall()
		{
			equation.Parse("$rank");
			Solve(2.0f);
		}

		[Test]
		public void ParseSingleParameter()
		{
			equation.Parse("$1");
			Solve(0.0f);
			
			equation.Parse("$2");
			Solve(1.0f);
		}

		[Test]
		public void ParseNegativeSingleParameter1()
		{
			equation.Parse("-$1");
			Solve(0.0f);
		}

		[Test]
		public void ParseNegativeSingleParameter2()
		{
			equation.Parse("-$2");
			Solve(-1.0f);
		}

		[Test]
		public void ParseSimpleEquation()
		{
			equation.Parse("1+1");
			Solve(2.0f);
		}

		[Test]
		public void ParseSimpleEquationWhiteSpace1()
		{
			equation.Parse("1 +1");
			Solve(2.0f);
		}

		[Test]
		public void ParseSimpleEquationWhiteSpace2()
		{
			equation.Parse("1+ 1");
			Solve(2.0f);
		}

		[Test]
		public void ParseSimpleEquationWhiteSpace3()
		{
			equation.Parse("1 + 1");
			Solve(2.0f);
		}

		[Test]
		public void ParseSimpleEquationWhiteSpaceNewLines()
		{
			equation.Parse("\n1\n +\n1\n");
			Solve(2.0f);
		}

		[Test]
		public void ParseSimpleEquationWhiteSpaceTabs()
		{
			equation.Parse("\n1\t\n +\n\t\t\n\n\n\t\t\t1\n");
			Solve(2.0f);
		}

		[Test]
		public void ParseEquationWithFloats()
		{
			equation.Parse("1 + 3.0 * 0.5");
			Solve(2.5f);
		}

		[TestCase("1 - 2 + 3.0 * 0.5", 0.5f)]
		[TestCase("1 * 0.5", 0.5f)]
		[TestCase("1 * 0.5 - 2", -1.5f)]
		[TestCase("1 * 0.5 - 2 + 3.0", 1.5f)]
		public void ParseOrderOfOperations(string texEquation, float expectedResult)
		{
			equation.Parse(texEquation);
			Solve(expectedResult);
		}

		[Test]
		public void ParseNegativeEquation()
		{
			equation.Parse("-(1+1)");
			Solve(-2.0f);
		}

		[Test]
		public void ParseSubequation()
		{
			//parse a more complicated text equation with subequation in parens

			//parens at the beginning
			equation.Parse("(1+3.0)*.5");
			Solve(2.0f);
		}

		[Test]
		public void ParseSubequationAtEnd()
		{

			//parens at the end
			equation.Parse("0.5*(1+3.0)");
			Solve(2.0f);
		}

		[Test]
		public void ParseSubequationInMiddle()
		{
			//parens in the middle
			equation.Parse("0.5*(1+3.0)-1");
			Solve(1.0f);
		}

		[Test]
		public void ParseDoubleSubequation()
		{
			//parens at both ends
			equation.Parse("(3-1)*(1+1)");
			Solve(4.0f);
		}

		[Test]
		public void ParseComplextEquation()
		{
			//parse a super complicated equation with parens in parens
			equation.Parse("((3-1)*(1+1))/2");
			Solve(2.0f);
		}

		[Test]
		public void ParseFunctionCallInEquation()
		{
			//try doing a function call in an equation, just to make sure it is parsing them correctly
			equation.Parse("$rank+1");
			Solve(3.0f);
		}

		[Test]
		public void TestExponent()
		{
			equation.Parse("2^3");
			Solve(8.0f);
		}

		[Test]
		public void TestDivideByZero()
		{
			equation.Parse("1/0");
			Solve(0.0f);
		}

		[Test]
		public void TestModulo()
		{
			equation.Parse("5 % 2");
			Solve(1.0f);
		}

		[Test]
		public void TestModuloByZero()
		{
			equation.Parse("5%0");
			Solve(0.0f);
		}

		private void Solve(double desiredResult)
		{
			Assert.AreEqual(desiredResult, equation.Solve(ParamFunc));
		}

		#endregion //tests
	}
}
