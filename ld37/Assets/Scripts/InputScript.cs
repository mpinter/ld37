using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using flyyoufools;

public class InputScript : MonoBehaviour {
	public IObservable<Vector2> Movement { get; private set; }
	public IObservable<bool> Spacebar { get; private set; }
	public IObservable<bool> Escape { get; private set; }
	private Animator playerAnimator;
	private MasterScript masterScript;
	private void Awake() {
		Movement = this.UpdateAsObservable()
		//.Throttle(System.TimeSpan.FromSeconds(1))
		.Where(_ => {
			return !playerAnimator.GetBool("Run") && !masterScript.blockInput;
		})
		.Select(_ => {
			var x = Input.GetAxis("Horizontal");
			var y = Input.GetAxis("Vertical");
			return new Vector2(x, -y).normalized;
		})
		.Where(v => v != Vector2.zero)
		.Do(_ => {
			playerAnimator.SetBool("Run", true);
		});
		
		Spacebar = this.UpdateAsObservable()
		.Select(_ => {
			//Debug.Log(Input.GetKeyDown("space"));
			return Input.GetKeyDown("space");
		});
		
		Escape = this.UpdateAsObservable()
		.Select(_ => {
			//Debug.Log(Input.GetKeyDown("escape"));
			return Input.GetKeyDown("escape");
		});
	}

	void Start () {
		playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
		masterScript = GameObject.FindWithTag("Master").GetComponent<MasterScript>();
	}
}
