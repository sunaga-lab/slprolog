remove( _, [], []).
remove( X, [X | L], Z ) :- remove( X, L, Z ).
remove( X, [Y | L], [Y | Z] ) :- X \== Y,remove( X, L, Z ).
