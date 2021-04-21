using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class CPT_func : MonoBehaviour
{

    Text txt;
    int lettersSize;

    Dictionary<string,int> letters1;
    List<float> ISI = new List<float> { 1f, 2f, 4f };
    List<float> shuffledISI;

    float elapsed = 0f;

    //int a = 0;
    //int b = 0;
    //int c = 0;
    //int x = 0;
    int reps = 1;

    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        letters1 = new Dictionary<string, int>
        {
            {"A",30},
            {"B",30},
            {"X",10},
        };
        lettersSize = letters1.Count;
        var rng = new System.Random();
        shuffledISI = ISI.OrderBy(a => Guid.NewGuid()).ToList();
    }

    // Update is called once per frame
    void Update()
    {

        if (reps < lettersSize)
        {
            Connors(shuffledISI[0]);
        }

        if (reps >= lettersSize && reps < (lettersSize+lettersSize))
        {
            Connors(shuffledISI[1]);
        }

        if (reps >= (lettersSize + lettersSize))
        {
            Connors(shuffledISI[2]);
        }
        if (reps >= (lettersSize + lettersSize + lettersSize))
        {
            var rng = new System.Random();
            shuffledISI = ISI.OrderBy(a => Guid.NewGuid()).ToList();
            reps = 1;
            Debug.Log("NEW ROUND");
        }

        //if (reps >= 100)
        //{
        //    Debug.Log("DONE_____");
        //    Debug.Log("A: " + a);
        //    Debug.Log("B: " + b);
        //    Debug.Log("C: " + c);
        //    Debug.Log("X: " + x);
        //    Debug.Log("Reps: " + reps);
        //}
    }
     
    void Connors(float interval)
    {
        string letter = weightedRandom();
        elapsed += Time.deltaTime;
        if (elapsed >= 0.25f)
        {
            txt.text = "";
        }
        if (elapsed >= interval)
        {
            txt.text = letter;
            elapsed = elapsed % 1f;
            reps++;
        }
    }

    string weightedRandom()
    {
        var rng = new System.Random();
        var pick = rng.Next(letters1.Values.Sum());
        int sum = 0;
        string letterString = "";
        foreach (var letter in letters1)
        {
            sum += letter.Value;
            if (sum >= pick)
                if (letter.Key != txt.text) {
                    {
                        letterString = letter.Key;
                        break;
                    }
            }
        }

        //if (letterString == "A")
        //{
        //    a++;
        //}
        //if (letterString == "B")
        //{
        //    b++;
        //}
        //if (letterString == "C")
        //{
        //    c++;
        //}
        //if (letterString == "X")
        //{
        //    x++;
        //}

        return letterString;
    }


}
