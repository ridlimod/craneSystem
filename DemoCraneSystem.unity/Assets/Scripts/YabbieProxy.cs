using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YabbieProxy : MonoBehaviour {
    public Color color = Color.white;
    public enum YabbieState { Free, Catched, Captured };
    public YabbieState yabbieState = YabbieState.Free;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateColor()
    {
        Material mat = GetComponent<Renderer>().material;
        mat.color = color;
    }
}
