using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;


// This simple class encapsulates the UI for
// the scenes so that the game manager need 
// only reference one thing to control the UI.
public class UIController : Singleton<UIController>
{
    [SerializeField] private UIFader m_IntroUI;                     // This controls fading the UI shown during the intro.
    [SerializeField] private UIFader m_InfoUI;                      // This controls fading the UI shown during Info mode.
    [SerializeField] private UIFader m_PlayerUI;                    // This controls fading the UI shown during Minigame mode.
    [SerializeField] private UIFader m_OutroUI;                     // This controls fading the UI shown during the outro.
    [SerializeField] private UIFader m_CheckList;                   // This controls fading the UI for player check list

    public bool Showing { get; set; }                               // If a UI is currently showing

    public IEnumerator ShowChecklist()
    {
        yield return StartCoroutine(m_CheckList.InteruptAndFadeIn());
    }

    public IEnumerator HideChecklist()
    {
        yield return StartCoroutine(m_CheckList.InteruptAndFadeOut());
    }

    public IEnumerator ShowIntroUI()
    {
        Showing = true;
        yield return StartCoroutine(m_IntroUI.InteruptAndFadeIn());
    }

    public IEnumerator HideIntroUI()
    {
        Showing = false;
        yield return StartCoroutine(m_IntroUI.InteruptAndFadeOut());
    }

    public IEnumerator ShowInfoUI()
    {
        // Default would be not commented
        // This is temporary because of requirements that
        // if a user see another animal, the user can select that animal
        // and the info will change.
        //Showing = true;
        yield return StartCoroutine(m_InfoUI.InteruptAndFadeIn());
    }


    public IEnumerator HideInfoUI()
    {
        // Default would be not commented
        // This is temporary because of requirements that
        // if a user see another animal, the user can select that animal
        // and the info will change.
        //Showing = false;
        yield return StartCoroutine(m_InfoUI.InteruptAndFadeOut());
    }

    public IEnumerator ShowPlayerUI()
    {
        yield return StartCoroutine(m_PlayerUI.InteruptAndFadeIn());
    }

    public IEnumerator HidePlayerUI()
    {
        yield return StartCoroutine(m_PlayerUI.InteruptAndFadeOut());
    }

    public IEnumerator ShowOutroUI()
    {
        yield return StartCoroutine(m_OutroUI.InteruptAndFadeIn());
    }

    public IEnumerator HideOutroUI()
    {
        yield return StartCoroutine(m_OutroUI.InteruptAndFadeOut());
    }
}