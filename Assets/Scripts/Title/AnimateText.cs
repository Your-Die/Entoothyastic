using UnityEngine;
using UnityEngine.EventSystems;
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
            _isHovering = true;
            startTime = Time.time;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovering = false;
            startTime = Time.time;
        }

        public void OnSelect(BaseEventData eventData)
        {
        }
    }
}
