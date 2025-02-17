using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.UI
{
    public class LevelMenu: MonoBehaviour
    {
        private void Start()
        {
            ChangeTimeScale(true);
        }

        private static void ChangeTimeScale(bool freeze)
        {
            Time.timeScale = freeze ? 0 : 1;
            DOTween.timeScale = freeze ? 0 : 1;
        }

        public void OnResume()
        {
            ChangeTimeScale(false);
            Destroy(gameObject);
        }

        public void OnLevelChange(string level)
        {
            ChangeTimeScale(true);
            SceneManager.LoadScene(level);
        }
        

        public class Factory : PlaceholderFactory<LevelMenu>
        {
        }
    }
}