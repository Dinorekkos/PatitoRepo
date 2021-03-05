using UnityEngine;
using System.Collections;

public interface IMovingSystem {
	Transform transform {
		get;
    }
	Vector3 CurrentSpeed {
		get;
	}
}
