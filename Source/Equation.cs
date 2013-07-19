using System.Collections.Generic;
using System;
using System.Text;
using System.Diagnostics;

namespace Equationator
{
	/// <summary>
	/// This is the main class of teh Equationator. 
	/// It contains one entire equation.  
	/// Usage is to create one of these, add all the delegate methods, parse the equation string, and then call teh Solve function whenever need a result from it.
	/// A good idea would be to subclass this and have the child class set up all the delegate methods in it's constructor.
	/// </summary>
	public class Equation
	{
		#region Members

		/// <summary>
		/// The text equation that this dude parsed.
		/// </summary>
		public string TextEquation { get; private set; }

		/// <summary>
		/// This is the root node of the equation.  This is set in the Parse method.
		/// </summary>
		private BaseNode RootNode { get; set; }

		/// <summary>
		/// A list of all the names and functions that can be used in teh equation grammar of this dude.
		/// </summary>
		public Dictionary<string, FunctionDelegate> FunctionDictionary { get; private set; }

		#endregion Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="Equationator.Equation"/> class.
		/// </summary>
		public Equation()
		{
			FunctionDictionary = new Dictionary<string, FunctionDelegate>();
		}

		/// <summary>
		/// Adds a function to the grammar dictionaary so that it can be used in equations.
		/// </summary>
		/// <param name="FunctionText">Function text. Must be 4 characters, no numerals</param>
		/// <param name="callbackMethod">Callback method that will be called when $XXXX is encountered in an equation</param>
		/// <exception cref="FormatException">thrown when the fucntionText is incorrect format</exception>
		public void AddFunction(string functionText, FunctionDelegate callbackMethod)
		{
			if (4 != functionText.Length)
			{
				//string length of a function text must be 4 cahracters, no numbers
				//This makes it easy to parse when we find $xxxx (that means it's a function)
				throw new FormatException("The functionText parameter must be exactly four characters in length.");
			}

			//Store the thing in the dictionary
			FunctionDictionary.Add(functionText, callbackMethod);
		}

		/// <summary>
		/// Parse the specified equationText.
		/// </summary>
		/// <param name="equationText">Equation text.</param>
		public void Parse(string equationText)
		{
			//grab the equation text
			TextEquation = equationText;

			//straight up tokenize the equation: operators, numbers, parens, functions, params
			List<Token> tokenList = Tokenize(equationText);

			//check if an empty equation was passed into the equationator
			if (0 == tokenList.Count)
			{
				RootNode = null;
			}
			else
			{
				//sort out those tokens into a linked list of equation nodes
				int index = 0;
				BaseNode listRootNode = BaseNode.Parse(tokenList, ref index, this);

				//take that linked list and bend it into a binary tree.  Grab the root node
				RootNode = listRootNode.Treeify();
			}
		}

		/// <summary>
		/// Get a result from the equation
		/// </summary>
		/// <param name="paramCallback">This is a callback function to get the value of params to pass to this equation</param>
		public float Solve(ParamDelegate paramCallback)
		{
			if (null != RootNode)
			{
				return RootNode.Solve(paramCallback);
			}
			else
			{
				//There is no equation parsed into this equationator
				return 0.0f;
			}
		}

		/// <summary>
		/// Tokenize the specified equationText.
		/// </summary>
		/// <param name="equationText">Equation text.</param>
		/// <returns>A list of tokens that were contained in the equation text.</returns>
		private List<Token> Tokenize(string equationText)
		{
			//The list that will hold all our tokens.
			List<Token> tokenList = new List<Token>();

			//Start at the beginning and parse tokens until we hit the end of the string
			int iCurrentIndex = 0;
			while (iCurrentIndex < equationText.Length)
			{
				//create a new token
				Token nextToken = new Token();

				//parse the token and update the current index
				iCurrentIndex = nextToken.ParseToken(equationText, iCurrentIndex);

				//Check the result of the token
				if (TokenType.Function == nextToken.TypeOfToken)
				{
					//function tokens require a bit more validation...
					if (FunctionDictionary.ContainsKey(nextToken.TokenText))
					{
						//We found a matching function call in the dictionary
						tokenList.Add(nextToken);
					}
					else
					{
						//error: there was a $something that wasn't a param and wasn't in our function dictionary
						throw new FormatException(string.Format("Equation text contained ${0} that was not found in the grammar dictionary", nextToken.TokenText));
					}
				}
				else if (TokenType.Invalid != nextToken.TypeOfToken)
				{
					tokenList.Add(nextToken);
				}
			}

			//ok, this should contain our whole entire token list
			return tokenList;
		}

		#endregion Methods
	}
}
