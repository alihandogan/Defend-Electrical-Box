using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnamenuKontrol : MonoBehaviour
{
    public void OyunaBasla()
    {
        SceneManager.LoadScene(1);
    }

    public void OyundanCik()
    {
        Application.Quit();
    }
}
