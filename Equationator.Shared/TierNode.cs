using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Equationator
{
	public class TierNode : BaseNode
	{
		#region Members

		#endregion Members
		
		#region Methods
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Equationator.FunctionNode"/> class.
		/// </summary>
		public TierNode()
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
			
			//nothing to grab here, tier is solved at runtime
			
			//increment the current index since we consumed the function name token
			curIndex++;
		}

		/// <summary>
		/// Solve the equation!
		/// This method recurses into the whole tree and returns a result from the equation.
		/// </summary>
		/// <param name="paramCallback">Parameter callback that will be used to get teh values of parameter nodes.</param>
		/// <returns>The solution of this node and all its subnodes!</returns>
		public override float Solve(ParamDelegate paramCallback, FunctionDelegate tierCallback)
		{
			//Return the function we found in the parser
			Debug.Assert(null != tierCallback); //TODO: throw exceptions
			return tierCallback();
		}
		
		#endregion Methods
	}
}