using System;
using System.Text;

namespace Equationator
{
	/// <summary>
	/// This is a single text token from an equation.
	/// The first step to compiling an equation is breaking it up into a list tokens and determining what is in those tokens.
	/// </summary>
	public class Token
	{
		#region Members

		/// <summary>
		/// Gets the token text.
		/// </summary>
		/// <value>The token text.</value>
		public string TokenText { get; private set; }

		/// <summary>
		/// Gets the type of token.
		/// </summary>
		/// <value>The type of token.</value>
		public TokenType TypeOfToken { get; private set; }

		#endregion Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="Equationator.Token"/> class.
		/// </summary>
		/// <param name="myText">My text.</param>
		/// <param name="myType">My type.</param>
		public Token()
		{
			TypeOfToken = TokenType.Invalid;
		}

		/// <summary>
		/// Pull one token out of a string.
		/// </summary>
		/// <returns>int: the new current index in the string. Use this as the start point for parsing the next token</returns>
		/// <param name="strEquationText">The full text string we are tokenizing</param>
		/// <param name="iCurrentIndex">The character index in the string where to start tokenizing</param>
		public int ParseToken(string strEquationText, int iStartIndex)
		{
			//Walk through the text and try to parse it out into an expression
			while (iStartIndex < strEquationText.Length)
			{
				//First check if we are reading in a number
				if (IsNumberCharacter(strEquationText, iStartIndex))
				{
					//read a number and return the new index
					return ParseNumberToken(strEquationText, iStartIndex);
				}
				else if (strEquationText[iStartIndex] == '$')
				{
					//read a function/param and return the new index
					return ParseFunctionToken(strEquationText, iStartIndex);
				}
				else if (IsOpenParenCharacter(strEquationText, iStartIndex))
				{
					//we found an open paren!
					TypeOfToken = TokenType.OpenParen;
					TokenText = strEquationText[iStartIndex].ToString();
					return ++iStartIndex;
				}
				else if (IsCloseParenCharacter(strEquationText, iStartIndex))
				{
					//we found a close paren!
					TypeOfToken = TokenType.CloseParen;
					TokenText = strEquationText[iStartIndex].ToString();
					return ++iStartIndex;
				}
				else if (IsOperatorCharacter(strEquationText, iStartIndex))
				{
					//We found an operator value...
					TypeOfToken = TokenType.Operator;
					TokenText = strEquationText[iStartIndex].ToString();
					return ++iStartIndex;
				}

				//We found some white space
				iStartIndex++;
			}

			return iStartIndex;
		}

		/// <summary>
		/// Our token is a number, parse the full text out of the expression
		/// </summary>
		/// <returns>The index in the string after our number</returns>
		/// <param name="strEquationText">String equation text.</param>
		/// <param name="iIndex">start index of this token.</param>
		private int ParseNumberToken(string strEquationText, int iIndex)
		{
			//Set this token as a number
			TypeOfToken = TokenType.Number;

			//Parse the string into a number
			StringBuilder word = new StringBuilder();
			while (IsNumberCharacter(strEquationText, iIndex))
			{
				//Add the digit/decimal to the end of the number
				word.Append(strEquationText[iIndex++]);

				//If we have reached the end of the text, quit reading
				if (iIndex >= strEquationText.Length)
				{
					break;
				}
			}

			//grab the resulting value and return the new string index
			TokenText = word.ToString();
			return iIndex;
		}

		private int ParseFunctionToken(string strEquationText, int iIndex)
		{
			//first, skip the dollar sign
			iIndex++;

			//check if it is a param or a function call
			StringBuilder word = new StringBuilder();
			if (strEquationText[iIndex] >= '0' && strEquationText[iIndex] <= '9')
			{
				//We have a param value
				TypeOfToken = TokenType.Param;

				//Parse the param until we hit the end
				while (strEquationText[iIndex] >= '0' && strEquationText[iIndex] <= '9')
				{
					word.Append(strEquationText[iIndex++]);

					//If we have reached the end of the text, quit reading
					if (iIndex >= strEquationText.Length)
					{
						break;
					}
				}
			}
			else
			{
				//We have a function call
				TypeOfToken = TokenType.Function;

				//TODO: Parse the function call until we hit the next token

				//check if the token is stored in our grammar dictionary
				word.Append(strEquationText.Substring(iIndex, 4));
				iIndex += 4;
			}

			//grab the resulting value and return the new string index
			TokenText = word.ToString();
			return iIndex;
		}

		/// <summary>
		/// Check whether the character at an index is a number
		/// </summary>
		/// <returns><c>true</c> if this instance is number character the specified strEquationText iIndex; otherwise, <c>false</c>.</returns>
		/// <param name="strEquationText">String equation text.</param>
		/// <param name="iIndex">I index.</param>
		static private bool IsNumberCharacter(string strEquationText, int iIndex)
		{
			return (('0' <= strEquationText[iIndex] && strEquationText[iIndex] <= '9') || strEquationText[iIndex] == '.');
		}

		/// <summary>
		/// Check whether the character at an index is a open parenthesis
		/// </summary>
		/// <returns><c>true</c> if this instance is number character the specified strEquationText iIndex; otherwise, <c>false</c>.</returns>
		/// <param name="strEquationText">String equation text.</param>
		/// <param name="iIndex">I index.</param>
		static private bool IsOpenParenCharacter(string strEquationText, int iIndex)
		{
			return (strEquationText[iIndex] == '(');
		}

		/// <summary>
		/// Check whether the character at an index is a close parenthesis
		/// </summary>
		/// <returns><c>true</c> if this instance is number character the specified strEquationText iIndex; otherwise, <c>false</c>.</returns>
		/// <param name="strEquationText">String equation text.</param>
		/// <param name="iIndex">I index.</param>
		static private bool IsCloseParenCharacter(string strEquationText, int iIndex)
		{
			return (strEquationText[iIndex] == ')');
		}

		/// <summary>
		/// Check whether the character at an index is an operator character
		/// </summary>
		/// <returns><c>true</c> if this instance is number character the specified strEquationText iIndex; otherwise, <c>false</c>.</returns>
		/// <param name="strEquationText">String equation text.</param>
		/// <param name="iIndex">I index.</param>
		static private bool IsOperatorCharacter(string strEquationText, int iIndex)
		{
			return (strEquationText[iIndex] == '*' || 
				strEquationText[iIndex] == '/' || 
				strEquationText[iIndex] == '+' || 
				strEquationText[iIndex] == '-' ||
				strEquationText[iIndex] == '^' ||
				strEquationText[iIndex] == '%');
		}

		#endregion Methods
	}
}

