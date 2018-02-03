qsort([], []).
qsort([X|L], S) :-
     partition(L, X, L1, L2),
     qsort(L1, S1),
     qsort(L2, S2),
     append(S1, [X|S2], S).

partition([], _, [], []).
partition([Y|L], X, [Y|L1], L2) :-
     Y @< X,
     partition(L, X, L1, L2).
partition([Y|L], X, L1, [Y|L2]) :-
     Y @>= X,
     partition(L, X, L1, L2).
