using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Equationator
{
	/// <summary>
	/// This node is an operator in the equation.
	/// </summary>
	public class OperatorNode : BaseNode
	{
		#region Members
		
		#endregion Members
		
		#region Methods
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Equationator.FunctionNode"/> class.
		/// </summary>
		public OperatorNode()
		{
			//default to invalid, this will be set by the Parse function
			OrderOfOperationsValue = PemdasValue.Invalid;
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

			//make sure the token text is the correct length
			if (tokenList[curIndex].TokenText.Length != 1)
			{
				throw new FormatException("operator text length can only be one character, was given " + tokenList[curIndex].TokenText);
			}
			
			//get the operator
			char oper = tokenList[curIndex].TokenText[0];
			
			//what operator is it?
			switch (oper)
			{
				case '^':
				{
					OrderOfOperationsValue = PemdasValue.Exponent;
				}
				break;
				case '*':
				{
					OrderOfOperationsValue = PemdasValue.Multiplication;
				}
				break;
				case '/':
				{
					OrderOfOperationsValue = PemdasValue.Division;
				}
				break;
				case '+':
				{
					OrderOfOperationsValue = PemdasValue.Addition;
				}
				break;
				case '-':
				{
					OrderOfOperationsValue = PemdasValue.Subtraction;
				}
				break;
				default:
				{
					throw new FormatException("invalid operator text: " + oper);
				}
			}
			
			//increment the current index since we consumed the operator token
			curIndex++;
		}
		
		#endregion Methods
	}
}