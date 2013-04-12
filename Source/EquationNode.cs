using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Equationator
{
	public class EquationNode : BaseNode
	{
		#region Members

		/// <summary>
		/// An equation node holds an entire sub-equation that is contained in parenthesis
		/// </summary>
		/// <value>The sub equation.</value>
		private BaseNode SubEquation { get; set; }
		
		#endregion Members
		
		#region Methods
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Equationator.FunctionNode"/> class.
		/// </summary>
		public EquationNode()
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
			Debug.Assert(null != tokenList);
			Debug.Assert(null != owner);
			Debug.Assert(curIndex < tokenList.Count);

			//parse the equation into our subnode
			SubEquation = BaseNode.Parse(tokenList, ref curIndex, owner);

			//if some smart ass types in () the parse method wouldve returned null.  
			//it should evaluate to 0 in that case, so create a number node and set the value.
			if (null == SubEquation)
			{
				NumberNode fakeNode = new NumberNode();
				fakeNode.NumberValue = 0.0f;
				SubEquation = fakeNode;
			}
			Debug.Assert(null != SubEquation);

			//treeify the subequation so we can solve it
			SubEquation = SubEquation.Treeify();
			Debug.Assert(null != SubEquation);
		}

		/// <summary>
		/// Solve the equation!
		/// This method recurses into the whole tree and returns a result from the equation.
		/// </summary>
		/// <param name="paramCallback">Parameter callback that will be used to get teh values of parameter nodes.</param>
		/// <returns>The solution of this node and all its subnodes!</returns>
		public override float Solve(ParamDelegate paramCallback)
		{
			//Return the sub equation solver
			Debug.Assert(null != SubEquation);
			return SubEquation.Solve(paramCallback);
		}
		
		#endregion Methods
	}
}