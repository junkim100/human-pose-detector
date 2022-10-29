using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JustWithJoints.Avatars
{
    public class MixamoAvatar : MonoBehaviour
    {
        public GameObject PointListAnnotation;
        public string DataPath = "Assets/Avatar/movementUpdate.txt";
        public GameObject MotionProvider;
        public bool EnableRetargetting = true;
        public bool RetargetTranslation = true;

        List<GameObject> joints_ = new List<GameObject>();
        List<GameObject> bones_ = new List<GameObject>();
        Vector3[] correctionRightEulers = new Vector3[13];

        
        private int frame = 0;
        private string[] joints = {
                                "rtibia", //26
                                "rfemur", //24
                                "rhipjoint", //24
                                "lhipjoint", //25
                                "lfemur", //25
                                "ltibia", //27 //Knee
                                "rwrist", //16
                                "rhumerus", //14
                                "rclavicle", //12
                                "lclavicle", //13
                                "lhumerus", //15
                                "lwrist", //17
                                "thorax", // 12+13+24+25/4
                                "head" //1
                            };

        void Start()
        {
            var root = gameObject;

            string prefix = "";
            for (int i = 0; i < root.gameObject.transform.childCount; i++)
            {
                var child = root.gameObject.transform.GetChild(i);
                if (child.name.EndsWith("Hips"))
                {
                    prefix = child.name.Substring(0, child.name.Length - 4);
                }
            }

            var hips = root.gameObject.transform.Find(prefix + "Hips").gameObject;
            var rightUpLeg = hips.transform.Find(prefix + "RightUpLeg").gameObject;
            var rightLeg = rightUpLeg.transform.Find(prefix + "RightLeg").gameObject;
            var rightFoot = rightLeg.transform.Find(prefix + "RightFoot").gameObject;
            var rightToeBase = rightFoot.transform.Find(prefix + "RightToeBase").gameObject;
            var leftUpLeg = hips.transform.Find(prefix + "LeftUpLeg").gameObject;
            var leftLeg = leftUpLeg.transform.Find(prefix + "LeftLeg").gameObject;
            var leftFoot = leftLeg.transform.Find(prefix + "LeftFoot").gameObject;
            var leftToeBase = leftFoot.transform.Find(prefix + "LeftToeBase").gameObject;
            var spine = hips.transform.Find(prefix + "Spine").gameObject;
            var spine1 = spine.transform.Find(prefix + "Spine1").gameObject;
            var spine2 = spine1.transform.Find(prefix + "Spine2").gameObject;
            var leftShoulder = spine2.transform.Find(prefix + "LeftShoulder").gameObject;
            var leftArm = leftShoulder.transform.Find(prefix + "LeftArm").gameObject;
            var leftForeArm = leftArm.transform.Find(prefix + "LeftForeArm").gameObject;
            var leftHand = leftForeArm.transform.Find(prefix + "LeftHand").gameObject;
            var rightShoulder = spine2.transform.Find(prefix + "RightShoulder").gameObject;
            var rightArm = rightShoulder.transform.Find(prefix + "RightArm").gameObject;
            var rightForeArm = rightArm.transform.Find(prefix + "RightForeArm").gameObject;
            var rightHand = rightForeArm.transform.Find(prefix + "RightHand").gameObject;
            var neck = spine2.transform.Find(prefix + "Neck").gameObject;
            var head = neck.transform.Find(prefix + "Head").gameObject;

            bones_.Clear();
            bones_.AddRange(new GameObject[]
            {
                hips,
                rightUpLeg,
                rightLeg,
                leftUpLeg,
                leftLeg,
                spine,
                leftShoulder,
                leftArm,
                leftForeArm,
                rightShoulder,
                rightArm,
                rightForeArm,
                neck,
            });


            var invRoot = Quaternion.Inverse(gameObject.transform.rotation);
            string[] lines = System.IO.File.ReadAllLines("Assets/JustWithJoints/MotionData/unitychan_inv_tpose.txt");
            for (int i = 0; i < correctionRightEulers.Length; i++)
            {
                var tokens = lines[i].Split(' ');
                float x = float.Parse(tokens[0]);
                float y = float.Parse(tokens[1]);
                float z = float.Parse(tokens[2]);
                var inv = Quaternion.Euler(x, y, z);
                var rot = inv * bones_[i].transform.rotation * invRoot;
                correctionRightEulers[i] = rot.eulerAngles;
            }
        }

        void LateUpdate()
        {
            Transform[] allChildren = PointListAnnotation.transform.GetComponentsInChildren<Transform>();
            movementUpdate(DataPath, allChildren);

            // Get pose
            Core.Pose pose = null;
            if (MotionProvider)
            {
                var component = MotionProvider.GetComponent<CMUMotionPlayer>();
                if (component)
                {
                    pose = component.GetCurrentPose();
                }
            }

            // Retarget
            if (pose != null)
            {
                // Retarget positions
                if (RetargetTranslation)
                {
                    bones_[0].transform.position = (pose.Positions[2] + pose.Positions[3]) * 0.5f + gameObject.transform.position;
                }

                if (EnableRetargetting)
                {
                    // Retarget rotations
                    for (int i = 0; i < 13; i++)
                    {
                        int boneIndex = i;
                        if (i == (int)Core.BoneType.Trans)
                        {
                            // Use spine as root of avatar
                            boneIndex = (int)Core.BoneType.Spine;
                        }
                        if (i == (int)Core.BoneType.Spine)
                        {
                            // Skip spine
                            continue;
                        }
                        if (i == (int)Core.BoneType.RightShoulder || i == (int)Core.BoneType.LeftShoulder)
                        {
                            // Skip shoulders
                            continue;
                        }
                        bones_[i].transform.rotation =
                            gameObject.transform.rotation *
                            pose.Rotations[boneIndex] *
                            Quaternion.Euler(correctionRightEulers[i]);
                    }
                }
            }
        }


        private void movementUpdate(string DataPath, Transform[] allChildren) {
            // Clear text file
            var writer = new System.IO.StreamWriter(DataPath, false);
            writer.WriteLine("");

            if (allChildren.Length >= 10) {
                writer.WriteLine(string.Format("frame: {0}", frame));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[0], allChildren[26].transform.localPosition.x, allChildren[26].transform.localPosition.y, allChildren[26].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[1], allChildren[24].transform.localPosition.x, allChildren[24].transform.localPosition.y, allChildren[24].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[2], allChildren[24].transform.localPosition.x, allChildren[24].transform.localPosition.y, allChildren[24].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[3], allChildren[25].transform.localPosition.x, allChildren[25].transform.localPosition.y, allChildren[25].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[4], allChildren[25].transform.localPosition.x, allChildren[25].transform.localPosition.y, allChildren[25].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[5], allChildren[27].transform.localPosition.x, allChildren[27].transform.localPosition.y, allChildren[27].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[6], allChildren[16].transform.localPosition.x, allChildren[16].transform.localPosition.y, allChildren[16].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[7], allChildren[14].transform.localPosition.x, allChildren[14].transform.localPosition.y, allChildren[14].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[8], allChildren[12].transform.localPosition.x, allChildren[12].transform.localPosition.y, allChildren[12].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[9], allChildren[13].transform.localPosition.x, allChildren[13].transform.localPosition.y, allChildren[13].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[10], allChildren[15].transform.localPosition.x, allChildren[15].transform.localPosition.y, allChildren[15].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[11], allChildren[17].transform.localPosition.x, allChildren[17].transform.localPosition.y, allChildren[17].transform.localPosition.z));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[12], (allChildren[12].transform.localPosition.x+allChildren[13].transform.localPosition.x+allChildren[24].transform.localPosition.x+allChildren[25].transform.localPosition.x)/4, (allChildren[12].transform.localPosition.y+allChildren[13].transform.localPosition.y+allChildren[24].transform.localPosition.y+allChildren[25].transform.localPosition.y)/4, (allChildren[12].transform.localPosition.z+allChildren[13].transform.localPosition.z+allChildren[24].transform.localPosition.z+allChildren[25].transform.localPosition.z)/4));
                writer.WriteLine(string.Format("\t{0}: {1} {2} {3}", joints[13], allChildren[1].transform.localPosition.x, allChildren[1].transform.localPosition.y, allChildren[1].transform.localPosition.z));
                frame++;
            }

            writer.Close();
        }
    }
}