using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaCamera : MonoBehaviour
{
    public List<PlayerModel> playerModels;

    public float speed = 3;
    
    public float angle = 30;
    public float minDistance = 10;
    public float maxDistance = 100;

    void Awake()
    {
        foreach (PlayerModel playerModel in playerModels)
        {
            playerModel.arenaCamera = this;
        }
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    float dist = minDistance;

	    float zDelta = dist * Mathf.Cos(Mathf.Deg2Rad * -angle);
	    float yDelta = dist * Mathf.Sin(Mathf.Deg2Rad * -angle);

	    Vector3 midPos = Vector3.zero;
	    for (int i = 0; i < playerModels.Count; i++)
	    {
	        midPos += playerModels[i].transform.position;
	    }

	    midPos /= playerModels.Count;

	    Vector3 finalPos = midPos;
	    finalPos.z -= zDelta;
	    finalPos.y -= yDelta;

        transform.position = Vector3.Lerp(transform.position, finalPos, speed * Time.fixedDeltaTime);

        transform.LookAt(midPos);
	}
}
