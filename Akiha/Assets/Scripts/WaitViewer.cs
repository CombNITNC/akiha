using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(UITransition))]
public class WaitViewer : MonoBehaviour {
  Animator anim;
  UITransition transitioner;

  void Start() {
    anim = GetComponent<Animator>();
    transitioner = GetComponent<UITransition>();
  }

  public void Done() {
    gameObject.SetActive(false);
  }

  public void ViewGetReady() {
    gameObject.SetActive(true);
    anim.SetTrigger("Play");
  }
}