using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pompali : MonoBehaviour
{
    Animator animatorum;

    [Header("AYARLAR")]
    public bool atesEdebilirmi;
    float ›ceridenAtesEtmeSikligi;
    public float disaridanAtesEtmeSiklik;
    public float menzil;

    [Header("SESLER")]
    public AudioSource AtesSesi;
    public AudioSource SarjorDegistirme;
    public AudioSource MermiBittiSes;
    public AudioSource MermiAlmaSesi;


    [Header("EFEKTLER")]
    public ParticleSystem AtesEfekt;
    public ParticleSystem Mermi›zi;
    public ParticleSystem KanEfekti;

    [Header("D›GERLER›")]
    public Camera benimCam;

    [Header("S›LAH AYARLAR")]
    int ToplamMermiSayisi;
    public int SarjorKapasitesi;
    int KalanMermi;
    public string Silahin_adi;
    public Text ToplamMermi_Text;
    public Text KalanMermi_Text;
    public float DarbeGucu;

    int AtilmisOlanMermi;


    void Start()
    {

        ToplamMermiSayisi = PlayerPrefs.GetInt(Silahin_adi + "_Mermi");
        Baslangic_mermi_doldur();
        
        SarjordoldurmaTeknikFonksiyon("NormalYaz");
        animatorum = GetComponent<Animator>();

    
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0) )
        {
            if (atesEdebilirmi && Time.time > ›ceridenAtesEtmeSikligi && KalanMermi != 0)
            {
                AtesEt();
                ›ceridenAtesEtmeSikligi = Time.time + disaridanAtesEtmeSiklik;

            }
            if(KalanMermi==0)
            {
                MermiBittiSes.Play();
            }

            
        }

        if (Input.GetKey(KeyCode.R) )
        {
            if (KalanMermi < SarjorKapasitesi && ToplamMermiSayisi!=0)
            {
                if (KalanMermi != 0)
                {
                    SarjordoldurmaTeknikFonksiyon("MermiVar");
                }
                else
                {
                    SarjordoldurmaTeknikFonksiyon("MermiYok");
                }
                

                animatorum.Play("sarjordegistir");
            }


            
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            MermiAL();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mermi"))
        {
            MermiKaydet(other.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Silahin_Turu, other.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Mermi_sayisi);

            Mermi_Kutusu_olustur.Mermi_kutusu_varmi = false;
            Destroy(other.transform.parent.gameObject);
        }

    }


    void SarjorDegistir()
    {
        SarjorDegistirme.Play();
    }

    void Baslangic_mermi_doldur()
    {
        if (ToplamMermiSayisi <= SarjorKapasitesi)
        {

            KalanMermi = SarjorKapasitesi;
            ToplamMermiSayisi = 0;
            PlayerPrefs.SetInt(Silahin_adi + "_Mermi", 0);
        }
        else
        {

            KalanMermi = SarjorKapasitesi;
            ToplamMermiSayisi -= SarjorKapasitesi;
            PlayerPrefs.SetInt(Silahin_adi + "_Mermi", ToplamMermiSayisi);

        }
    }

    void SarjordoldurmaTeknikFonksiyon(string tur)
    {
        switch (tur)
        {
            case "MermiVar":
                if (ToplamMermiSayisi <= SarjorKapasitesi)
                {
                    int OlusanToplamDeger = KalanMermi + ToplamMermiSayisi;

                    if (OlusanToplamDeger >SarjorKapasitesi)
                    {
                        KalanMermi = SarjorKapasitesi;
                         ToplamMermiSayisi= OlusanToplamDeger - SarjorKapasitesi;
                        PlayerPrefs.SetInt(Silahin_adi + "_Mermi", ToplamMermiSayisi);
                    }
                    else
                    {
                        KalanMermi += ToplamMermiSayisi;
                        ToplamMermiSayisi = 0;
                        PlayerPrefs.SetInt(Silahin_adi + "_Mermi", 0);
                    }
                    
                }
                else
                {
                    AtilmisOlanMermi = SarjorKapasitesi - KalanMermi;

                    ToplamMermiSayisi = ToplamMermiSayisi - AtilmisOlanMermi;
                    KalanMermi = SarjorKapasitesi;
                    PlayerPrefs.SetInt(Silahin_adi + "_Mermi", ToplamMermiSayisi);
                }


                
                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();
                break;

            case "MermiYok":
                if (ToplamMermiSayisi <= SarjorKapasitesi)
                {
                    KalanMermi = ToplamMermiSayisi;
                    ToplamMermiSayisi = 0;
                    PlayerPrefs.SetInt(Silahin_adi + "_Mermi", 0);
                }
                else
                {
                    ToplamMermiSayisi = ToplamMermiSayisi - SarjorKapasitesi;
                    PlayerPrefs.SetInt(Silahin_adi + "_Mermi", ToplamMermiSayisi);
                    KalanMermi = SarjorKapasitesi;
                }
                
                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();

                break;

            case "NormalYaz":
                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();
                break;
        }

    }

    void MermiAL()
    {
        RaycastHit hit;
        if (Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, 4))
        {
            if (hit.transform.gameObject.CompareTag("Mermi"))
            {
                MermiKaydet(hit.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Silahin_Turu, hit.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Mermi_sayisi);

                Mermi_Kutusu_olustur.Mermi_kutusu_varmi = false;
                Destroy(hit.transform.parent.gameObject);
            }


        }



    }


    void AtesEt() 
    {
        AtesSesi.Play();
        AtesEfekt.Play();
        animatorum.Play("ateset");


        KalanMermi--;
        KalanMermi_Text.text = KalanMermi.ToString();

        RaycastHit hit;

        if (Physics.Raycast(benimCam.transform.position,benimCam.transform.forward, out hit,menzil))
        {
            if (hit.transform.gameObject.CompareTag("Dusman"))
            {
                Instantiate(KanEfekti, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<Dusman>().DarbeAl(DarbeGucu);
            }
            else
            {
                Instantiate(Mermi›zi, hit.point, Quaternion.LookRotation(hit.normal));
            }

           
        }

        //Instantiate(Mermi›zi, hit.point, Quaternion.LookRotation(hit.normal));
    }

    void MermiKaydet(string silahturu,int mermisayisi)
    {

        MermiAlmaSesi.Play();

        switch (silahturu)
        {
            case "Taramali":
                PlayerPrefs.SetInt("Taramali_Mermi", PlayerPrefs.GetInt("Taramali_Mermi") + mermisayisi);
                break;

            case "Pompali":
                ToplamMermiSayisi += mermisayisi;
                PlayerPrefs.SetInt(Silahin_adi + "_Mermi", ToplamMermiSayisi);
                SarjordoldurmaTeknikFonksiyon("NormalYaz");
                break;

            case "Magnum":

                break;

            case "Sniper":
               
                break;
        }
    }
}
