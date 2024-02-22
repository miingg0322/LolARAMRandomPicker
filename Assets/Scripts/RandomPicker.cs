using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RandomPicker : MonoBehaviour
{
    private static RandomPicker instance;
    public static RandomPicker Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

    private List<Sprite> originChamps = new List<Sprite>();
    private List<Sprite> pickList = new List<Sprite>();
    private List<Image> blueTeam = new List<Image>();
    private List<Image> purpleTeam = new List<Image>();
    public int champCount = 10;
    public Sprite basicSprite;

    public Sprite basicRerollSprite;
    public Image selectedChamp;

    public GameObject trashBinContent;
    public GameObject trashPrefab;

    public Image selectedPortrait;
    private bool isPicked = false;
    public Button pickButton;
    public Button resetButton;
    public Text champCountText;

    public float delay = 0.25f;

    public Effect effector;

    private void Awake()
    {
        if (instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        Screen.SetResolution(1440, 810, FullScreenMode.Windowed);
        originChamps.AddRange(Resources.LoadAll<Sprite>("Portraits"));
        GameObject[] blue = GameObject.FindGameObjectsWithTag("Blue");
        foreach (var go in blue)
        {
            blueTeam.Add(go.GetComponent<Image>());
        }
        blueTeam.Sort(TeamComparer);
        GameObject[] purple = GameObject.FindGameObjectsWithTag("Purple");
        foreach (var go in purple)
        {
            purpleTeam.Add(go.GetComponent<Image>());
        }
        purpleTeam.Sort(TeamComparer);
        pickList = originChamps.ToList();
        champCountText.text = $"{pickList.Count} / {originChamps.Count}";
        effector = GetComponent<Effect>();
    }
    private int TeamComparer(Image img1, Image img2)
    {
        return img1.transform.parent.name.CompareTo(img2.transform.parent.name);
    }
    public void RandomPick()
    {
        if (pickList.Count > champCount * 2)
        {
            SetInteractable(false);
            if (isPicked)
            {
                InitRerollPortrait();
                StartCoroutine(DeleteCoroutine(delay));
            }
            StartCoroutine(PickCoroutine(delay));
            isPicked = true;
        }
    }

    public void RandomPickOne(Image selectedPortrait)
    {
        int randIdx = Random.Range(0, pickList.Count);
        selectedPortrait.sprite = pickList[randIdx];
        pickList.RemoveAt(randIdx);
        champCountText.text = $"{pickList.Count} / {originChamps.Count}";
    }
    public void Reset()
    {
        isPicked = false;
        pickList.Clear();
        pickList = originChamps.ToList();
        InitRerollPortrait();
        champCountText.text = $"{pickList.Count} / {originChamps.Count}";
        for (int i = 0; i < champCount; i++)
        {
            blueTeam[i].sprite = basicSprite;
            blueTeam[i].gameObject.GetComponent<ChampPortrait>().isAssigned = false;
            purpleTeam[i].sprite = basicSprite;
            purpleTeam[i].gameObject.GetComponent<ChampPortrait>().isAssigned = false;
        }
        GameObject[] allTrash = GameObject.FindGameObjectsWithTag("Trash");
        foreach (GameObject trash in allTrash)
        {
            Destroy(trash);
        }
    }

    public void OnRerollBtnClicked()
    {
        if (selectedChamp.sprite != basicRerollSprite)
        {
            if (pickList.Count > 0)
            {
                InstantiateTrash(selectedChamp.sprite);
                InitRerollPortrait();
                RandomPickOne(selectedPortrait);
                selectedPortrait = null;
                
            }
        }
    }
    public void InitRerollPortrait()
    {
        selectedChamp.sprite = basicRerollSprite;
        effector.StopFadeEffect(selectedChamp);
    }
    IEnumerator PickCoroutine(float delay)
    {
        int randIdx = 0;
        for (int i = 0; i < champCount; i++)
        {
            randIdx = Random.Range(0, pickList.Count);
            blueTeam[i].sprite = pickList[randIdx];
            blueTeam[i].gameObject.GetComponent<ChampPortrait>().isAssigned = true;
            pickList.RemoveAt(randIdx);
            effector.PickEffect(blueTeam[i].transform.parent.gameObject);
            yield return new WaitForSeconds(delay);
            champCountText.text = $"{pickList.Count} / {originChamps.Count}";
            randIdx = Random.Range(0, pickList.Count);
            purpleTeam[i].sprite = pickList[randIdx];
            purpleTeam[i].gameObject.GetComponent<ChampPortrait>().isAssigned = true;
            pickList.RemoveAt(randIdx);
            effector.PickEffect(purpleTeam[i].transform.parent.gameObject);
            yield return new WaitForSeconds(delay);
            champCountText.text = $"{pickList.Count} / {originChamps.Count}";
        }
        SetInteractable(true);
    }

    IEnumerator DeleteCoroutine(float delay)
    {
        for (int i = 0; i < blueTeam.Count; i++)
        {
            InstantiateTrash(blueTeam[i].sprite);
            yield return new WaitForSeconds(delay);
            InstantiateTrash(purpleTeam[i].sprite);
            yield return new WaitForSeconds(delay);
        }
    }

    private void SetInteractable(bool interactable)
    {
        pickButton.interactable = interactable;
        resetButton.interactable = interactable;
        for (int i = 0; i < champCount; i++)
        {
            blueTeam[i].GetComponent<Button>().interactable = interactable;
            purpleTeam[i].GetComponent<Button>().interactable = interactable;
        }
    }
    private void InstantiateTrash(Sprite sprite)
    {
        GameObject trash = Instantiate(trashPrefab);
        trash.transform.SetParent(trashBinContent.transform);        
        trash.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        effector.InstantiateEffect(trash, transform.root.localScale);
    }
}