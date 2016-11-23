using UnityEngine;
using System.Collections;

public class Gracz : MonoBehaviour
{
    public GameObject pociskPref;
    public GameObject empty;

    GameObject[] pociski = new GameObject[20];

    GameObject pociskiMatka;

    bool lewo;
    bool prawo;

    GameObject[] silniki = new GameObject[2];

    Rigidbody rb;


	void Start ()
    {
        //setup---------------------------------------------------------------------
        rb = GetComponent<Rigidbody>();
        silniki[0] = transform.FindChild("Cylinder (1)").gameObject;
        silniki[1] = transform.FindChild("Cylinder (2)").gameObject;

        pociskiMatka = (Instantiate(empty, new Vector3(0, -100, 0), Quaternion.identity) as GameObject);
        pociskiMatka.name = "PociskiMatkaGracz";
        for(int i = 0; i < 20; i++)
        {
            pociski[i] = (GameObject)Instantiate(pociskPref, pociskiMatka.transform.position, Quaternion.identity, pociskiMatka.transform);
        }
        StartCoroutine(strzelaj());
    }
	
	void FixedUpdate ()
    {
        //sterowanie----------------------------------------------------------------
        lewo = false;
        prawo = false;
        if (Input.touchSupported)
        {
            foreach (Touch t in Input.touches)
            {
                if (t.position.x < Screen.width / 2) lewo = true;
                else prawo = true;
            }
        }
        else
        {
            if (Input.GetKey("a")) lewo = true;
            if (Input.GetKey("d")) prawo = true;
        }

        //ruch-----------------------------------------------------------------------
        if (lewo)
        {
            rb.AddForceAtPosition(transform.forward * 1000 * Time.deltaTime, silniki[1].transform.position);
        }
        if (prawo)
        {
            rb.AddForceAtPosition(transform.forward * 1000 * Time.deltaTime, silniki[0].transform.position);
        }
        //pociski ruch----------------------------------------------------------------
        foreach(GameObject p in pociski)
        {
            p.transform.Translate(new Vector3(0, 0, 50 * Time.deltaTime));
        }
    }

    IEnumerator strzelaj()
    {
        int ktoryPocisk = 0;
        while (true)
        {
            Color kolor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            foreach (Light l in transform.GetComponentsInChildren<Light>())
            {
                l.color = kolor;
            }
            pociski[ktoryPocisk].transform.position = transform.position;
            pociski[ktoryPocisk].transform.rotation = transform.rotation;
            pociski[ktoryPocisk].GetComponent<Renderer>().material.color = kolor;
            ktoryPocisk++;
            if (ktoryPocisk == 20) ktoryPocisk = 0;

            yield return new WaitForSeconds(0.3f);
        }
    }
}
