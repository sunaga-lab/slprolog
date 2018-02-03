permutation([], []).
permutation(L, [X|L2]) :-
  del(X, L, L1),
  permutation(L1, L2).

del(X, [X|L], L).
del(X, [Y|L], [Y|L1]) :-
  del(X, L, L1).
