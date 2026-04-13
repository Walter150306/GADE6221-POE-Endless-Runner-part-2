using UnityEngine;

public class IceFloorLogic : MonoBehaviour
{
    [Header("Ice Floor Settings")]
    public float iceFloorSpeed = 5f;

    [Header("Delete Settings")]
    public float deleteZ = 66f;

    void Update()
    {
        transform.Translate(Vector3.forward * iceFloorSpeed * Time.deltaTime, Space.World);

        if (transform.position.z >= deleteZ)
        {
            Debug.Log("Deleting whole tile: " + transform.parent.gameObject.name);
            Destroy(transform.parent.gameObject);
        }
    }
}