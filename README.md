# GScript

GScript is a programming language which has simple and unified grammer.

### Examples

#### Hello world

	print 1 + 2;

#### Variable

	define integer a: 0;
	let a = 1;
	print a + 2;

#### If statement

	define integer a: 3;
	define integer b: 4;
	if (a > b)
	{
		print a;
	}
	else
	{
		print b;
	}

#### While statement

	def int sum: 0;
	def int i: 1;
	while (i <= 5)
	{
		let sum = sum + i;
		let i = i + 1;
	}
	print sum;

#### Function

	define (function(int) -> int) fib: function(int n) -> int
	{
		if (n == 0 or n == 1)
		{
			return n;
		}
		else if (n == 2)
		{
			return 1;
		}
		
		return fib(n - 1) + fib(n - 2);
	};

	return fib(10);
