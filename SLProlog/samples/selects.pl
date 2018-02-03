selects([], Y1).
selects([X | X1], Y1) :- select(X, Y1, Y2), selects(X1, Y2).
