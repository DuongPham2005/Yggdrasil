using UnityEngine;

namespace IdyllicFantasyNature
{
    public class ExitGame : MonoBehaviour
    {
        [SerializeField] private KeyCode quitKey = KeyCode.F10;

        void Update()
        {
            // Only quit when a dedicated key is pressed, to avoid conflicts with settings menu
            if (Input.GetKeyDown(quitKey))
            {
                Application.Quit();
            }
        }
    }
}
