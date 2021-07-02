using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Equationator
{
	/// <summary>
	/// This is a special node for getting random values
	/// </summary>
	public class RandNode : BaseNode
	{
		#region Members

		/// <summary>
		/// the rand object for getting values
		/// </summary>
		Random _rand = new Random();

		#endregion Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="Equationator.FunctionNode"/> class.
		/// </summary>
		public RandNode()
		{
			OrderOfOperationsValue = PemdasValue.Value;
		}

		/// <summary>
		/// Parse the specified tokenList and curIndex.
		/// overloaded by child types to do there own specific parsing.
		/// </summary>
		/// <param name="tokenList">Token list.</param>
		/// <param name="curIndex">Current index.</param>
		/// <param name="owner">the equation that this node is part of.  required to pull function delegates out of the dictionary</param>
		protected override void ParseToken(List<Token> tokenList, ref int curIndex, Equation owner)
		{
			//check arguments
			if (null == tokenList)
			{
				throw new ArgumentNullException("tokenList");
			}
			if (null == owner)
			{
				throw new ArgumentNullException("owner");
			}
			Debug.Assert(curIndex < tokenList.Count); //TODO: throw exceptions

			//increment the current index since we consumed the function name token
			curIndex++;
		}

		/// <summary>
		/// Solve the equation!
		/// This method recurses into the whole tree and returns a result from the equation.
		/// </summary>
		/// <param name="paramCallback">Parameter callback that will be used to get teh values of parameter nodes.</param>
		/// <param name="tierCallback">function callback that will be used to get the tier value at runtime.</param>
		/// <returns>The solution of this node and all its subnodes!</returns>
		public override double Solve(ParamDelegate paramCallback)
		{
			//return a random float between 0.0 and 1.0
			return _rand.NextDouble();
		}

		#endregion Methods
	}
}
