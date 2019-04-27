using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Title
{
    public class AnimateText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISelectHandler
    {
        [SerializeField] private AnimationCurve _animationCurve;
        private bool _isHovering;
        private float startTime;
        [SerializeField] private float _animationSpeed;
        [SerializeField] private float _maximumSize;
        private Vector3 _startScale;
        public Outline Outline;

        private void Start()
        {
            _startScale = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            Animate();
        }

        private void Animate()
        {
            var timeDelta = _isHovering ? Time.time - startTime : startTime - Time.time;
            transform.localScale =
                _startScale + (_animationCurve.Evaluate(timeDelta * _animationSpeed) * _maximumSize) * Vector3.one;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Outline) Outline.enabled = true;
            AudioManager.instance.Play("Whoosh");
            _isHovering = true;
            startTime = Time.time;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Outline) Outline.enabled = false;
            _isHovering = false;
            startTime = Time.time;
        }

        public void OnSelect(BaseEventData eventData)
        {
        }
    }
}
