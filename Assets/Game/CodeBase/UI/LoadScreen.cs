using System;
using DG.Tweening;
using UnityEngine;

namespace Game.CodeBase.UI
{
    public class LoadScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _loadScreenGroup;
        private static LoadScreen _instance;
        public static LoadScreen Instance => _instance;
        

        private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(this.gameObject);
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            
            gameObject.SetActive(false);
            _loadScreenGroup.alpha = 0f;
        }

        public void Show(Action func)
        {
            gameObject.SetActive(true);
            _loadScreenGroup.alpha = 0f;
            
            DOTween.Sequence()
                .Append(_loadScreenGroup.DOFade(1f, .5f))
                .AppendCallback(() => func?.Invoke())
                .AppendInterval(1f)
                .Append(_loadScreenGroup.DOFade(0f, 5f))
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}