using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneEventReceiver : MonoBehaviour {
    public GameObject systemObj;
    private TransportSystem system;
	// Use this for initialization
	void Start () {
        system = systemObj.GetComponent<TransportSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartBelt(float p)
    {
        system.StartBelt(0);
    }

    public void Catch(float p)
    {
        system.Catch();
    }

    public void Uncatch(float p)
    {
        system.Uncatch();
    }

    public void Transmute(float p)
    {
        system.Transmute();
    }
}
