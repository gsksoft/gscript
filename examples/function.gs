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