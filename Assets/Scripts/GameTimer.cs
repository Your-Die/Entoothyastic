using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class GameTimer : MonoBehaviour
{
    public int timeDuration;
    public int failurePenalty = 2;
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

    [Header("Animation Triggers")]
    [SerializeField]private string secondPassedTrigger = "PassedSecond";
    [SerializeField]private string bonusTimeTrigger = "BonusTime";
    [SerializeField]private string penaltyTrigger = "Penalty";

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        currentTime = timeDuration;
        InvokeRepeating(nameof(UpdateTime), 1, 1);
        sliderImage.color = start;
    }

    private void OnEnable()
    {
        InputHandler.Instance.InvalidInputRegistered.AddListener(ApplyPenalty);
    }

    private void OnDisable()
    {
        InputHandler.Instance.InvalidInputRegistered.RemoveListener(ApplyPenalty);
    }


    private void UpdateTime()
    {
        if (shouldCountDown)
        {
            AudioManager.instance.Play("Vocal3");
            currentTime--;
            animator.SetTrigger(secondPassedTrigger);
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        // Clamp.
        if (currentTime < 0)
            currentTime = 0;

        SetTimerText(currentTime);
        UpdateSlider(currentTime);

        if (currentTime == 0)
        {
            timeOut.Invoke();
            shouldCountDown = false;
        }
    }

    public void AddBonusTime(int time)
    {
        bonusText.text = "+" + time;
        currentTime += time + 1;
        if(currentTime >= timeDuration)
        {
            timeDuration = currentTime;
        }
        animator.SetTrigger(bonusTimeTrigger);
    }

    [ContextMenu("Penalty")]
    public void ApplyPenalty()
    {
        bonusText.text = "-" + failurePenalty;
        currentTime -= failurePenalty;
        animator.SetTrigger(penaltyTrigger);
        
        UpdateTimerDisplay();
    }

    private void SetTimerText(float number)
    {
        number = Mathf.FloorToInt(number);
        timerText.text = number.ToString();
    }

    private void UpdateSlider(int now)
    {
        float dec = 1.0f - ((float)now / (float)timeDuration);

        AudioManager.instance.sounds.First(s => s.name == "Vocal3").pitch = dec * 1.5f;

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
