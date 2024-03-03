#include <iostream>
#include <stdlib.h>
#include <fstream>
using namespace std;


float* Read(const char filename[],int size){
    ifstream infile;
infile.open(filename,ifstream::in);
string s;
float* f = (float*)malloc(sizeof(float)*size);
        for (int i = 0; i < size; i++)
        {
            getline(infile, s);
           f[i] = stof(s); 
        }
    infile.close();
    return f;
}
float* sort(float f[5]){
    int n =5;
        float *res =(float*)malloc(sizeof(float)*n);
        int min = 0;
        for(int z=0;z<n;z++){
    loop:
    min=res[z];

    for(int i=z;i<n;i++){
        if(min<=res[i]){}
        else{
             min=res[i];
             res[i]=res[z];
             res[z]=min;
             goto loop;         
         }
   }
   }
        return res;
}
/*for (int i = 0; i < 5; i++){
            if(f[i]>n[0]){
                n[0]=f[i];
            }
            
        }
        for (int i = 0; i < 5; i++){
            if(f[i]>n[1] && f[i]!=n[0]){
                n[1]=f[i];
            }
        }
        for (int i = 0; i < 5; i++){
            if(f[i]>n[2] && f[i]!=n[0] && f[i]!=n[1]){
                n[2]=f[i];
            }
        }
        for (int i = 0; i < 5; i++){
            if(f[i]>n[3] && f[i]!=n[0]  && f[i]!=n[1] && f[i]!=n[2]){
                n[3]=f[i];
            }
        }
        for (int i = 0; i < 5; i++){
            if(f[i]>n[4] && f[i]!=n[0]  && f[i]!=n[1] && f[i]!=n[2]&& f[i]!=n[3]){
                n[4]=f[i];
            }
        }*/
int main(){

int l3 = 22;
int l4 = 8;
int l4_1=28;
int l5 = 30;
int l6 = 24;
int lmax = l5;
int lmin = l4;
float* p3= Read("3pos.txt",l3);
float* p4= Read("4pos.txt",l4);
float* p4_1= Read("4.1pos.txt",l4_1);
float* p5= Read("5pos.txt",l5);
float* p6= Read("6pos.txt",l6);
float res[5*lmax];


    

    
    for(int i=0;i<5*lmin;i+=5){
        float f[] = {p3[i],p4[i],p4_1[i],p5[i],p6[i]};
        float* f2 = sort(f);
        res[i] = f2[0];
        res[i+1] = f2[1];
        res[i+2] = f2[2];
        res[i+3] = f2[3];
        res[i+4] = f2[4];     
        
        free(f2);
    }
    ofstream Out;
    Out.open("orderedPos.txt");
for(int i=0;i<lmax;i++){
    if(i<l3){
        Out<<p3[i]<<"-----1--"<<i<<endl;
    }else{Out<<endl;}

    if(i<l4){
        Out<<p4[i]<<"-----2--"<<i<<endl;
    }else{Out<<endl;}

    if(i<l4_1){
        Out<<p4_1[i]<<"-----3--"<<i<<endl;
    }else{Out<<endl;}

    if(i<l5){
        Out<<p5[i]<<"-----4--"<<i<<endl;
    }else{Out<<endl;}

    if(i<l6){
        Out<<p6[i]<<"-----5--"<<i<<endl;
    }else{Out<<endl;}
    Out<<endl;
}
    Out.close();
    float g[]={4,5,8,1,3};
    float *h = sort(g);
                for(int i=0;i<l4;i++){
              //  cout<<h[i]<<endl;
                }
    free(h);

    free(p3);
    free(p4);
    free(p4_1);
    free(p5);
    free(p6);

}