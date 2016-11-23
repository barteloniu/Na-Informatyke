using UnityEngine;
using System.Collections;

public class Kamera : MonoBehaviour
{

    GameObject gracz;

	void Start ()
    {
        gracz = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update ()
    {
        transform.LookAt(gracz.transform);
	}
}
