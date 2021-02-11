using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
public class IkPhonePlacement : MonoBehaviour
{
    public GameObject PhonePrefab;
    public Text heightUI;
    private GameObject placedObj;
    public float distance = 2.0f;


    public void InstantiateModel()
    {
        Transform cam = Camera.main.transform;
        Vector3 pos = cam.position + cam.forward;
        pos.y = cam.position.y;
        pos = (pos - cam.position).normalized * distance;
        placedObj = Instantiate(PhonePrefab, cam.position + pos, Quaternion.identity);
        placedObj.transform.LookAt(cam.position, Vector3.up);
        RigBuilder rigBuilder = placedObj.GetComponentInChildren<RigBuilder>();
        var humanoidControllerRef = rigBuilder.GetComponent<HumanoidController>();
        humanoidControllerRef.SetDependecies(placedObj.GetComponentInChildren<BoundsManager>(), placedObj.transform.GetChild(0), placedObj.transform.GetChild(1));        
        rigBuilder.transform.parent = null;
        rigBuilder.enabled = true;
        placedObj.GetComponentInChildren<MirrorTransform>().enabled = true;
        //ArUxManager.instance.HideInstruction();
    }

    private void Update()
    {
        if(placedObj!=null)
        {

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                InstantiateModel();
            }
        }
      
    }
}
