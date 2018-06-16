using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WaitViewer : MonoBehaviour {
  Animator anim;

  void Start() {
    anim = GetComponent<Animator>();
  }

  public void Done() {
    Time.timeScale = 1f;
  }

  public void ViewGetReady() {
    anim.SetTrigger("Play");
    Time.timeScale = 0f;
  }
}