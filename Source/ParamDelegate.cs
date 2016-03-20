
namespace Equationator
{
	/// <summary>
	/// Param Delegate.
	/// One of these is passed into the equation at "Solve" time, to get the result of any param nodes.
	/// </summary>
	/// <param name="iParamIndex">index of the parameter to get.</param>
	/// <returns>The value of that parameter.</returns>
	public delegate double ParamDelegate(int iParamIndex);
}
