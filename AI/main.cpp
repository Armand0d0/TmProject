#include <iostream>
#include <X11/Xlib.h>
#include <X11/Xutil.h>
#include <assert.h>
#include <unistd.h>
#define Width 1920
#define Height 1080
#define ITER 500
#define Interval 50000
#define NIL (0)
using namespace std;

bool IsWhite(Display *d,XImage *img, int x, int y){
XColor ret;
ret.pixel = XGetPixel(img,x,y);
XQueryColor (d, XDefaultColormap(d, XDefaultScreen(d)), &ret);
return ((ret.red/256) >=250 && (ret.green/256)>=250 && (ret.blue/256)>=250 );
}
void wait(int t){
  clock_t t0 = clock();
  while (clock()-t0 < t){}
  return;
}

int main(int, char**){
   

    Display *d = XOpenDisplay((char *) NULL);
    assert(d);
    int sc = XDefaultScreen(d);
    XImage *img;

   // for (size_t i = 0; i < ITER; i++){
    img = XGetImage (d, XRootWindow (d, sc), 0, 0, Width, Height, AllPlanes, XYPixmap);

    printf("%d %d %d %ld %d",img->bitmap_bit_order,img->bitmap_pad,img->bits_per_pixel,img->blue_mask,img->format);

    XFree (img);
       
     wait(Interval);
    //}//*/
   
    return 0;
}//g++ -o ScreenWatcher ScreenWatcher.cpp -lX11