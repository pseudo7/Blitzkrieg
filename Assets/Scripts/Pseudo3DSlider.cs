using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pseudo3DSlider : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
    [SerializeField] Button playButton;
    [SerializeField] Text chopperName;
    [SerializeField] Transform slotTransform;
    [SerializeField] Vector3 prefabRotation = new Vector3(0, 120, 0);
    [SerializeField] float slotSize = 15f, slidingSpeed = 10f;
    static int currentIndex;

    List<GameObject> spawnedPrefabs;
    bool isSliding;
    float limit;

    private void Start()
    {
        spawnedPrefabs = new List<GameObject>();
        var spawnRotation = Quaternion.Euler(prefabRotation);
        int i;
        for (i = 0; i < prefabs.Length; i++)
            spawnedPrefabs.Add(Instantiate(prefabs[i], Vector3.up + Vector3.right * i * slotSize, spawnRotation, slotTransform));

        limit = (i - 1) * -slotSize;
        chopperName.text = prefabs[currentIndex].name;
        spawnedPrefabs[currentIndex].GetComponent<EngineController>().engineOn = true;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftArrow) && slotTransform.position.x > limit)
            Slide(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow) && slotTransform.position.x < 0)
            Slide(1);
#else
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            float deltaX;
            if (Mathf.Abs(deltaX = touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
                if (deltaX < 0 && slotTransform.position.x > limit)
                    Slide(-1);
                else if (deltaX > 0 && slotTransform.position.x < 0)
                    Slide(1);
        }
#endif
    }

    private void Slide(float deltaX)
    {
        if (!isSliding)
            StartCoroutine(Sliding(deltaX < 0));
    }

    IEnumerator Sliding(bool slideLeft)
    {
        isSliding = true;
        Vector3 nextPos = slideLeft ? Vector3.left : Vector3.right;
        nextPos *= slotSize;
        nextPos += slotTransform.position;

        playButton.interactable = false;
        spawnedPrefabs[currentIndex].GetComponent<EngineController>().engineOn = false;

        currentIndex += slideLeft ? 1 : -1;

        chopperName.text = prefabs[currentIndex].name;

        while (slideLeft ? slotTransform.position.x > nextPos.x : slotTransform.position.x < nextPos.x)
        {
            slotTransform.position = Vector3.MoveTowards(slotTransform.position, nextPos, Time.deltaTime * slidingSpeed);
            yield return new WaitForEndOfFrame();
        }
        spawnedPrefabs[currentIndex].GetComponent<EngineController>().engineOn = true;
        playButton.interactable = true;

        isSliding = false;
    }

    public void PlayGame()
    {
        DDOL_Navigation.SelectedChopper = prefabs[currentIndex];
        DDOL_Navigation.Instance.LoadGame();
    }
}