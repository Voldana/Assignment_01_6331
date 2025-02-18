using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.UI
{
    public class LoseMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text reasonText, scoreText;
        
        [Inject] private Leaderboard leaderboard;
        [Inject] private string reason;
        
        private void Start()
        {
            DOTween.KillAll();
            reasonText.text = reason;
            if(!leaderboard) return;
            scoreText.text += $"\n Winner is {leaderboard.GetWinner().company} With {leaderboard.GetWinner().score} points!";
        }

        public void OnRetry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        

        public class Factory : PlaceholderFactory<string,Leaderboard, LoseMenu>
        {
        }
    }

}
