using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public HeroData heroData = new HeroData();

    public FSMController fsmController;
	// Use this for initialization
	void Awake () {
        fsmController = GetComponent<FSMController>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
