
namespace Equationator
{
	/// <summary>
	/// Enum used to sort out the equation tree by order of operations.
	/// The lower the number, the leafier the node.
	/// </summary>
	public enum PemdasValue
	{
		Value, //numbers, equations, function, params, tier, are always leaf nodes
		Exponent,
		Multiplication,
		Division,
		Modulo,
		Addition,
		Subtraction,
		Invalid
	}
}
