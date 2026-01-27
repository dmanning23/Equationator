using Equationator;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace EquationatorTest
{
	[TestFixture]
	public class FunctionTests
	{
		Dictionary<int, double> parameters = new Dictionary<int, double>();

		double ParamFunc(int index)
		{
			return parameters[index];
		}

		double tier;

		double TierFunc()
		{
			return tier;
		}

		double health;

		double HealthFunc()
		{
			return health;
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
		public void AddTier()
		{
			tier = 1.0;
			equation.AddFunction("tier", TierFunc);
			equation.Parse("$tier");
			equation.Solve(ParamFunc).ShouldBe(1.0);
		}

		[Test]
		public void ChangeParam()
		{
			tier = 1.0;
			equation.AddFunction("tier", TierFunc);
			equation.Parse("$tier");

			tier = 2.0;

			equation.Solve(ParamFunc).ShouldBe(2.0);
		}

		[Test]
		public void TwoFuncs()
		{
			tier = 1.0;
			health = 2.0;
			equation.AddFunction("tier", TierFunc);
			equation.AddFunction("hlth", HealthFunc);
			equation.Parse("$tier + $hlth");
			equation.Solve(ParamFunc).ShouldBe(3.0);
		}
	}
}
