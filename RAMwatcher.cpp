#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <iomanip>
#include "zlib.h"  
#include <stdio.h>
#include <string.h>
#include <math.h> 

using namespace std;


char*  read(const char* filename){

std::ifstream in(filename, std::ifstream::ate | std::ifstream::binary);
    int n = in.tellg(); 
    in.close();
    cout<<"file size : "<<n <<endl;
    char  *c;
    ifstream infile;
    infile.open(filename, ios::binary | ios::in);
    
    c = (char*)malloc(n*sizeof(char));
    infile.read(c, n); 

    infile.close();
    return c;
}
//*/
void printBytes(char* c,int start,int nb){
for(int i=start;i<nb+start;i++){
            cout<<hex<<uppercase<<setfill('0')<<setw(2)<<(int)(unsigned char)c[i]<<" ";
    }cout<<endl<<endl;
}

int main(){

       const char* filename = "ram.txt";
       
    float n = 1048576;

	char* c = read("ram.txt");
    char* c2 = read("ram2.txt");

    int dif;
    for(int i = 0;i<n;i++){
        if(c[i]!=c2[i]){dif++;}
    }
   
   // printBytes(c,0,n);
    cout<<dif<<endl;

    free(c);
    cout <<"finnnnnnnnnnnnnnn"<<endl;



    return 0;
}
//sudo g++ -o RAMwatcher RAMwatcher.cpp && ./RAMwatcher
/*	FILE *f;
        size_t n = 1048576;
    c = (char *)malloc(n * sizeof(char));
    if((f = fopen(filename, "rwb")) == NULL)
		{
			printf("Unable to open file!\n");
			exit(1);
		}else printf("Opened file successfully.\n");
    if(fread(c, n * sizeof( char ), 1, f) != 1)
		{
			printf( "Read error!\n" );
			exit( 1 );
		}else printf( "Read was successful.\n" );
		fclose(f);
*/