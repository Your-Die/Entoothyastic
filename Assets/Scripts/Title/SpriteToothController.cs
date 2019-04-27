using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToothController : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _teeth = new List<RectTransform>();

    [SerializeField] private float _fallSpeed;
    [SerializeField] private int _numberOfTeeth;
    [SerializeField] private GameObject _toothSpritePrefab;

    private RectTransform _canvas;
    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        for (int i = 0; i < _numberOfTeeth; i++)
        {
            var go = Instantiate(_toothSpritePrefab, transform);
            var rect = go.GetComponent<RectTransform>();
            _teeth.Add(rect);
            ResetTooth(rect, true);
        }
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
        rectTransform.anchoredPosition = new Vector2(rand, isNew ? Random.Range(-_canvas.rect.height, 0) : 0);
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
