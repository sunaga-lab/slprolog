slow_sort(L, L1) :-
  permutation(L, L1), ordered(L1).

ordered([]).
ordered([_]).
ordered([X,Y|L]) :-
  X =< Y, ordered([Y|L]).