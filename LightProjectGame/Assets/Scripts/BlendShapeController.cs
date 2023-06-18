using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// blendshape helper script von Markus
/// </summary>

public class BlendShapeController : MonoBehaviour
{
    //bitte reihen folge shoulder
    public Transform leftShoulderTransform;
    public Transform rightShoulderTransform;
    
    public Transform spineTransform;
    public Transform spine1Transform;
    public Transform spine2Transform;

    public Transform leftshoulderRef;
    public Transform rightshoulderRef;

    private SkinnedMeshRenderer skinned;

    private float[] blendWeights;
    private Quaternion rightShoulderRefQ;
    private Quaternion leftShoulderRefQ;


    // Start is called before the first frame update
    void Start()
    {
        skinned = GetComponent<SkinnedMeshRenderer>();

        int blendshapeCount = skinned.sharedMesh.blendShapeCount;

        // Float-Array mit entsprechender Größe erstellen
        blendWeights = new float[blendshapeCount];

        print(leftShoulderTransform.localRotation + " 1 Leftshoulder rotation");
        print(rightShoulderTransform.localRotation + " 1 rightshoulder rotation");
        print(spine1Transform.localRotation + " 1 spine1 rotation");

        rightShoulderRefQ = Quaternion.Inverse(rightshoulderRef.localRotation);
        leftShoulderRefQ = Quaternion.Inverse(leftshoulderRef.localRotation);

    }

    // Update is called once per frame
    void Update()
    {
        //Blender   X Front Y Side Z Top
        //Unity     Z Font X Side Y Top
        //Update blendshapes
        //Leftshoulder X Minus Rotation
        /*print(leftShoulderTransform.rotation+ " Leftshoulder rotation");
        print(rightShoulderTransform.rotation + " rightshoulder rotation");
        print(spine1Transform.rotation + " spine1 rotation");*/
        Quaternion leftDifference = leftShoulderRefQ * leftShoulderTransform.localRotation;
        Quaternion rightDifference = rightShoulderRefQ * rightShoulderTransform.localRotation;



        if (blendWeights.Length == 18)
        {

            blendWeights[0] = Mathf.Clamp(((leftDifference.x) * 9 * 100), 0, 100);
            blendWeights[1] = Mathf.Clamp((((-leftDifference.y) / 3) * (leftDifference.z) * 2 * 100), 0, 100);
            blendWeights[2] = Mathf.Clamp(((-leftDifference.x) * 4 * 100), 0, 100);
            blendWeights[3] = Mathf.Clamp(((rightDifference.x) * 4 * 100), 0, 100);
            blendWeights[4] = Mathf.Clamp(((-rightDifference.y) * 5f * 100), 0, 100);
            blendWeights[5] = Mathf.Clamp(((-rightDifference.x) * 4f *100), 0, 100);
            //if ()
            blendWeights[6] = Mathf.Clamp(((spine1Transform.localRotation.y + spineTransform.localRotation.y + spine2Transform.localRotation.y) * 3f * 100), 0, 100);
            blendWeights[7] = Mathf.Clamp(((-spine1Transform.localRotation.y + -spineTransform.localRotation.y + -spine2Transform.localRotation.y) * 3f * 100), 0, 100);

            blendWeights[8] = Mathf.Clamp(((-spineTransform.localRotation.z + -spine1Transform.localRotation.z + -spine2Transform.localRotation.z)*2.7f*100), 0, 100);
            blendWeights[9] = Mathf.Clamp(((spineTransform.localRotation.z + spine1Transform.localRotation.z + spine2Transform.localRotation.z) * 2.7f * 100), 0, 100);
            print(spineTransform.localRotation + " " + spine1Transform.localRotation + " " + spine2Transform.localRotation + "Weight: " + blendWeights[9]);
            print(-spineTransform.localRotation.x + "+" + -spine1Transform.localRotation.x + "+" + -spine2Transform.localRotation.x + "=" + blendWeights[9]);

            blendWeights[10] = Mathf.Clamp(((spineTransform.localRotation.x + spine1Transform.localRotation.x + spine2Transform.localRotation.x) * 2 * 100), 0, 100);

            blendWeights[11] = Mathf.Clamp(((-leftDifference.z) * 3 * 100), 0, 100);
            blendWeights[12] = Mathf.Clamp(((leftDifference.z) * 2.7f * 100), 0, 100);
            blendWeights[13] = Mathf.Clamp(((rightDifference.z) * 3 * 100), 0, 100);
            blendWeights[14] = Mathf.Clamp(((-rightDifference.z) * 5 * 100), 0, 100);

            blendWeights[15] = Mathf.Clamp(((-spineTransform.localRotation.x + -spine1Transform.localRotation.x + -spine2Transform.localRotation.x) * 2 * 100), 0, 100);
            blendWeights[16] = Mathf.Clamp((-leftDifference.y * 2.5f), 0, 100);
            blendWeights[17] = Mathf.Clamp((-rightDifference.y * 2.5f), 0, 100);

            //Mathf.Clamp((),0,100)

            //set weights
            for (int i = 0; i < blendWeights.Length; i++)
            {
                skinned.SetBlendShapeWeight(i, blendWeights[i]);
                
            }

        } else
        {
            print("there is an unexpected amount of blendshapes for arin");
        }

    }
}
