using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Equationator
{
	/// <summary>
	/// This is a single node of the equation.
	/// All the other node types inherit from this one.
	/// </summary>
	public abstract class BaseNode
	{
		#region Members
			
		/// <summary>
		/// Gets or sets the previous node.
		/// </summary>
		/// <value>The previous.</value>
		protected BaseNode Prev { get; set; }
			
		/// <summary>
		/// Gets or sets the next node.
		/// </summary>
		/// <value>The next.</value>
		protected BaseNode Next { get; set; }

		/// <summary>
		/// The value of this node for order of operations.
		/// This is set in all teh chlid nodes.
		/// </summary>
		/// <value>The pembas value.</value>
		protected int PembasValue { get; set; }
			
		#endregion Members
		
		#region Methods
			
		/// <summary>
		/// Initializes a new instance of the <see cref="Equationator.EquationNode"/> class.
		/// </summary>
		/// <param name="prev">Previous node in the list, or null if this is head</param>
		/// <param name="next">Next node in the list, or null if this is tail.</param>
		public BaseNode()
		{
			this.Prev = null;
			this.Next = null;
		}
			
		/// <summary>
		/// Make this node into a head node.
		/// </summary>
		protected void MakeHead()
		{
			//note that if there is a prev node, it has to take care of it's own pointers!
			Prev = null;
		}
			
		/// <summary>
		/// Makes this node into a tail node.
		/// </summary>
		protected void MakeTail()
		{
			//note that if there is a next node, it has to take care of it's own pointers!
			Next = null;
		}
			
		/// <summary>
		/// Appends the next node.
		/// </summary>
		/// <param name="nextNode">Next node.</param>
		protected void AppendNextNode(BaseNode nextNode)
		{
			Debug.Assert(null != nextNode);
			nextNode.Prev = this;
			this.Next = nextNode;
		}

		/// <summary>
		/// Parse a list of tokens into a linked list of equation nodes.
		/// This will sort it out into a flat equation
		/// </summary>
		/// <param name="tokenList">Token list.</param>
		/// <param name="curIndex">Current index. When this function exits, will be incremented to the past any tokens consumed by this method</param>
		/// <returns>A basenode pointing at the head of a linked list parsed by this method</returns>
		static public BaseNode Parse(List<Token> tokenList, ref int curIndex)
		{
			//first get a value, which will be a number, function, param, or equation node
			BaseNode myNumNode = BaseNode.ParseValueNode(tokenList, curIndex);
			Debug.Assert(null != myNumNode);

			//if there are any tokens left, get an operator
			BaseNode myOperNode = null;
			if (curIndex < tokenList.Count)
			{
				myOperNode = BaseNode.ParseOperNode(tokenList, curIndex);
				Debug.Assert(null != myOperNode);

				//add that node to the end of the list
				myNumNode.AppendNextNode(myOperNode);
			}

			//do that again until we get to the end of the list
			if (curIndex < tokenList.Count)
			{
				//ok we have tokens left... Recurse into the parse function and sort out the rest of em
				BaseNode nextNode = BaseNode.Parse(tokenList, curIndex);
				Debug.Assert(null != nextNode);

				//add that node to the end of the list
				myOperNode.AppendNextNode(nextNode);
			}

			//return the head node that I found
			return myNumNode;
		}

		/// <summary>
		/// Given a list of tokens and the index, get a node based on whatever is at that index
		/// </summary>
		/// <returns>The value node, will be a number, function, param, or equation node</returns>
		/// <param name="tokenList">Token list.</param>
		/// <param name="curIndex">Current index.</param>
		static protected BaseNode ParseValueNode(List<Token> tokenList, ref int curIndex)
		{
			//what kind of token do I have at that index?
			switch (tokenList[curIndex].TypeOfToken)
			{
				case TokenType.Number:
				{
					//awesome, that's nice and easy... just shove the text into a node as a number

					//create the number node
					NumberNode valueNode = new NumberNode();

					//parse the text into the number node
					valueNode.Parse(tokenList, curIndex);

					//return the number node as our result
					return valueNode;
				}
				break;

				case TokenType.Param:
				{
					//also not bad, grab the text as a parameter index and put in a node

					//create the param node
					ParamNode valueNode = new ParamNode();

					//parse the parameter index into the node
					valueNode.Parse(tokenList, curIndex);

					//return it as our result
					return valueNode;
				}
				break;

				case TokenType.Function:
				{
					//hmmm... need to get the delegate and put in a node?
				}
				break;

				case TokenType.OpenParen:
				{
					//ok don't panic... 

					//move past this token, cuz nothing else to do with it

					//starting at the next token, start an equation node

					//start parsing into the equation node
				}
				break;

				case TokenType.Operator:
				{
					//whoa, how did an operator get in here?  it better be a minus sign

					//the next node had better be a number

					//create a number node, parse the next token into it

					//multiply that number by minus one
				}
				break;

				default:
				{
					//should just be close paren nodes in here, which we should never get
					throw new FormatException("Expected a \"value\" token, but got a " + tokenList[curIndex].TypeOfToken.ToString());
				}
				break;
			}
		}

		/// <summary>
		/// Given a list of tokens and the index, get an operator node based on whatever is at that index.
		/// </summary>
		/// <returns>The oper node.</returns>
		/// <param name="tokenList">Token list.</param>
		/// <param name="curIndex">Current index.</param>
		static protected BaseNode ParseOperNode(List<Token> tokenList, ref int curIndex)
		{
		}

		/// <summary>
		/// Parse the specified tokenList and curIndex.
		/// overloaded by child types to do there own specific parsing.
		/// </summary>
		/// <param name="tokenList">Token list.</param>
		/// <param name="curIndex">Current index.</param>
		protected abstract BaseNode Parse(List<Token> tokenList, ref int curIndex);
			
		#endregion Methods
	}
}

