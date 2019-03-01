using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Pics
{
    public string[] picSlot;
}

public class Timer : MonoBehaviour {

    public Pics[] pics;


    public int sizeOfArray;
    public int count = 0;
    public int endcount = 0;
    public Text TimerUI;

    

    public CanvasGroup[] uiElement = new CanvasGroup[5];

    public string[] slideStart = new string[5];
    public string[] slideEnd = new string[5];
    public Texture2D[] imageNum = new Texture2D[5];//Image that will be used for the canvas
    public int[] imageSlot = new int[5];//Slot that will be used for the image
    public int scene1;
    public int[] slides = new int[5];
    public RawImage[] slideImage = new RawImage[5];//the 5 slot canvas
    private float startTimer;
    


	void Start () {

        startTimer = Time.time;

	}
	
	// Update is called once per frame
	void Update () {
        float t = Time.time - startTimer;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0");
        TimerUI.text = minutes + ":" + seconds;
        checkStartTime();
        checkEndTime();
    }

    public void checkStartTime()
    {
        for (int i = 0; i < sizeOfArray; i++)
        {
            if (slideStart[count] == TimerUI.text)
            {
                setImage();
                FadeIn();
                count++;
            }
        }
    }
    public void checkEndTime()
    {
        for (int i = 0; i < sizeOfArray; i++)
        {
            if (slideEnd[endcount] == TimerUI.text)
            {
                FadeOut();
                endcount++;
            }
        }
    }

    public void setImage()
    {
        slideImage[imageSlot[count]].texture = imageNum[count];
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(uiElement[imageSlot[count]], uiElement[imageSlot[count]].alpha, 1, .5f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(uiElement[imageSlot[endcount]], uiElement[imageSlot[endcount]].alpha, 0, .5f));
     
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;
        
        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }

        print("done");
    }
}
