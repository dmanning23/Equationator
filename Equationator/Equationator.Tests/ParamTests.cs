using Equationator;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace EquationatorTest
{
	[TestFixture]
	public class ParamTests
	{
		Dictionary<int, double> parameters = new Dictionary<int, double>();

		double ParamFunc(int index)
		{
			return parameters[index];
		}

		double RankFunc()
		{
			return 0.0;
		}

		Equation equation;

		[SetUp]
		public void Setup()
		{
			//create an equationator
			this.equation = new Equation();
		}

		[Test]
		public void OneParam()
		{
			parameters[1] = 1.0;
			equation.Parse("$1");
			equation.Solve(ParamFunc).ShouldBe(1.0);
		}

		[Test]
		public void ChangeParam()
		{
			parameters[1] = 1.0;
			equation.Parse("$1");

			parameters[1] = 2.0;

			equation.Solve(ParamFunc).ShouldBe(2.0);
		}
	}
}
