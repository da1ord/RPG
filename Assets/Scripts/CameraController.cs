<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 cameraOffset_ = new Vector3( 0.0f, 0.0f, -5.0f );
    Vector3 battleCameraPosition_ = new Vector3( -20.0f, 0.0f, -5.0f );

    // Use this for initialization
    void Start()
    {
	}
	
	// Update is called once per frame
	public void MovePosition( Vector3 position )
    {
        transform.position = Vector3.Lerp( transform.position, position, 0.1f ) + cameraOffset_;	
	}

    public void BattleCamera()
    {
        transform.position = battleCameraPosition_;

        // Move camera closer to player
        Camera.main.orthographicSize = 4;
    }

    public void MapCamera()
    {
        // Move camera further from player
        Camera.main.orthographicSize = 6;
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {
	}
	
	// Update is called once per frame
	public void MovePosition( Vector3 position )
    {
        transform.position = Vector3.Lerp( transform.position, position, 0.1f ) + new Vector3( 0, 0, -10 );	
	}

    public void BattleCamera()
    {
        transform.position = new Vector3( -20.0f, 0.0f, -5.0f );

        // Move camera closer to player
        Camera.main.orthographicSize = 4;
    }

    public void MapCamera()
    {
        // Move camera further from player
        Camera.main.orthographicSize = 6;
    }
}
>>>>>>> b26a507af31452553c716ea355cbdf2c1484bb42
