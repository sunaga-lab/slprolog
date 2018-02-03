take_integer([X | X1], Y1) :-
    take_integer(X, Y2), take_integer(X1, Y3), append(Y2, Y3, Y1).
take_integer(X, [X]) :- integer(X).
take_integer(X, []).
