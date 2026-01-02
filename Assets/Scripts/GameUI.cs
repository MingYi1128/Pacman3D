using TMPro;
using UnityEngine;


public class GameUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _pelletCounterText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _lifeText;


    public void setLife(int life)
    {
        _lifeText.text = life.ToString();
    }

    public void setLevel(int level)
    {
        _levelText.text = "Lvl " + level.ToString();
    }


    public void setPelletCount(int currentPelletCount, int totalPelletCount)
    {
        _pelletCounterText.text = currentPelletCount + "/" + totalPelletCount;
    }
}