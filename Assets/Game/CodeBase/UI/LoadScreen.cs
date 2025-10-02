using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoadScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _loadScreenGroup;
        [Header("Progress")]
        [SerializeField] private GameObject _progressObject;
        [SerializeField] private Image _progressImage;
        [Header("Accept Obj")]
        [SerializeField] private GameObject _acceptPressObject;
        [SerializeField] private Button _acceptButton;

        public event Action OnSceneLoad;
        public event Action OnLoadScreenShow;
        public event Action OnLoadScreenHide;
        
        private void Awake()
        {
            _acceptPressObject.SetActive(false);
            gameObject.SetActive(false);
            _loadScreenGroup.alpha = 0f;

            OnSceneLoad += ShowAcceptToPress;
            _acceptButton.onClick.AddListener(Hide); 
        }

        private void OnDestroy()
        {
            OnSceneLoad -= ShowAcceptToPress;
            _acceptButton.onClick.RemoveListener(Hide);
        }

        public async void LoadScene(int sceneIndex)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                await SceneManager.LoadSceneAsync(sceneIndex); 
                return;
            }
            
            Show();
            await Task.Delay(TimeSpan.FromSeconds(1f));
            await SceneManager.LoadSceneAsync(sceneIndex);

            LoadProgressImage(sceneIndex != 1);
        }

        private void LoadProgressImage(bool needWait)
        {
            if (needWait)
                _progressImage.DOFillAmount(1f,2f)
                    .OnComplete(() => OnSceneLoad?.Invoke());
            else
                _progressImage.DOFillAmount(1f,2f)
                    .OnComplete(Hide);
        }

        private void Show()
        {
            OnLoadScreenShow?.Invoke();
            gameObject.SetActive(true);
            _progressObject.SetActive(true);
            _acceptPressObject.SetActive(false);
            
            _progressImage.fillAmount = 0f;
            _loadScreenGroup.alpha = 0f;
            _loadScreenGroup.DOFade(1f, .5f);
        }

        private void Hide()
        {
            _loadScreenGroup.DOFade(0f, 0.5f)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    OnLoadScreenHide?.Invoke();
                });
        }

        private void ShowAcceptToPress()
        {
            _progressObject.SetActive(false);
            _acceptPressObject.SetActive(true);
        }
    }
}