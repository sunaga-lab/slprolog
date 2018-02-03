slow_sort(L, L1) :-
  permutation(L, L1), ordered(L1).

ordered([]).
ordered([_]).
ordered([X,Y|L]) :-
  X =< Y, ordered([Y|L]).


permutation([], []).
permutation(L, [X|L2]) :-
  del(X, L, L1),
  permutation(L1, L2).

del(X, [X|L], L).
del(X, [Y|L], [Y|L1]) :-
  del(X, L, L1).


% 階乗 fact.pl
fact(0,1).
fact(X,Sum) :-
    X > 0, X1 is X - 1, fact(X1,Sum1), Sum is X * Sum1.


% 挿入 insert.pl
% insert( Index, Item, List, Result )
insert( 1, X, L, [X | L] ).
insert( N, X, [Y | L], [Y | Z] ) :-
    N > 1, N1 is N - 1, insert( N1, X, L, Z ).

% my_length.pl
my_length( [], 0 ).
my_length( [_ | Xs], N ) :- my_length( Xs, N1 ), N is N1 + 1.


% qsort.pl
qsort([], []).
qsort([X|L], S) :-
     partition(L, X, L1, L2),
     qsort(L1, S1),
     qsort(L2, S2),
     append(S1, [X|S2], S).

partition([], _, [], []).
partition([Y|L], X, [Y|L1], L2) :-
     Y < X,
     partition(L, X, L1, L2).
partition([Y|L], X, L1, [Y|L2]) :-
     Y >= X,
     partition(L, X, L1, L2).


% A < B :- N is op_lt(A, B), N = 1.
% A > B :- N is op_gt(A, B), N = 1.
% A =< B :- N is op_le(A, B), N = 1.
% A >= B :- N is op_ge(A, B), N = 1.


% Term = Term.


append([], List2, List2).
append([List1Head|List1], List2, [List1Head|ResultBody]) :-
	append(List1,List2,ResultBody).



reverse(List, ListRev) :-
	reverse(List, [], ListRev).

reverse([], List, List).
reverse([Head|Body], C, D) :-
	reverse(Body, [Head|C], D).



