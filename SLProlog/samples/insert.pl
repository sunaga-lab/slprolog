insert( 1, X, L, [X | L] ).
insert( N, X, [Y | L], [Y | Z] ) :-
    N > 1, N1 is N - 1, insert( N1, X, L, Z ).