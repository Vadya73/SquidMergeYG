using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LoadScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _loadScreenGroup;
        
        private void Awake()
        {
            gameObject.SetActive(false);
            _loadScreenGroup.alpha = 0f;
        }

        public async void LoadScene(int sceneIndex)
        {
            Show();
            await SceneManager.LoadSceneAsync(sceneIndex);
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
            _loadScreenGroup.alpha = 0f;
            _loadScreenGroup.DOFade(1f, .5f);
        }

        private void Hide()
        {
            _loadScreenGroup.DOFade(0f, 0.5f)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}