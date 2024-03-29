/*************************** EX-STOC.CPP ******************** AgF 2001-11-11 *
*                                                                            *
* Example showing how to use non-uniform random variate generator library.   *
* Necessary files are in stocc.zip.                                          *
*                                                                            *
* Compile for console mode.                                                  *               *
* � 2001 Agner Fog. GNU General Public License www.gnu.org/copyleft/gpl.html *
*****************************************************************************/

#include <time.h>                      // define time()
#include "randomc.h"                   // define classes for random number generators
#include "stocc.h"                     // define random library classes

#ifndef MULTIFILE_PROJECT
// If compiled as a single file then include these cpp files, 
// If compiled as a project then compile and link in these cpp files.
   #include "mersenne.cpp"             // code for random number generator
   #include "stoc1.cpp"                // random library source code
   #include "userintf.cpp"             // define system specific user interface
#endif


int main() {
   int32 seed = (int32)time(0);        // random seed
   StochasticLib1 sto(seed);           // make instance of random library
   int i;                              // loop counter
   double r;                           // random number
   int ir;                             // random integer number

   // make random numbers with uniform distribution
   printf("Random numbers with uniform distribution:\n");
   for (i=0; i<16; i++) {
      ir = sto.IRandom(0, 20);
      printf("%8i  ", ir);
   }

   // make random numbers with normal distribution
   printf("\n\nRandom numbers with normal distribution:\n");
   for (i=0; i<16; i++) {
      r = sto.Normal(10, 4);
      printf("%8.5f  ", r);
   }

   // make random numbers with poisson distribution
   printf("\n\nRandom numbers with poisson distribution:\n");
   for (i=0; i<16; i++) {
      ir = sto.Poisson(10);
      printf("%8i  ", ir);
   }

   // make random numbers with binomial distribution
   printf("\n\nRandom numbers with binomial distribution:\n");
   for (i=0; i<16; i++) {
      ir = sto.Binomial(40, 0.25);
      printf("%8i  ", ir);
   }

   // make random numbers with hypergeometric distribution
   printf("\n\nRandom numbers with hypergeometric distribution:\n");
   for (i=0; i<16; i++) {
      ir = sto.Hypergeometric(20, 20, 40);
      printf("%8i  ", ir);
   }

   EndOfProgram();                     // system-specific exit code
   return 0;
}
