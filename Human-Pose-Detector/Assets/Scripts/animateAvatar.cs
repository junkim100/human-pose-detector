using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateAvatar : MonoBehaviour
{
    public GameObject Model;
    public GameObject PointListAnnotation;

    void Update()
    {
        Transform[] bodyParts = Model.transform.GetComponentsInChildren<Transform>();
        Transform[] allChildren = PointListAnnotation.transform.GetComponentsInChildren<Transform>();

        // for (int i = 0; i < allChildren.Length; i++) {
        //     Debug.Log(allChildren[i].name);
        // }

        // if (allChildren.Length != 1)
        //     placeBodyParts(bodyParts, allChildren);


        // bodyParts[0].transform.localPosition = (allChildren[24].transform.localPosition + allChildren[25].transform.localPosition)/2;
        // Debug.Log((allChildren[24].transform.localPosition + allChildren[25].transform.localPosition)/2);
        Debug.Log(bodyParts[0].transform.position);
    }

    private void placeBodyParts(Transform[] bodyParts, Transform[] allChildren) {
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

        rotateBodyParts(allChildren[13], allChildren[15], bodyParts[18]);
        rotateBodyParts(allChildren[15], allChildren[17], bodyParts[19]);
        rotateBodyParts(allChildren[17], allChildren[21], bodyParts[20]);

        // Right Arm
        bodyParts[45].transform.position = allChildren[12].transform.position;
        bodyParts[46].transform.position = allChildren[14].transform.position;
        bodyParts[47].transform.position = allChildren[16].transform.position;

        rotateBodyParts(allChildren[12], allChildren[14], bodyParts[45]);
        rotateBodyParts(allChildren[14], allChildren[16], bodyParts[46]);
        rotateBodyParts(allChildren[16], allChildren[20], bodyParts[47]);

        // Right Leg
        bodyParts[9].transform.position = allChildren[24].transform.position;
        bodyParts[10].transform.position = allChildren[26].transform.position;
        bodyParts[11].transform.position = allChildren[28].transform.position;
        bodyParts[13].transform.position = allChildren[32].transform.position;

        rotateBodyParts(allChildren[24], allChildren[26], bodyParts[9]);
        rotateBodyParts(allChildren[26], allChildren[28], bodyParts[10]);
        rotateBodyParts(allChildren[28], allChildren[32], bodyParts[11]);
        // rotateBodyParts(allChildren[32], allChildren[20], bodyParts[13]);

        // // Left Leg
        bodyParts[4].transform.position = allChildren[25].transform.position;
        bodyParts[5].transform.position = allChildren[27].transform.position;
        bodyParts[6].transform.position = allChildren[29].transform.position;
        bodyParts[8].transform.position = allChildren[33].transform.position;

        rotateBodyParts(allChildren[25], allChildren[27], bodyParts[4]);
        rotateBodyParts(allChildren[27], allChildren[29], bodyParts[5]);
        rotateBodyParts(allChildren[29], allChildren[33], bodyParts[6]);
        // rotateBodyParts(allChildren[33], allChildren[20], bodyParts[8]);

        // Head
        bodyParts[42].transform.position = allChildren[1].transform.position;
    }

    private void rotateBodyParts(Transform part1, Transform part2, Transform bodyPart) {
        Vector3 point1 = part1.position;
        Vector3 point2 = part2.position;
        Vector3 rotationVector = point1 - point2;
        Debug.Log(rotationVector);
        // rotationVector.x = rotationVector.x*2;
        Debug.Log(Quaternion.LookRotation(rotationVector));
        bodyPart.rotation = Quaternion.LookRotation(rotationVector);
        bodyPart.Rotate(270, 0, 0);
    }

}
