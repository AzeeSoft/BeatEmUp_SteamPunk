using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaCamera : MonoBehaviour
{
    public float smoothTime = 0.2f;
    public float zoomFactor = 1f;
    
    public float angle = 30;
    public float minDistance = 10;
    private float maxDistance = float.MaxValue;

    private Vector3 velocity;

    void Awake()
    {

    }

	// Use this for initialization
	void Start ()
	{
	    
	}

    void OnDrawGizmos()
    {

    }
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    CoverAllCharacters();
	}

    void CoverAllCharacters()
    {
        ArenaManager arenaManager = ArenaManager.Instance;

        if (arenaManager.characterModels.Count == 0)
        {
            return;
        }

        Bounds bounds = new Bounds(arenaManager.characterModels[0].transform.position, Vector3.zero);
        for (int i = 0; i < arenaManager.characterModels.Count; i++)
        {
            Vector3 playerPos = arenaManager.characterModels[i].transform.position;
            bounds.Encapsulate(playerPos);
        }

        Vector3 midPos = bounds.center;

        float boundSize = bounds.size.magnitude;
        float dist = minDistance + (boundSize * zoomFactor);

        float zDelta = dist * Mathf.Cos(Mathf.Deg2Rad * -angle);
        float yDelta = dist * Mathf.Sin(Mathf.Deg2Rad * -angle);

        Vector3 backward = -transform.forward;
        backward.y = 0;

        Vector3 finalPos = midPos + (backward * zDelta);
        finalPos.y -= yDelta;

        transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref velocity, smoothTime);

        midPos.x = transform.position.x;
        transform.LookAt(midPos);
    }
}
