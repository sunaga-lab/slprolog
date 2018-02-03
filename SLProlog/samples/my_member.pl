my_member(X, [X | L]).
my_member(X, [Y | L]) :- my_member(X, L).