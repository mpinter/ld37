﻿using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using flyyoufools;

public class InputScript : MonoBehaviour {
	public ReactiveProperty<bool> MovementUnlocked { get; set; }
	public IObservable<Vector2> Movement { get; private set; }
	private Animator playerAnimator;
	private void Awake() {
		Movement = this.UpdateAsObservable()
		//.Throttle(System.TimeSpan.FromSeconds(1))
		.Where(_ => {
			return !playerAnimator.GetBool("Run");
		})
		.Select(_ => {
			var x = Input.GetAxis("Horizontal");
			var y = Input.GetAxis("Vertical");
			playerAnimator.SetBool("Run", true);
			return new Vector2(x, -y).normalized;
		});
	}

	void Start () {
		playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
	}
}
