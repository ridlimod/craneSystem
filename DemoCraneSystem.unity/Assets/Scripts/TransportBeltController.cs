using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportBeltController : MonoBehaviour {
    public Transform DummyCtl;
    public Material objMaterial;
    public bool x, y;
    public float sens = 0.25f;
    private float xoff=0f, yoff=0f;
    private float mx, my;
    private Material instanceMat;
    // Use this for initialization
    void Start () {
		//WorldObjects.GameSounds.sourcesNeedVolume.Add(GetComponent<AudioSource>());
        xoff = -DummyCtl.position.z;
        yoff = -DummyCtl.position.x;
        
        instanceMat = Instantiate<Material>(objMaterial);
        GetComponent<Renderer>().material = instanceMat;
    }
	
	// Update is called once per frame
	void Update () {
        if (x) mx = (DummyCtl.position.z + xoff)*sens;
        else mx = 0;
        if (y) my = (-DummyCtl.position.x + yoff)*sens;
        else my = 0;

        instanceMat.mainTextureOffset = new Vector2(mx, my);
    }

	void OnDestroy(){
		//WorldObjects.GameSounds.sourcesNeedVolume.Remove(GetComponent<AudioSource>());
	}
}
