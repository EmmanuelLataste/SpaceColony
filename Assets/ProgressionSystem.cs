using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSystem : MonoBehaviour {

    public bool[] levels;
    public float[] newTimerPosses;
    public float[] newRangeManip;
    public float[] newRadiusContactControl;
    public int statusProg;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Progression();
	}

    public void Progression()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if ( i == statusProg)
            {
                levels[i] = true;
            }

            else
            {
                levels[i] = false;
            }
        }

        if (levels[statusProg] == true)
        {
            MindPower.timerPossess = newTimerPosses[statusProg];
            MindPower.rangeManip = newRangeManip[statusProg];
            MindPower.radiusContactControl = newRadiusContactControl[statusProg];
        }
    }
}
