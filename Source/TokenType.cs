using System;

namespace Equationator
{
	/// <summary>
	/// All the different types of values that might be stored in a Token object
	/// </summary>
	public enum TokenType
	{
		Number,
		Operator,
		OpenParen,
		CloseParen,
		Function,
		Param
	}
}

