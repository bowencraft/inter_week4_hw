using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jumpScript : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.S))
        {
            player.GetComponent<PlayerControl>().resetHealth();
            SceneManager.LoadScene("Level1");
        }
    }
}
