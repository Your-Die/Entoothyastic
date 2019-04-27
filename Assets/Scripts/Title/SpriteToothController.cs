using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToothController : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _teeth = new List<RectTransform>();

    [SerializeField] private float _fallSpeed;
    [SerializeField] private int _numberOfTeeth;
    [SerializeField] private GameObject _toothSpritePrefab;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private TMPro.TextMeshProUGUI counter;
    
    private int score;
    
    private RectTransform _canvas;
    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        for (int i = 0; i < _numberOfTeeth; i++)
        {
            var go = Instantiate(_toothSpritePrefab, transform);
            var rect = go.GetComponent<RectTransform>();
            var tooth = go.GetComponent<ToothSprite>();
            tooth.SpriteToothController = this;
            _teeth.Add(rect);
            ResetTooth(rect, true);
        }
    }

    public void IncreaseCount()
    {
        score++;
        if (score == 5) _startButton.SetActive(true);
        if (counter != null) counter.text = "Teeth cleaned " + score.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        foreach (var tooth in _teeth)
        {
            MoveTooth(tooth);
        }
    }

    private void ResetTooth(RectTransform rectTransform, bool isNew = false)
    {
        var rand = Random.Range(-(_canvas.rect.width / 2), (_canvas.rect.width / 2));
        rectTransform.anchoredPosition = new Vector2(rand, isNew ? Random.Range(-_canvas.rect.height, _canvas.rect.height) : Random.Range(0, _canvas.rect.height));
        var randScale = Random.Range(.3f, 1.2f);
        rectTransform.localScale = Vector3.one * randScale;
        rectTransform.GetComponent<ToothSprite>().ResetTooth();
    }

    private void MoveTooth(RectTransform rectTransform)
    {
        var moved = Time.deltaTime * _fallSpeed;
        rectTransform.anchoredPosition -= new Vector2(0, moved);
        if (rectTransform.anchoredPosition.y < -(_canvas.rect.height + rectTransform.rect.height))
        {
            ResetTooth(rectTransform);
        }
    }
}
