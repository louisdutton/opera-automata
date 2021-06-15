using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private TextAsset dictionaryFile;
    [SerializeField] private TextAsset maleNamesFile;
    [SerializeField] private TextAsset femaleNamesFile;

    public static Dictionary<string, string> dictionary = new Dictionary<string, string>();
    public static NameList names = new NameList();
    public float checkRate = .5f;

    private bool dictionaryLoaded = false;
    private bool namesLoaded = false;

    private void Start()
    {
        StartCoroutine(LoadNames());
        StartCoroutine(LoadDictionary());
        StartCoroutine(CheckLoadingComplete());
    }

    private IEnumerator CheckLoadingComplete()
    {
        while (!(dictionaryLoaded && namesLoaded))
        {
            yield return new WaitForSeconds(checkRate);
        }

        EventManager.instance.DataLoaded();
    }

    private IEnumerator LoadDictionary()
    {
        string[] lines = dictionaryFile.text.Split('\n');
        foreach (string line in lines)
        {
            string[] keyValue = line.Split(';');
            if (!dictionary.ContainsKey(keyValue[0])) dictionary.Add(keyValue[0], keyValue[1]);
        }

        print("Dictionary Loaded");
        dictionaryLoaded = true;
        yield return null;
    }

    private IEnumerator LoadNames()
    {
        names.male = maleNamesFile.text.Split('\n');
        names.female = femaleNamesFile.text.Split('\n');
        print("Names Loaded");
        namesLoaded = true;
        yield return null;
    }

    public struct NameList
    {
        public string[] male;
        public string[] female;
    }
}
