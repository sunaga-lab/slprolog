smartreverse(X,Y):-smartreverse(X,[],Y).
smartreverse([],Y,Y).
smartreverse([A|B],Y,Z):-smartreverse(B,[A|Y],Z).