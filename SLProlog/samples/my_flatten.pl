my_flatten([X | X1], Y1) :-
    my_flatten(X, Y2), my_flatten(X1, Y3), append(Y2, Y3, Y1).
my_flatten(X, [X]) :- atomic(X), X \== [].
my_flatten([], []).
