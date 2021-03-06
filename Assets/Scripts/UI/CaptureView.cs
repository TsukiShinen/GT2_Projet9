using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptureView : MonoBehaviour
{
    [SerializeField] private Score score;
    [Space(10)]
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;

    public void Update()
    {
        var team = score.TeamScoring;
        
        if (team == null)
        {
            image.color = Color.white;
            image.fillAmount = 0;
            return;
        }

        image.color = team.Color;
        image.fillAmount = score.Progression / 100f;
        text.text = Mathf.FloorToInt(score.Progression).ToString();
    }
}
