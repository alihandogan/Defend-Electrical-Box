using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;


public class GameKontrolcu : MonoBehaviour
{
    float health = 100;

    [Header("SAGLÝK AYARLARI")]
    public Image HealthBar;

    [Header("SÝLAH AYARLARI")]
    public GameObject[] silahlar;
    public AudioSource degisimSesi;
    public GameObject Bomba;
    public GameObject BombaPoint;
    public Camera benimCam;

    [Header("DUSMAN AYARLARI")]
    public GameObject[] dusmanlar;
    public GameObject[] cikisNoktalari;
    public GameObject[] hedefNoktalar;
    public float DusmancikmaSuresi;
    public Text KalanDusman_text;
    public int Baslangic_dusman_sayisi;
    public static int Kalan_dusman_sayisi;

    [Header("DÝGER AYARLAR")]
    public GameObject GameOverCanvas;
    public GameObject KazandinCanvas;
    public GameObject PauseCanvas;

    public AudioSource OyunIcSes;



    void Start()
    {
        KalanDusman_text.text = Baslangic_dusman_sayisi.ToString();
        Kalan_dusman_sayisi = Baslangic_dusman_sayisi;
       

        if (!PlayerPrefs.HasKey("OyunBasladimi"))
        {
             PlayerPrefs.SetInt("Taramali_Mermi", 320);
             PlayerPrefs.SetInt("Pompali_Mermi", 250);
             PlayerPrefs.SetInt("Magnum_Mermi", 30);
             PlayerPrefs.SetInt("Sniper_Mermi", 20);
            
            PlayerPrefs.SetInt("OyunBasladimi", 1);
        }
        StartCoroutine(DusmanCikar());
        OyunIcSes = GetComponent<AudioSource>();
        OyunIcSes.Play();
    }


    IEnumerator DusmanCikar()
    {
        

        while (true)
        {
            yield return new WaitForSeconds(DusmancikmaSuresi);
            if (Baslangic_dusman_sayisi!=0)
            {
                int dusman = Random.Range(0, 5);
                int cikisnoktasi = Random.Range(0, 2);
                int hedefnoktasi = Random.Range(0, 2);

                GameObject Obje = Instantiate(dusmanlar[dusman], cikisNoktalari[cikisnoktasi].transform.position, Quaternion.identity);
                Obje.GetComponent<Dusman>().HedefBelirle(hedefNoktalar[hedefnoktasi]);
                Baslangic_dusman_sayisi--;
            }

        }

    }   

    public void Dusman_sayisi_guncelle()
    {

        Kalan_dusman_sayisi--;
        if (Kalan_dusman_sayisi<=0)
        {
            KazandinCanvas.SetActive(true);
            KalanDusman_text.text = "0";
            Time.timeScale = 0;
        }
        else
        {
            KalanDusman_text.text = Kalan_dusman_sayisi.ToString();
        }
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            degisimSesi.Play();
            silahlar[0].SetActive(true);
            silahlar[1].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            degisimSesi.Play();
            silahlar[0].SetActive(false);
            silahlar[1].SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject obje = Instantiate(Bomba, BombaPoint.transform.position, BombaPoint.transform.rotation);
            Rigidbody rg = obje.GetComponent<Rigidbody>();
            Vector3 acimiz = Quaternion.AngleAxis(90, benimCam.transform.forward) * benimCam.transform.forward;
            rg.AddForce(acimiz * 250f);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void DarbeAl(float darbegucu)
    {
        health -= darbegucu;
        HealthBar.fillAmount = health / 100;
        if (health <= 0)
        {
            GameOver();
        }
    }

    public void Saglik_doldur()
    {
        health = 100;
        HealthBar.fillAmount = health / 100;

    }


    void GameOver()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void BastanBasla()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;

    }

    public void DevamEt()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void anaMenu()
    {
        SceneManager.LoadScene(0);
    } 



}
