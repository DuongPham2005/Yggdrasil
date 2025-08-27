using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


    public class MainStory : MonoBehaviour
    {
        void OnEnable()
        {
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }
    }

