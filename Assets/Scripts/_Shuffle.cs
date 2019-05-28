using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public static class _Shuffle  {
    
    public static int [] Shuffle()
    {
        int n;
        int c = 0;
        bool vdd = true;
        bool vdd2 = true;
        int[] arrayRandom = new int[5];
        for(int i = 0; i<5; i++)
        {
            arrayRandom[i] = -1;
        }

        while (vdd2)
        {
            n = UnityEngine.Random.Range(0, 5);
			Debug.Log("n");
          for(int i = 0;i<5; i++)
            {
                if (n == arrayRandom[i])
                {
                    vdd = false;
                    break;
                }
            }

            if (vdd) {
                arrayRandom[c] = n;
                c++;
            }
            else
            {
                vdd = true;

            }


            if (c == 5)
            {
                vdd2 = false;
                break;
            }
                    
            
        }

        return arrayRandom;

    }




}
