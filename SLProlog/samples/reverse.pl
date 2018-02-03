reverse([], []).
    reverse([X|L], R) :-
        reverse(L, R1),
        append(R1, [X], R).