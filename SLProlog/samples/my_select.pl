my_select(X, [X | X1], X1).
my_select(X, [Y | Y1], [Y | Z1]) :- my_select(X, Y1, Z1).
