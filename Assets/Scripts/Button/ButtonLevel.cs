using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{
    public int levelID;
    [SerializeField] Button btnLevel;
    [SerializeField] Text t_level;
    [SerializeField] List<Transform> stars;

    public void Start()
    {
        t_level.text = levelID.ToString();
        btnLevel.onClick.AddListener(() => { GameManager.Instance.LoadLevel(levelID); });
    }
}
