using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mermi_Kutusu_olustur : MonoBehaviour
{
    public List<GameObject> MermiKutusuPoint = new List<GameObject>();
    public GameObject Mermi_kutusunun_kendisi;

   public static bool Mermi_kutusu_varmi;
    public float Kutu_cikma_suresi;

    
    void Start()
    {
        Mermi_kutusu_varmi = false;
        StartCoroutine(Mermi_Kutusu_yap());
    }

    

    IEnumerator Mermi_Kutusu_yap()
    {
        
        

       while (true)
        {

            yield return null;

            if (!Mermi_kutusu_varmi)
            {
                yield return new WaitForSeconds(Kutu_cikma_suresi);
                int randomsayim = Random.Range(0, 5);


                Instantiate(Mermi_kutusunun_kendisi, MermiKutusuPoint[randomsayim].transform.position, MermiKutusuPoint[randomsayim].transform.rotation);

                Mermi_kutusu_varmi = true;
            }
               
            

        }

    }
}
