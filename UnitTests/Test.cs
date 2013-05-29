using System;
using Equationator;
using NUnit.Framework;

namespace EquationatorTest
{
	[TestFixture]
	public class EquationatorTest
	{
		#region setup

		static private Random g_Random = new Random(DateTime.Now.Millisecond);

		/// <summary>
		/// For testing purposes, this returns 1.0 instead of a random number.
		/// </summary>
		/// <returns>The float.</returns>
		static float RandomFloat()
		{
			return 1.0f;
		}

		static float GetDifficulty()
		{
			return 2.0f;
		}

		static float ParamFunc(int iIndex)
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
			equation.AddFunction("rand", RandomFloat);
			equation.AddFunction("rank", GetDifficulty);
		}

		#endregion //setup

		#region tests

		[Test]
		public void ParseSingleNumber()
		{
			equation.Parse("4");
			Assert.AreEqual(4.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseSingleFloatNumber()
		{
			equation.Parse("2.00");
			Assert.AreEqual(2.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseSingleSmallNumber()
		{
			equation.Parse("0.1");
			Assert.AreEqual(0.1f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseNegativeNumber()
		{
			equation.Parse("-4");
			Assert.AreEqual(-4.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseRandFunctionCall()
		{
			equation.Parse("$rand");
			Assert.AreEqual(1.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseNegativeFunctionCall()
		{
			equation.Parse("-$rand");
			Assert.AreEqual(-1.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseRankFunctionCall()
		{
			equation.Parse("$rank");
			Assert.AreEqual(2.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseSingleParameter()
		{
			equation.Parse("$1");
			Assert.AreEqual(0.0f, equation.Solve(ParamFunc));
			
			equation.Parse("$2");
			Assert.AreEqual(1.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseNegativeSingleParameter1()
		{
			equation.Parse("-$1");
			Assert.AreEqual(0.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseNegativeSingleParameter2()
		{
			equation.Parse("-$2");
			Assert.AreEqual(-1.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseSimpleEquation()
		{
			equation.Parse("1+1");
			Assert.AreEqual(2.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseSimpleEquationWhiteSpace1()
		{
			equation.Parse("1 +1");
			Assert.AreEqual(2.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseSimpleEquationWhiteSpace2()
		{
			equation.Parse("1+ 1");
			Assert.AreEqual(2.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseSimpleEquationWhiteSpace3()
		{
			equation.Parse("1 + 1");
			Assert.AreEqual(2.0f, equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseEquationWithFloats()
		{
			equation.Parse("1 + 3.0 * 0.5");
			Assert.AreEqual(2.5f, equation.Solve(ParamFunc)); //should be 2.5
		}

		[Test]
		public void ParseOrderOfOperations()
		{
			equation.Parse("1 - 2 + 3.0 * 0.5");
			Assert.AreEqual(-2.5f, equation.Solve(ParamFunc)); //should be -2.5
		}

		[Test]
		public void ParseNegativeEquation()
		{
			equation.Parse("-(1+1)");
			Console.WriteLine(equation.TextEquation);
			Console.WriteLine(equation.Solve(ParamFunc));
		}

		[Test]
		public void ParseSubequation()
		{
			//parse a more complicated text equation with subequation in parens

			//parens at the beginning
			equation.Parse("(1+3.0)*.5");
			Assert.AreEqual(2.0f, equation.Solve(ParamFunc)); //should be 2
		}

		[Test]
		public void ParseSubequationAtEnd()
		{

			//parens at the end
			equation.Parse("0.5*(1+3.0)");
			Assert.AreEqual(2.0f, equation.Solve(ParamFunc)); //should be 2
		}

		[Test]
		public void ParseSubequationInMiddle()
		{
			//parens in the middle
			equation.Parse("0.5*(1+3.0)-1");
			Assert.AreEqual(1.0f, equation.Solve(ParamFunc)); //should be 1
		}

		[Test]
		public void ParseDoubleSubequation()
		{
			//parens at both ends
			equation.Parse("(3-1)*(1+1)");
			Assert.AreEqual(4.0f, equation.Solve(ParamFunc)); //should be 4
		}

		[Test]
		public void ParseComplextEquation()
		{
			//parse a super complicated equation with parens in parens
			equation.Parse("((3-1)*(1+1))/2");
			Assert.AreEqual(2.0f, equation.Solve(ParamFunc)); //should be 2
		}

		[Test]
		public void ParseFunctionCallInEquation()
		{
			//try doing a function call in an equation, just to make sure it is parsing them correctly
			equation.Parse("$rank+1");
			Assert.AreEqual(3.0f, equation.Solve(ParamFunc)); //should be 3
		}

		#endregion //tests
	}
}
