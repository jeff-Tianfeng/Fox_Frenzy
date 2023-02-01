using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Title is used to manage the Minecraft inspired "Title" system.
/// <para/>
/// This systems provdies the ability to show some contextual information for the player,
/// in the form of a title and description.
/// <see>https://minecraft.fandom.com/wiki/Commands/title</see>
/// </summary>
public class Title : MonoBehaviour
{
    private static Title _instance;

    public static Title Instance { get { return _instance; } }

    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private int lifeTime = 3;

    [SerializeField]
    private float fadeTime = 0.2f;

    private CanvasGroup canvasGroup;

    private IEnumerator currentCoroutine;

    private float titleFontSize, descriptionFontSize;

    private bool coroutineRunning = false;
    public bool Running { get { return coroutineRunning; } }

    private void Awake()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();

        // the UI should be hidden until required
        canvasGroup.alpha = 0;

        // cache the initial prefab font sizes
        titleFontSize = title.fontSize;
        descriptionFontSize = description.fontSize;

        // ensures that only one Title exists in the scene
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Shows the Title GUI with the given title and description.
    /// </summary>
    public void Show(string title, string description, float titleFontSize = 0, float descriptionFontSize = 0, int lifeTime = 0)
    {
        if (titleFontSize == 0)
            titleFontSize = this.titleFontSize;

        if (descriptionFontSize == 0)
            descriptionFontSize = this.descriptionFontSize;

        if (lifeTime == 0)
            lifeTime = this.lifeTime;

        // this ensures that if show is called while a title is currently
        // visible then the new one takes precedence
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        this.title.SetText(title);
        this.title.fontSize = titleFontSize;

        this.description.SetText(description);
        this.description.fontSize = descriptionFontSize;

        currentCoroutine = Countdown(lifeTime);
        StartCoroutine(currentCoroutine);
    }

    private IEnumerator Countdown(int lifeTime)
    {
        coroutineRunning = true;

        StartCoroutine(Fade(canvasGroup, canvasGroup.alpha, 1));

        int counter = lifeTime;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }

        yield return StartCoroutine(Fade(canvasGroup, canvasGroup.alpha, 0));

        coroutineRunning = false;
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0f;

        while (counter < fadeTime)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / fadeTime);
            yield return null;
        }
    }
}
