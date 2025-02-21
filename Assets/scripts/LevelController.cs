using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class LevelController : MonoBehaviour
{
    [SerializeField] internal Color[] colors;

    internal static LevelController instance;

    internal bool won = false;

    [SerializeField] internal GameObject winpanel;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text ScoreText;
    [SerializeField] Transform scoreBoard;
    [SerializeField] Transform[] coins;
    [SerializeField] ParticleSystem particleSystem1;
    internal int level = 1;
    internal int score = 0;
    void Awake()
    {
        instance = this;
        level = PlayerPrefs.GetInt("currentLevel", 1);
        score = PlayerPrefs.GetInt("score", 0);

        levelText.text = level.ToString();
        ScoreText.text = score.ToString();
        //PlayerPrefs.DeleteAll();
    }


    public void OnWin()
    {
        level++;
        score += 5;

        PlayerPrefs.SetInt("currentLevel", level);
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
        this.particleSystem1.Stop();
        this.particleSystem1.Play();
        Invoke(nameof(OnGameOver), 1f);

    }

    void OnGameOver() {

        winpanel.SetActive(true);
        float delay = 0;
        for (int i = 0; i < coins.Length; i++)
        {
            int index = i;
            coins[index].gameObject.SetActive(true);
            coins[index].DOLocalMove(scoreBoard.transform.localPosition, 0.1f + (index * 0.1f)).OnComplete(() => {
                coins[index].gameObject.SetActive(false);

            });
            delay += 0.1f + (index * 0.1f);
        }

        DOVirtual.DelayedCall(0.2f, () =>
        {
            ScoreText.text = score.ToString();

        });
    }

    public void ReloadScene() { 
    
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

}
