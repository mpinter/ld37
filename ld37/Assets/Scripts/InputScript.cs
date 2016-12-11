using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using flyyoufools;

public class InputScript : MonoBehaviour {
	public ReactiveProperty<bool> MovementUnlocked { get; set; }
	public IObservable<Vector2> Movement { get; private set; }
	private void Awake() {
		Movement = this.UpdateAsObservable()
		//.Throttle(System.TimeSpan.FromSeconds(1))
		.Select(_ => {
			var x = Input.GetAxis("Horizontal");
			var y = Input.GetAxis("Vertical");
			return new Vector2(x, -y).normalized;
		});
	}

	void Start () {
		
	}
}
