using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class AnimatedBlocks : MonoBehaviour {
    private Animator anim;
  //private int flag = 0;
    public GameObject prefab;
    // Use this for initialization
    void Start () {
        anim = gameObject.GetComponentInChildren<Animator>();
        InputManager.Instance.PushModalInputHandler(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("w"))
        {
            anim.SetInteger("WallBroke", 1);
        }
        else
        {
            anim.SetInteger("WallBroke", 0);
        }
    }
}
