using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class GameTimer : MonoBehaviour
{
    public int timeDuration;
    private int currentTime = 10000;
    private float lastSecond;
    private Animator animator;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bonusText;
    public Image sliderImage;
    public Color start;
    public Color middle;
    public Color end;
    public UnityEvent timeOut;
    private bool shouldCountDown = true;


    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currentTime = timeDuration;
        InvokeRepeating("UpdateTime", 1, 1);
        sliderImage.color = start;
        
    }

    private void UpdateTime()
    {
        if (shouldCountDown)
        {
            currentTime--;
            animator.SetTrigger("PassedSecond");
            SetTimerText(currentTime);
            UpdateSlider(currentTime);
            if (currentTime <= 5)
            {
                AddBonusTime(4);
            }
            if (currentTime <= 0)
            {
                timeOut.Invoke();
                shouldCountDown = false;
            }
        }
    }

    public void AddBonusTime(int time)
    {
        bonusText.text = "+" + time.ToString();
        currentTime += time + 1;
        if(currentTime >= timeDuration)
        {
            timeDuration = currentTime;
        }
        animator.SetTrigger("BonusTime");
    }


    private void SetTimerText(float number)
    {
        number = Mathf.FloorToInt(number);
        timerText.text = number.ToString();
    }

    private void UpdateSlider(int now)
    {
        float dec = 1.0f - ((float)now / (float)timeDuration);

        if (dec > 0.75f)
        {
            sliderImage.color = end;
        }

        else if (dec > 0.6f)
        {
            sliderImage.color = middle;
        }

        else
        {
            sliderImage.color = start;
        }

        sliderImage.fillAmount = dec;
    }

}
