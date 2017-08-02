using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportSystem : MonoBehaviour {
    public GameObject grua;
    public Material mPrecatched, mCatched, mPostcatched;
    public float Speed;
    public Color[] yabbiecolor = new Color[3] { new Color(1.0f,0.5f,0.5f) , new Color(0.5f, 1.0f, 0.5f), new Color(0.5f, 0.5f, 1.0f) };
    private Animator gruaAnimatior;
    private enum sState { wait,waiting, move, moving};
    private sState cState = sState.wait;
    private int steps = 0;
    private float movement = 0f;
    private float initpos = 0f;
    private float firstpos;
    private List<GameObject> yabbies = new List<GameObject>();
    private int first, last, catched;
    private Vector3 catchPos;
    private Quaternion catchRot;
    private Vector3 catchScale;
    private Transform catchParent;
    public Transform catchObject;
	public AudioSource controllerAudio;

    // Use this for initialization
    void Start () {
        gruaAnimatior = grua.GetComponent<Animator>();
        movement = gameObject.transform.position.x;
        initpos = movement;
        
		SetYabbiesInstances();

        first = 0;
        last = transform.childCount-1;
        catched = 5;
        firstpos = yabbies[first].transform.position.x;
    }

	private void SetYabbiesInstances(){
		for(int i = 0; i < transform.childCount; i++){
            GameObject yabbie = transform.GetChild(i).gameObject;
            yabbies.Add(yabbie);

            YabbieProxy yabbieProxy = yabbie.GetComponent<YabbieProxy>();
            yabbieProxy.color = rndColor();
            if (yabbieProxy.yabbieState==YabbieProxy.YabbieState.Captured)
            {
                yabbieProxy.color = Darkener(yabbieProxy.color);
            }
            yabbieProxy.UpdateColor();
		}
	}

    private Color rndColor()
    {
        float frnd = (Random.value * yabbiecolor.Length);
        int rColorIx = Mathf.RoundToInt(frnd) % yabbiecolor.Length;
        return yabbiecolor[rColorIx];
    }

    // Update is called once per frame
    void Update()
    {
        switch (cState)
        {
            case sState.move:
                Move();
                break;
            case sState.wait:
                if (steps == 3)
                {
                    restart();
                }
                ActivateGrua();
                break;
        }
    }

    void Move()
    {
        cState = sState.moving;
        movement = movement - 4;
        steps += 1;

        LeanTween.moveX(gameObject, movement, 1f/Speed).setOnComplete(EndMove).setEaseInOutQuad();
    }

    void EndMove()
    {
        GameObject yabbie = yabbies[last];
        yabbies[last] = null;

        first = last;
        last = ((last + yabbies.Count) - 1) % yabbies.Count;
        catched = ((catched + yabbies.Count) - 1) % yabbies.Count;

        yabbies[first] = yabbie;
        yabbies[first].transform.position = yabbies[last].transform.position;
        yabbies[first].transform.rotation = yabbies[last].transform.rotation;
        yabbies[first].transform.localScale = yabbies[last].transform.localScale;
        yabbies[first].GetComponent<Renderer>().material = mPrecatched;

        YabbieProxy yabbieProxy = yabbies[first].GetComponent<YabbieProxy>();
        yabbieProxy.color = rndColor();
        yabbieProxy.UpdateColor();
        yabbieProxy.yabbieState = YabbieProxy.YabbieState.Free;

        LeanTween.moveX(yabbies[first], firstpos, 0).setOnComplete(EndMove2);

    }
    void EndMove2()
    { 
        cState = sState.wait;
    }

    void ActivateGrua()
    {
        Debug.Log("waiting");
        cState = sState.waiting;
        gruaAnimatior.SetTrigger("Activate");
		controllerAudio.Play();
    }

    public void StartBelt(float p)
    {
        Debug.Log("move");
        cState = sState.move;
    }

    public void restart()
    {
        List<float> lTM = new List<float>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            lTM.Add(transform.GetChild(i).position.x);
        }

        LeanTween.moveX(gameObject, initpos, 0f);
        
        steps = 0;
        
        for (int i = 0;i<transform.childCount;++i)
        {
            LeanTween.moveX(transform.GetChild(i).gameObject,lTM[i], 0f);
        }
        movement = initpos;
    }

    public void Catch()
    {
        GameObject yabbie = yabbies[catched];
        catchPos = yabbie.transform.position;
        catchRot = yabbie.transform.rotation;
        catchScale = yabbie.transform.localScale;
        catchParent = yabbie.transform.parent;
        yabbie.transform.SetParent(catchObject);

        YabbieProxy yabbieProxy = yabbie.GetComponent<YabbieProxy>();
        yabbieProxy.yabbieState = YabbieProxy.YabbieState.Catched;
    }

    public void Uncatch()
    {
        GameObject yabbie = yabbies[catched];
        yabbie.transform.SetParent(catchParent);
        yabbie.transform.position = catchPos;
        yabbie.transform.rotation = catchRot;
        yabbie.transform.localScale = catchScale;

        YabbieProxy yabbieProxy = yabbie.GetComponent<YabbieProxy>();
        yabbieProxy.yabbieState = YabbieProxy.YabbieState.Captured;
    }

    public void Transmute()
    {
        GameObject yabbie = yabbies[catched];
        Renderer yabbieRend = yabbie.GetComponent<Renderer>();
        yabbieRend.material= mPostcatched;

        YabbieProxy yabbieProxy = yabbie.GetComponent<YabbieProxy>();
        yabbieProxy.color = Darkener(yabbieProxy.color);
        yabbieProxy.UpdateColor();      
    }

    private Color Darkener(Color color)
    {
        color.r -= 0.5f;
        color.g -= 0.5f;
        color.b -= 0.5f;

        return color;
    }
}
