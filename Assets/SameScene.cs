using UnityEngine;

public class SameScene : MonoBehaviour
{
    public Animator transition;

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("FirstNextButtonIsPush") == 1)
        {
            PlayerPrefs.SetInt("FirstNextButtonIsPush", 0);
            transition.SetTrigger("FirstButtonIsPush");
        }

        if (PlayerPrefs.GetInt("SecondNextButtonIsPush") == 1)
        {
            PlayerPrefs.SetInt("SecondNextButtonIsPush", 0);
            transition.SetTrigger("SecondButtonIsPush");
        }
    }
}
