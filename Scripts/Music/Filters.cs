using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filters : MonoBehaviour
{
   [Header("Filters")]
   public bool highpass = false;
   public bool lowpass = false;
   public float alpha = .99f;
   
   private float prevInput = 0f;
   private float prevOutput = 0f;
   
   private void OnAudioFilterRead(float[] data, int channels)
   {
     
            
      for (int i = 0; i < data.Length; i += channels)
      {
         var input = data[i];
                
         // Apply filters
         float output;
                
         if (highpass) output = input - prevInput;
         else if (lowpass)
         {
            output = (alpha * prevOutput) + (1 - alpha) * input;
         }
         else output = input;

         prevOutput = output;
         prevInput = input;

         data[i] = output;
         data[i + 1] = data[i];
      }
   }
}
