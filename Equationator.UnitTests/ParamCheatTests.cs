using Equationator;
using NUnit.Framework;

namespace EquationatorTest
{
    [TestFixture]
    class ParamCheatTests
    {
        static float ParamFunc(int iIndex)
        {
            return iIndex - 1;
        }

        Equation equation;

        [SetUp]
        public void Init()
        {
            //create an equationator
            this.equation = new Equation();
        }

        [Test]
        public void ParseX()
        {
            equation.Parse("x");
            Solve(0.0f);
        }

        [Test]
        public void ParseY()
        {

            equation.Parse("y");
            Solve(1.0f);
        }

        [Test]
        public void ParseZ()
        {
            equation.Parse("z");
            Solve(2.0f);
        }

        private void Solve(float desiredResult)
        {
            Assert.AreEqual(desiredResult, equation.Solve(ParamFunc, () => { return 0.0f; }));
        }
    }
}
