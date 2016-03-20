Equationator
============

A thing for taking a math equation as a text string, parsing it, and getting a result.

-supports addition subtraction multiplication division
-supports PEMDAS order of operations: parans, multiplication, division, addition, subtraction
-can take up to nine params designated by $1 -$9
-also supports functions, where the footprint is:
	name must be four letters
	take zero parameters
	return a float

functions are registered by passing in a text string that will call the function, and a delegate that will be called when that string is encountered in the equation.
for example might register a function "rand" with a delegate that returns a random floating point number between 0.0-1.0.  This function can then be called by inserting "$rand" in an equation
also will have "$rank" which is game difficulty

solving an equation will also require a ParamDelegate callback that takes an integer between 1-9 and returns the value of the parameter at that index.



first, string is tokenized into substrings.  (already have code for this in bulletmllib)

next, start at beginning of list of tokens and insert into equationNodes, which are stored in a linked list.

token order has to be "number operator number".  An equation has to start and end with a number.  If a number is expected but an operator is encountered, that operator has to be a "-" and the number is made negative.

if an "(" is encountered, create a new equation.  Parse that equation until it reaches a ")", and return to parent parsing at that point.



Once an equation has finished putting it's tokens into a the linked list of EquationNodes, it is ready to sort them into an EquationTree.
First it goes through and picks a "root" node using the order of operations.  Everything prev and next are put in two separate equations.  Each of those equations are solved.
This is recursed until every leaf node is a number, a function call, or a param


Now that the whole equation is sorted into a binary tree, we are ready to start solving.
Equation nodes return "A EQ B"
number nodes just return the number
function nodes call the delegate and return that value
param nodes will use the delegate that was passed to the Solve() method, pass in the parameter number, and return that.

The final result returned from the single root node should be the correct value.