#include <iostream>
#include <stdlib.h>
#include <fstream>
#include <iomanip>
#include "zlib.h"  
#include <stdio.h>
#include <string.h>
#include <math.h> 
#include <filesystem>

using namespace std;

void printBytes(char* c,int start,int nb){
for(int i=start;i<nb+start;i++){
            cout<<hex<<uppercase<<setfill('0')<<setw(2)<<(int)(unsigned char)c[i]<<" ";
    }cout<<endl<<endl;
}
uint32_t* toUint32(char* c,int start,int nb){
uint32_t* u = (uint32_t*)malloc(nb*sizeof(uint32_t));
int j=0;
for(int i=start;i<nb*4+start;i+=4){
           u[j]=0;
u[j] = (u[j] << 8) + (unsigned char)c[i+3];
u[j] = (u[j] << 8) + (unsigned char)c[i+2];
u[j] = (u[j] << 8) + (unsigned char)c[i+1];
u[j] = (u[j] << 8) + (unsigned char)c[i+0];
j++;
    }cout<<endl<<endl;
    return u;
}
float* toFloat32(char *c,int start,int nb){
    float *f= (float* )malloc(nb*sizeof(float));
    int j=0;
    start*=4;
    for(int i=start;i<4*nb+start;i+=4){
        unsigned char b[4] = {(unsigned char)c[i], (unsigned char)c[i+1],(unsigned char)c[i+2], (unsigned char)c[i+3]};
        memcpy(&f[j], &b, sizeof(f[0]));
        j++;
    }
return f;
}
int32_t* toInt32(char *c,int bstart,int nb){
    int32_t *ints= (int32_t* )malloc(nb*sizeof(int32_t));
    int j=0;
    //start*=4;
    for(int i=bstart;i<4*nb+bstart;i+=4){
        unsigned char b[4] = {(unsigned char)c[i], (unsigned char)c[i+1],(unsigned char)c[i+2], (unsigned char)c[i+3]};
        memcpy(&ints[j], &b, sizeof(ints[0]));
        j++;
    }
return ints;
}
int16_t* toInt16(char *c,int start,int nb){
    int16_t *r= (int16_t* )malloc(nb*sizeof(int16_t));
    int j=0;
    start*=2;
    for(int i=start;i<2*nb+start;i+=2){
        unsigned char b[2] = {(unsigned char)c[i], (unsigned char)c[i+1]};
        memcpy(&r[j], &b, sizeof(r[0]));
        j++;
    }
return r;
}
int8_t* toInt8(char *c,int start,int nb){
    int8_t *r= (int8_t* )malloc(nb*sizeof(int8_t));
    int j=0;
    for(int i=start;i<nb+start;i++){
        unsigned char b = (unsigned char)c[i];
        memcpy(&r[j], &b, sizeof(int8_t));
        j++;
    }
return r;
}
int findChunkStart(char* c,int n){
        for(int i=0;i<n;i++){
            //cout<<toInt32(c,i,1)[0]<<endl;
            if(toInt32(c,i,1)[0]==0x0303F005){
                return i;
                
            }
        }
        return -1;
}
void WriteRotations(char* ucompGohst,int ucompSize,const char OutFilename[]){
 int len2 = (ucompSize /2 );
    int16_t* ints = toInt16(ucompGohst,6*2+3*2,len2);//*/
    
    ofstream Out;
    Out.open(OutFilename);
    int j=0; int dif=0;
    for(int i=0;i<len2;i++){
          int d = 61*2;
         
           /* if(j==11)dif=18;
            if(j==17)dif=61-6;
            if(j==23)dif=61-6;
            if(j==35)dif=11;
            if(j==38)dif=11;
            if(j==49)dif=11+7;
            if(j==51)dif=18+1;
            if(j== 122)dif= 19;*/
          
          if(i%(d)==dif*2){
           //   if(j!=11 && j!=17 && j!=23 && j!=35 && j!=38 && j!=38 && j!=49 && j!=51 && j!=122){
            uint16_t angle = (uint16_t)ints[i]; 
            int16_t axisHeading =  ints[i+1];
            int16_t axisPitch = ints[i+2];
            float a = (((float)angle)/65535.00)*M_PI;
            float b = (((float)axisHeading)/32768)*M_PI;
            float c = (((float)axisPitch)/32768)*M_PI_2;
            Out<<a<<"  "<<b<<"  "<<c<<endl; 
                       //  }
              j++;
          }
          
    }
    Out.close();//*/ 
    free(ints);
    }
    void printInt16(char* ucompGohst,int ucompSize,const char OutFilename[]){
         int len2 = (ucompSize /2 );
    int16_t* ints = toInt16(ucompGohst,6*2+3*2,len2);//*/
        ofstream Out;
    Out.open(OutFilename);
    int j=0; int dif=0;
    for(int i=0;i<len2;i++){
          int d = 61*2;
       
            
            /*if(j==11)dif=18;
            if(j==17)dif=61-6;
            if(j==23)dif=61-6;
            if(j==35)dif=11;
            if(j==38)dif=11;
            if(j==49)dif=11+7;
            if(j==51)dif=18+1;
            if(j== 122)dif= 19;//*/

          Out<< ints[i];
          if(i%(d)==dif*2){
           /* uint16_t angle = (uint16_t)ints[i]; 
            int16_t axisHeading =  ints[i+1];
            int16_t axisPitch = ints[i+2];
            float a = (((float)angle)/65535.00)*M_PI;
            float b = (((float)axisHeading)/32768)*M_PI;
            float c = (((float)axisPitch)/32768)*M_PI_2;
            cout<<a<<"   "<<b<<"  "<<c
            <<endl;//*/



              Out<<"    <--"<<j;
              j++;
          }
          
          Out<<endl;
    }
    Out.close();
    free(ints);
    }
void printFloat32(char* ucompGohst,int ucompSize,const char OutFilename[]){
    
     int len = (ucompSize/4 );
    float* f = toFloat32(ucompGohst,6,len);
    
    ofstream Out;
    Out.open(OutFilename);
    int j=0,k=0; int dif=0;
    for(int i=0;i<len;i++){
          int d = 61;
       
            
          Out<< f[i];
          if(i%(d)==dif ){
              Out<<"    <--"<<j;
              j++;
          }
          
          Out<<endl;
    }
    Out.close();//*/
    free(f);
    
}
bool notEqual(int n, int size,int* arr){
    for(int i=0;i<size;i++){
        if(arr[i]==n+1){
            return false;
        }
    }
    return true;
}
float* getPosX(char* ucompGohst,int ucompSize){
    int len = (ucompSize/4 );
    float* f = toFloat32(ucompGohst,6,len);
    int n=0;
    float* posx = (float*)malloc(sizeof(float)*len);
    int blist[] = { 13, 15, 17, 19, 21, 23, 25, 27, 29, 31,33}; int size = 11;
    int blist2[] = { 560, 559, 33, 896,724,795}; int size2=6;
        for(int i=0;i<len;i++){
            if(f[i]<990 && f[i]>561 && n<=32){//&& (n==0 ? f[0] : posx[n-1])-f[i]<100 // && notEqual((int)f[i]-1,size2,blist2) 
                if(notEqual(n,size,blist)  ){//
                posx[n+1]=f[i];
              cout<<posx[n+1]<<"------------------------------"<<n+1<<endl;
                }
                n++;
          }
        }
    free(f);
    posx[0]=n;
    return (float*)realloc(posx,sizeof(float)*(n+1));
}
void WritePos(char* ucompGohst,int ucompSize,const char OutFilename[]){
    
     int len = (ucompSize/4 );
    float* f = toFloat32(ucompGohst,6,len);
    
    ofstream Out;
    Out.open(OutFilename);
    int j=0; int dif=0;
    for(int i=0;i<len;i++){
          int d = 61;
       
           if(j==9)dif=61-5;
            if(j==13)dif=8;
            if(j==21)dif=4;
            if(j==23)dif=61+4-9;
            if(j==26)dif=7;
            
          if(i%(d)==dif){
              if(j!=12 && j!=21 && j!=23 && j!=26){
              Out<<"v "<<f[i]<<"  "<<f[i+1]<<"  "<<f[i+2]<<endl;
              }
              j++;
          }
    }
    Out.close();
    free(f);
    
}//*/
void WritePos(float* posx,const char OutFilename[]){
        
    ofstream Out;
    Out.open(OutFilename);
    int size = posx[0];
    for(int i=1;i<size+1;i++){
       Out<<"v "<<posx[i]<<" 33.3642  560"<<endl;
       
    }
    Out.close();
    
}
int main()
  {
      char filename[] = "THE_ACC_TRACK/THE_TRACK_ACC_00.Replay.Gbx";
      std::ifstream in(filename, std::ifstream::ate | std::ifstream::binary);
    int n = in.tellg(); 
    in.close();
    cout<<"file size : "<<n <<endl;
    char  *c;
    ifstream infile;
    infile.open(filename, ios::binary | ios::in);
    
  //   int n = 11313;//94895;
    c = (char*)malloc(n*sizeof(char));
    infile.read(c, n); 

    infile.close();
    int chunkStart = findChunkStart(c,n);//5343;//11396;
        cout<<"chunkStart : "<<chunkStart<<endl;
    int CompressedGostStart = chunkStart + 12;
    unsigned int CgohstSize = (unsigned int)toInt32(c,chunkStart+8,1)[0];//5496;//21806;
    cout<<"compressed gohst size : "<<CgohstSize<<endl;
    unsigned int UGohstSize = (unsigned int)toInt32(c,chunkStart+4,1)[0];//10650;//28889;
        cout<<"uncompressed gohst size : "<<UGohstSize<<endl;
    char* compressedGohst = (char*)malloc(sizeof(char)*CgohstSize);
    for(int i=0;i<CgohstSize;i++){
        compressedGohst[i] = c[CompressedGostStart+i];
    }
    //printBytes(compressedGohst,0,CompressedGostStart);
    uLong ucompSize = UGohstSize+1; // "Hello, world!" + NULL delimiter.
    uLong compSize = CgohstSize+1;//compressBound(ucompSize);
    char ucompGohst[ucompSize-1];
    uncompress((Bytef *)ucompGohst, &ucompSize, (Bytef *)compressedGohst, compSize);
    //printBytes(ucompGohst,0,ucompSize);



    //printInt16(ucompGohst,ucompSize,"ints16ACC.txt");
    //WriteRotations(ucompGohst,ucompSize,"rotationACC.txt");

    printFloat32(ucompGohst,ucompSize,"THE_ACC_TRACK_Floats32/0.txt");
   // WritePos(ucompGohst,ucompSize,"THE_ACC_TRACK_POS/6.txt");
    float* posx = getPosX(ucompGohst,ucompSize);
    WritePos(posx,"THE_ACC_TRACK_POS/00.txt");


     /* ofstream Out;
    Out.open("uncompressedReplayACC",ios::binary | ios::out);
    Out.write(ucompGohst,ucompSize);
    Out.close();//*/

    free(posx);
    free(c);
    free(compressedGohst);

  }//11396  21806   28853   33214
//4634  
        /*if(j==11)dif=18;
            if(j==17)dif=61-6;
            if(j==23)dif=61-6;
            if(j==35)dif=11;
            if(j==38)dif=11;
            if(j==49)dif=11+7;
            if(j==51)dif=18+1;
            if(j== 122)dif= 19;
            */
//6
        //if(j==/**/9)dif=61-5;
        //    if(j==/*--*/13)dif=8;
         //   if(j==21)dif=4;
          //  if(j==23)dif=61+4-9;
          //  if(j==26)dif=7;//*/

           //run command:   g++ -o exec Main.cpp -lz && ./exec