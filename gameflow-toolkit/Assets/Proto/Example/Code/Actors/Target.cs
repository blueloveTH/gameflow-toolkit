using Proto;
using UnityEngine;

namespace Proto.Example
{
	public class Target : MonoBehaviour
	{
		// the target, a navmesh agent controlled by clicking
		//------------------------------------------------------

		UnityEngine.AI.NavMeshAgent agent;

		void Awake ()
		{
			agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		}

		void Update ()
		{
			if (Input.GetMouseButtonDown (0))
			{
				RaycastHit hit;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100f))
					agent.destination = hit.point;
			}
		}
	}
}