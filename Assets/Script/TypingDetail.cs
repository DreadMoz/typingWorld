using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypingDetail : MonoBehaviour
{
    [SerializeField]
    private GameObject stage1;
    [SerializeField]
    private GameObject stage2;
    [SerializeField]
    private GameObject stage3;

    private DetailMenu menu1;
    private DetailMenu menu2;
    private DetailMenu menu3;

    // Start is called before the first frame update
    void Start()
    {
        menu1 = stage1.GetComponent<DetailMenu>();
        menu2 = stage2.GetComponent<DetailMenu>();
        menu3 = stage3.GetComponent<DetailMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show()
    {
        transform.position = new Vector3(700, 1500, transform.position.z);
        menu1.setStars();
        menu2.setStars();
        menu3.setStars();
    }

    public void hide()
    {
        transform.position = new Vector3(700, 1500, transform.position.z);
        menu1.resetStars();
        menu2.resetStars();
        menu3.resetStars();
    }
}
