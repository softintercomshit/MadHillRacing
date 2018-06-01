using UnityEngine;
using System.Collections;

public class SmoothFollow2D : MonoBehaviour
{

	private Vector3 velocity = Vector3.zero;
	Transform target;
	[Space (3)]
	public float startDelay = 0.03f;
	public string targetTag = "Player";

	public Vector2 position = new Vector2 (0.3f, 0.5f);
	public bool shouldFollow = true;

	private static SmoothFollow2D instance;

	public static SmoothFollow2D Instance {
		get { 
			if (instance == null) {
				instance = GameObject.FindObjectOfType<SmoothFollow2D> ();
			}
			return SmoothFollow2D.instance; 
		}
	}


	IEnumerator Start ()
	{
		yield return new WaitForEndOfFrame ();
		target = GameObject.FindGameObjectWithTag (targetTag).transform;
	}

	void LateUpdate ()
	{
		if (target && shouldFollow) {
			Vector3 point = GetComponent<Camera> ().WorldToViewportPoint (target.position);
			Vector3 delta = target.position - GetComponent<Camera> ().ViewportToWorldPoint (new Vector3 (position.x, position.y, point.z)); //(new Vector3(0.5, 0.5, point.z));

			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, 0);
		}
	}

	public void StopFollowingTarget ()
	{
		shouldFollow = false;
	}
}