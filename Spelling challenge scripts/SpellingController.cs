using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Wikitude;
using UnityEngine.UI;

[System.Serializable]
public class WordList
{
    public string word;
    public Texture prompt;
}

public class SpellingController : MonoBehaviour
{
    public GameObject trackable;
    public GameObject tick;
    public GameObject cross;

    public GameObject claire;
    bool gotReward = false;

    public string word = "";

    public List<WordList> words;
    public Texture gameover;
    public RawImage screenPrompt;
    public Text score;

    int currentWord = -1;

    private void Start()
    {
        PickRandomWord();
    }


    public void PickRandomWord()
    {
        if (currentWord > -1) words.RemoveAt(currentWord);
        currentWord = Random.Range(0, words.Count);
        word = words[currentWord].word;
        screenPrompt.texture = words[currentWord].prompt;
    }

    public void OnImageRecognized(ImageTarget target)
    {
        Invoke("CheckSpelling", 0.1f);
    }

    public void OnImageLost(ImageTarget target)
    {
        if (gotReward)
        {
            if (words.Count == 1)
            {
                screenPrompt.texture = gameover;
                trackable.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                PickRandomWord();
                gotReward = false;
            }
        }
    }

    void CheckSpelling()
    {
        Transform[] allTransforms = trackable.GetComponentsInChildren<Transform>();
        List<Transform> markers = new List<Transform>();
        foreach (Transform t in allTransforms)
        {
            if(t.parent.gameObject == trackable)
            {
                markers.Add(t);
            }
        }
        if (markers.Count != word.Length)
        {
            gotReward = false;
            return;
        };
        markers = markers.OrderByDescending(marker => marker.position.x).ToList();
        int countCorrect = 0;
        for(int i = 0; i< markers.Count; i++)
        {
            if (markers[i].gameObject.name.StartsWith(word[i] + "_"))
            {
                countCorrect++;
                GameObject tickObj = Instantiate(tick, markers[i].position, markers[i].rotation);
                tickObj.transform.parent = markers[i];
            }
            else
            {
                GameObject crossObj = Instantiate(cross, markers[i].position, markers[i].rotation);
                crossObj.transform.parent = markers[i];
            }
        }
        if (countCorrect == word.Length && !gotReward)
        {
            GameObject c = Instantiate(claire, markers[0].position, markers[0].rotation);
            c.transform.parent = markers[0];
            gotReward = true;
            score.text = (int.Parse(score.text) + 1).ToString();
        }
        else
        {
        }
    }
}
