using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateAvatar : MonoBehaviour
{
    public GameObject PointListAnnotation;
    public GameObject Avatar;

    void Start()
    {
        // Transform[] bodyParts = Avatar.transform.GetComponentsInChildren<Transform>();
        // for (int i = 0; i < bodyParts.Length; i++) {
        //     Debug.Log(bodyParts[i].name);
        // }

        // // Left Leg
        // Debug.Log(bodyParts[4].name);
        // Debug.Log(bodyParts[5].name);
        // Debug.Log(bodyParts[6].name);
        // Debug.Log(bodyParts[8].name);

        // // Right Leg
        // Debug.Log(bodyParts[9].name);
        // Debug.Log(bodyParts[10].name);
        // Debug.Log(bodyParts[11].name);
        // Debug.Log(bodyParts[13].name);

        // // Left Arm
        // Debug.Log(bodyParts[18].name);
        // Debug.Log(bodyParts[19].name);
        // Debug.Log(bodyParts[20].name);

        // // Left Finger
        // Debug.Log(bodyParts[21].name);
        // Debug.Log(bodyParts[37].name);

        // // Right Arm
        // Debug.Log(bodyParts[45].name);
        // Debug.Log(bodyParts[46].name);
        // Debug.Log(bodyParts[47].name);

        // // Right Finger
        // Debug.Log(bodyParts[48].name);
        // Debug.Log(bodyParts[64].name);

        // //Head
        // Debug.Log(bodyParts[42].name);
    }

    void Update()
    {
        Transform[] bodyParts = Avatar.transform.GetComponentsInChildren<Transform>();

        Transform[] allChildren = PointListAnnotation.transform.GetComponentsInChildren<Transform>();
        // for (int i = 0; i < allChildren.Length; i++) {
        //     Debug.Log(allChildren[i].name);
        // }

        if (allChildren.Length != 1)
            updateBodyParts(bodyParts, allChildren);

    }

    private void updateBodyParts(Transform[] bodyParts, Transform[] allChildren) {
        // Body
        if (bodyParts[42].transform.position != null) {
            bodyParts[3].transform.position = new Vector3(bodyParts[42].transform.position.x, bodyParts[42].transform.position.y-20.0f, bodyParts[42].transform.position.z);
        }
        // bodyParts[3].transform.position = new Vector3(  (bodyParts[4].transform.position.x+bodyParts[9].transform.position.x)/2,
        //                                                 (bodyParts[4].transform.position.y+bodyParts[9].transform.position.y)/2 + 1.0f,
        //                                                 (bodyParts[4].transform.position.z+bodyParts[9].transform.position.z)/2
        //                                              );

        // Left Arm
        bodyParts[18].transform.position = allChildren[13].transform.position;
        bodyParts[19].transform.position = allChildren[15].transform.position;
        bodyParts[20].transform.position = allChildren[17].transform.position;

        // Right Arm
        bodyParts[45].transform.position = allChildren[12].transform.position;
        bodyParts[46].transform.position = allChildren[14].transform.position;
        bodyParts[47].transform.position = allChildren[16].transform.position;

        // Right Leg
        bodyParts[9].transform.position = allChildren[24].transform.position;
        bodyParts[10].transform.position = allChildren[26].transform.position;
        bodyParts[11].transform.position = allChildren[28].transform.position;
        bodyParts[13].transform.position = allChildren[32].transform.position;

        // // Left Leg
        bodyParts[4].transform.position = allChildren[25].transform.position;
        bodyParts[5].transform.position = allChildren[27].transform.position;
        bodyParts[6].transform.position = allChildren[29].transform.position;
        bodyParts[8].transform.position = allChildren[33].transform.position;

        // Head
        bodyParts[42].transform.position = allChildren[1].transform.position;
    }

}
