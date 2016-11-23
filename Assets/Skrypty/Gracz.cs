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
                if (t.position.x < Screen.width / 2) prawo = true;
                else lewo = true;
            }
        }
        else
        {
            if (Input.GetKey("d")) lewo = true;
            if (Input.GetKey("a")) prawo = true;
        }

        //ruch i swiatlo silnikow-------------------------------------------------------
        foreach (Light l in GetComponentsInChildren<Light>())
        {
            l.enabled = false;
        }
        if (lewo)
        {
            transform.FindChild("Spotlight").GetComponent<Light>().enabled = true;
            transform.FindChild("Point light").GetComponent<Light>().enabled = true;
            rb.AddForceAtPosition(transform.forward * 1000 * Time.deltaTime, silniki[0].transform.position);
        }
        if (prawo)
        {
            transform.FindChild("Spotlight (1)").GetComponent<Light>().enabled = true;
            transform.FindChild("Point light (1)").GetComponent<Light>().enabled = true;
            rb.AddForceAtPosition(transform.forward * 1000 * Time.deltaTime, silniki[1].transform.position);
        }
        //pociski ruch----------------------------------------------------------------
        foreach (GameObject p in pociski)
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

            yield return new WaitForSeconds(0.25f);
        }
    }
}
