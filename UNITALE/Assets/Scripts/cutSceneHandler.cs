using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cutSceneHandler : MonoBehaviour
{
    public GameObject dialogueBox;
    private void Start()
    {
        StartCoroutine(childSequence());
    }
    private IEnumerator childSequence()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Deactivate();
            transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitUntil(() => transform.GetChild(i).GetComponent<cutSceneDialogue>().nextSet);
        }
        gameObject.SetActive(false);
        dialogueBox.SetActive(false);
    }
    
    private void Deactivate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
