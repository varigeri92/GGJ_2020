using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void ZoomIn()
	{
		animator.SetBool("ZoomIn", true);
	}

	public void ZoomOut()
	{
		animator.SetBool("ZoomIn", false);
	}
}
