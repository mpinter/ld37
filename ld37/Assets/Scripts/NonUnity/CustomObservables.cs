using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace flyyoufools {
	public static class CustomObservables {

		public static IObservable<bool> Latch(IObservable<Unit> tick, IObservable<Unit> latchTrue, bool initialValue) {
			// Create a custom Observable, whose behavior is determined by our calls to the provided 'observable'
			return Observable.Create<bool>(observer => {
				// Our state value.
				var value = initialValue;

				// Create an inner subscription to latch:
				// Whenever latch fires, store true.
				var latchSub = latchTrue.Subscribe(_ => value = true);

				// Create an inner subscription to tick:
				var tickSub = tick.Subscribe(
					// Whenever tick fires, send the current value and reset state.
					_ => {
						observer.OnNext(value);
						value = false;
					},
					observer.OnError, // pass through tick's errors (if any)
					observer.OnCompleted); // complete when tick completes

				// If we're disposed, dispose inner subscriptions too.
				return Disposable.Create(() => {
					latchSub.Dispose();
					tickSub.Dispose();
				});
			});
		}
	}
}