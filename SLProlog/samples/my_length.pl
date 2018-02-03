my_length( [], 0 ).
my_length( [X | Xs], N ) :- my_length( Xs, N1 ), N is N1 + 1.