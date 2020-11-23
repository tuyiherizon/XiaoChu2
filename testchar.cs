using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testchar : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //int inTimes = TestDoorQ(100000);
        //Debug.Log("inTimes:" + inTimes);
        //int inTimes2 = TestDoorQ2(100000);
        //Debug.Log("inTimes2:" + inTimes2);
        int star = 0;
        SetStar(0, ref star);
        Debug.Log("test star:" + star + "," + GetStar(0, ref star));
        SetStar(1, ref star);
        Debug.Log("test star:" + star + "," + GetStar(1, ref star));
        SetStar(2, ref star);
        Debug.Log("test star:" + star + "," + GetStar(2, ref star));
    }

    public string FilterStrReplace(string orgStr, List<string> filterStrs)
    {
        char[] c = orgStr.ToCharArray();

        List<int> charIdx = new List<int>();
        string onlyChar = "";
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
            {
                charIdx.Add(i);
                onlyChar += c[i];
            }
            else
            { }
        }

        List<int> repleaceStrIdx = new List<int>();

        foreach (var filterStr in filterStrs)
        {
            int idx = onlyChar.IndexOf(filterStr);

            if (idx >= 0)
            {
                for (int i = 0; i < filterStr.Length; ++i)
                {
                    repleaceStrIdx.Add(charIdx[i + idx]);
                }
            }
        }
        string replaceStr = "";
        for (int i = 0; i < orgStr.Length; ++i)
        {
            if (repleaceStrIdx.Contains(i))
            {
                replaceStr += "*";
            }
            else
            {
                replaceStr += orgStr[i];
            }
        }

        return replaceStr;
    }

    public int TestDoorQ(int testTimes)
    {
        int chooinTimes = 0;
        for (int i = 0; i < testTimes; ++i)
        {
            int randomAwardIdx = Random.Range(0, 3);

            int chooseIdx = Random.Range(0, 3);

            if (chooseIdx == randomAwardIdx)
            {
                ++chooinTimes;
            }
        }

        return chooinTimes;
    }

    public int TestDoorQ2(int testTimes)
    {
        int chooinTimes = 0;
        for (int i = 0; i < testTimes; ++i)
        {
            int randomAwardIdx = Random.Range(0, 3);

            int chooseIdx = Random.Range(0, 3);

            int changeIdx = -1;
            for (int j = 0; j < 3; ++j)
            {
                if (j != randomAwardIdx && j != chooseIdx)
                {
                    changeIdx = j;
                    break;
                }
            }

            int finalChoose = -1;
            for (int j = 0; j < 3; ++j)
            {
                if (j != changeIdx && j != chooseIdx)
                {
                    finalChoose = j;
                    break;
                }
            }

            if (finalChoose == randomAwardIdx)
            {
                ++chooinTimes;
            }
        }

        return chooinTimes;
    }

    public void SetStar(int idx, ref int Star)
    {
        Star = 1 << idx;
    }

    public bool GetStar(int idx, ref int Star)
    {
        return Star >> idx > 0;
    }
}
