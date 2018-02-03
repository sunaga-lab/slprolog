retrieve( 1,  [X | L], X ).
retrieve( N, [Y | L], X) :-
    N > 1, N1 is N - 1, retrieve( N1, L, X ).