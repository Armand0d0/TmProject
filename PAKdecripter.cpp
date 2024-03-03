#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <iomanip>
#include "zlib.h"  
#include <stdio.h>
#include <string.h>
#include <math.h> 

#include <sys/types.h>
#include <sys/stat.h>
#include <sys/mman.h>
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <openssl/md5.h>
#include <openssl/evp.h>
#include <openssl/blowfish.h>
#include <iomanip>
#include <stdexcept>
#include <vector>

using namespace std;


char*  read(const char* filename,int* size){

std::ifstream in(filename, std::ifstream::ate | std::ifstream::binary);
    int n = in.tellg(); 
    in.close();
    *size = n;
    cout<<"file size : "<<n <<endl;
    char  *c;
    ifstream infile;
    infile.open(filename, ios::binary | ios::in);
    
    c = (char*)malloc(n*sizeof(char));
    infile.read(c, n); 

    infile.close();
    return c;
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
//*/
void printBytes(char* c,int start,int nb){
for(int i=start;i<nb+start;i++){
            cout<<hex<<uppercase<<setfill('0')<<setw(2)<<(int)(unsigned char)c[i]<<" ";
    }cout<<dec<<endl<<endl;
}
void printString(unsigned char* c,int start,int nb){
for(int i=start;i<nb+start;i++){
            cout<<c[i];
    }cout<<dec<<endl<<endl;
}

void print_md5_sum(unsigned char* md) {
    int i;
    printf("nameKey : ");
    for(i=0; i <MD5_DIGEST_LENGTH; i++) {
            printf("%02x",md[i]);
    }
    cout<<endl;
}
unsigned char* md5(const char* in,int n){
    unsigned char* r= (unsigned char*)malloc(MD5_DIGEST_LENGTH*sizeof(unsigned char));
     MD5((const unsigned char*)in,n,r); //sizeof(in)*2
     return r;
}
/*void blowfishDecrypt(EVP_CIPHER_CTX *ctx,unsigned char* in, unsigned char iv[8],unsigned char** out,int* outl){

  
 
  *outl = len1+len2; 
      *out = buf;  
        //cout<<&out<<"   --   "<<&buf<<endl;

            printBytes((char*)*out,0,*outl);

}*/
int main(){
int n1;
  char* c = read("packlist2.dat",&n1);
  printBytes(c,6,4);
    uint32_t saltInt = (unsigned int)toInt32(c,6,1)[0];//lil: 374512307  big: 3013235222
    saltInt = 374512307;
    //cout<<(int)c[6];
    cout<<dec;
 
  
   
    //char lll[20];
    unsigned char* nameKey = md5("6611992868945B0B59536FC3226F3FD0374512307" /*+ sprintf(lll,"%u", saltInt)*/, 41);//c[6]+c[7]+c[8]+c[9]
            print_md5_sum(nameKey);
    char nameLength = c[11]; cout<<"nameLength = "<<(int)nameLength<<endl;
    char name[nameLength];
    char encryptedName[nameLength];
    for(int i=0;i<nameLength;i++){
        encryptedName[i]=c[12+i];
    }
     cout<< "EncName:"; printBytes(encryptedName,0,nameLength);
    for (int i = 0; i < nameLength; i++)
        name[i] = encryptedName[i] ^ nameKey[i % 16];

        name[nameLength]  = 0;
     cout<<"name : "<<name<<" => ";     printBytes(name,0,nameLength); 


     unsigned char* keyStringKey = md5("resource374512307B97C1205648A66E04F86A1B5D5AF9862",49);         
     unsigned char keyString[0x20];
     char encryptedKeyString[0x20];
     for(int i=0;i<0x20;i++){
        encryptedKeyString[i]=c[20+i]; 
    }
   // printBytes(encryptedKeyString,0,0x20);
    for (int i = 0; i < 0x20; i++)
     keyString[i] = encryptedKeyString[i] ^ keyStringKey[i % 16];
    int n =40;
    unsigned char AA[n];
    unsigned char bb[] = "NadeoPak";
    for(int i=0;i<0x20;i++){
        AA[i] = keyString[i];
    }
    for(int i=0x20;i<n;i++){
        AA[i] = bb[i];
    }
   const unsigned char *key = md5((const char*)AA ,n);
    cout<<"key : "; printBytes((char*)key,0,MD5_DIGEST_LENGTH);
    
    int size=0;
    unsigned char* file = (unsigned char*)(read("Resource.pak",&size));
    
    unsigned char* in = file+20+256; //printBytes((char*)in,0,10);//encrypted
    size = 256;
    cout<<"in : ";printBytes((char*)in,0,8);
  //const unsigned char key[] = "123457";   // 6
  const unsigned char  iv[8] = {file[12],file[13],file[14],file[15],file[16],file[17],file[18],file[19]}; // 8
  cout<<"iv : ";printBytes((char*)iv,0,8);//headervec
 // const unsigned char in[] = "input";   //5
   // const unsigned char in[] = {0x66, 0xDA, 0x61, 0x32, 0x71, 0x0C, 0xBE, 0x94};
  EVP_CIPHER_CTX *ctx;
    ctx = EVP_CIPHER_CTX_new();
    EVP_CIPHER_CTX_init(ctx); 
    int rc = 0;
//    rc += EVP_DecryptInit_ex(ctx, NULL, NULL, 0, 0);
  rc = EVP_DecryptInit_ex(ctx, EVP_bf_cbc(), NULL, 0, 0);
  rc = EVP_CIPHER_CTX_set_key_length(ctx, strlen((char*)key));
  rc = EVP_DecryptInit_ex(ctx, NULL, NULL, key, iv);
  unsigned char* buf; 
  int len1 , len2;
  rc = EVP_DecryptUpdate(ctx, buf, &len1, in, size); //strlen((char*)in)  
  if(rc != 1){throw runtime_error("Decrypt update fail !!");}
  rc = EVP_DecryptFinal_ex(ctx, buf+len1, &len2);
  int len = len1 + len2;
  cout<<"length : "<<len<<"   ("<< len1<<" + " <<len2<<")"<<endl;
  //if (rc != 6)
    //throw runtime_error("encryption failed");

      printString(buf,0,len);
      cout<<"<---=--->"<<endl;
      printBytes((char*)buf,0,56);

 int cSize = 0x100;
        int uSize = 0x200;
       uLong ucompSize = uSize; // "Hello, world!" + NULL delimiter.
    uLong compSize = cSize;//compressBound(ucompSize);
    char ucompBuf[ucompSize-1];

    for(int i=0;i<30;i++){
       ucompSize++;
       uSize++;
      uncompress((Bytef *)ucompBuf, &ucompSize, (Bytef *)buf, compSize);
            cout<<"uncompSize : "<<ucompSize<<endl;
           ucompSize = uSize;
      cout<<"uncopressed : ";
      printString((unsigned char*)ucompBuf,0,ucompSize);
      cout<<"<---=--->"<<endl;
      printBytes(ucompBuf,0,ucompSize);
      cout<<"i="<<i<<endl;
    }
        //ofstream wf;
       //wf.open("./aa.txt", ios::out | ios::binary);
       //  wf.write((char *) buf, 0);
      // wf.close();


  //EVP_CIPHER_CTX_cleanup(ctx);
   // free(in);
    free(keyStringKey);
    free(nameKey);
  free(c);/**/
    return 0;
}
/*
g++ -o PAKdecripter PAKdecripter.cpp -lcrypto -lz && ./PAKdecripter
*/


/*
    const unsigned char* ch = key;
   
   int l = (int) strlen((const char*)ch);
   
   BF_KEY Key;
   //generate Blowfish key
   BF_set_key(&Key, l, ch);
    unsigned char ivec;
    unsigned char* in = (unsigned char*)"123456789";
    unsigned char* out;
   long length = (long) strlen((const char*)in);
   BF_KEY *schedule = &Key;

          BF_cbc_encrypt(in, out, length, schedule, &ivec,  BF_DECRYPT);

    cout << "out : " <<out<<endl;
  */