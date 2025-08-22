BEGIN {
        srand();
        for(i=1; i<=36; i++){
                printf("<Key Name=\"%d\" Command=\"SET;NextQuestion,%d\"/>\n",
                i,i);
        }
}