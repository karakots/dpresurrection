using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlySim
{
    public class Bucket
    {
        private static Random rand = new Random();

        private List<int> draws;

        int max;

        public Bucket(int size)
        {
            draws = new List<int>();

            for (int i = 0; i < size; i++)
            {
                draws.Add(i);
            }

            max = size;
        }

        public int Draw
        {
            get
            {
                int draw = rand.Next(max);

                max--;

                if (draw == max)
                {
                    return draws[draw];
                }

                int ret = draws[draw];

                draws[draw] = draws[max];

                return ret;
            }
        }

        public bool Empty
        {
            get
            {
                if (max <= 0)
                {
                    return true;
                }

                return false;
            }
        }

        public void Reset(int size)
        {
            draws = new List<int>();

            for (int i = 0; i < size; i++)
            {
                draws.Add(i);
            }
        }


    }
}
