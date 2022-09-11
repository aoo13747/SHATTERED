using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Character 
{
    public string characterName;
    //public string displayName = "";
    [HideInInspector] public RectTransform root;
    public bool enabled { get { return root.gameObject.activeInHierarchy; } set { root.gameObject.SetActive(value); } }
    public Vector2 anchorPadding { get { return root.anchorMax - root.anchorMin; } }
    DialogSystem dialog;
    public void Say(string speech, bool add = false)
    {
        if (!enabled)
            enabled = true;
        if (!isInScene)
            FadeIn();

        dialog.Say(speech, characterName,add);
       
    }
    Vector2 targetPosition;
    Coroutine moving;
    bool isMoving { get { return moving != null; } }
    public void MoveTo(Vector2 Target, float speed, bool smooth = true)
    {
        StopMoving();
        moving = CharacterManager.instance.StartCoroutine(Moving(Target, speed, smooth));
    }
    public void StopMoving(bool arriveAtTargetPositionImmediately = false)
    {
        if (isMoving)
        {
            CharacterManager.instance.StopCoroutine(moving);
            if (arriveAtTargetPositionImmediately)
                SetPosition(targetPosition);
        }
        moving = null;
    }
    public void SetPosition(Vector2 target)
    {
        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;
        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);

        root.anchorMin = minAnchorTarget;
        root.anchorMax = root.anchorMin + padding;
    }
    IEnumerator Moving(Vector2 target, float speed, bool smooth)
    {
        targetPosition = target;
        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;

        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        speed *= Time.deltaTime;
        while (root.anchorMin != minAnchorTarget)
        {
            root.anchorMin = (!smooth) ? Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed) : Vector2.Lerp(root.anchorMin, minAnchorTarget, speed);
            root.anchorMax = root.anchorMin + padding;
            yield return new WaitForEndOfFrame();
        }

        StopMoving();
    }

    public Sprite GetSprite(int index = 0)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Novelpic/" + characterName);
        //Debug.Log(sprites.Length);
        return sprites[index];
    }
    public Sprite GetSprite(string spriteName = "")
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Novelpic/" + characterName);
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name == spriteName)
            {
                return sprites[i];
            }
           
        }
        return sprites.Length > 0 ? sprites[0] : null;
    }
    public void SetBody(int index)
    {
        renderers.bodyRenderer.sprite = GetSprite(index);
    }
    public void SetBody(Sprite sprite)
    {
        renderers.bodyRenderer.sprite = sprite;
    }
    public void SetBody(string spriteName)
    {
        renderers.bodyRenderer.sprite = GetSprite(spriteName);
    }
    bool isTransitioningBody { get { return transitioningBody != null; } }
    Coroutine transitioningBody = null;

    public void TransitionBody(Sprite sprite, float speed, bool smooth)
    {
        if (renderers.bodyRenderer.sprite == sprite)
            return;

        StopTransitioningBody();
        transitioningBody = CharacterManager.instance.StartCoroutine(TransitioningBody(sprite, speed, smooth));
    }

    void StopTransitioningBody()
    {
        if (isTransitioningBody)
            CharacterManager.instance.StopCoroutine(transitioningBody);
        transitioningBody = null;
    }
    public IEnumerator TransitioningBody(Sprite sprite, float speed, bool smooth)
    {
        for (int i = 0; i < renderers.allBodyRenderers.Count; i++)
        {
            Image image = renderers.allBodyRenderers[i];
            if (image.sprite == sprite)
            {
                renderers.bodyRenderer = image;
                break;
            }
        }

        if (renderers.bodyRenderer.sprite != sprite)
        {
            Image image = GameObject.Instantiate(renderers.bodyRenderer.gameObject, renderers.bodyRenderer.transform.parent).GetComponent<Image>();
            renderers.allBodyRenderers.Add(image);
            renderers.bodyRenderer = image;
            image.color = GlobalF.SetAlpha(image.color, 0f);
            image.sprite = sprite;
        }

        while (GlobalF.TransitionImages(ref renderers.bodyRenderer, ref renderers.allBodyRenderers, speed, smooth))
            yield return new WaitForEndOfFrame();

        Debug.Log("done");
        StopTransitioningBody();
    }
    //public bool isEnteringOrExitingScene { get { return enteringExiting != null; } }
    public bool isInScene = false;
    Sprite lastBodySprite = null;

    //Coroutine enteringExiting = null;
    public void FadeOut(float speed = 3, bool smooth = false)
    {
        Sprite alphaSprite = Resources.Load<Sprite>("Images/AlphaOnly");

        lastBodySprite = renderers.bodyRenderer.sprite;

        TransitionBody(alphaSprite, speed, smooth);
    }
    public void FadeIn(float speed = 3, bool smooth = false)
    {
        if (lastBodySprite != null )
        {
            TransitionBody(lastBodySprite, speed, smooth);
        }
    }

   /* IEnumerator EnteringScene(float speed = 3, bool smooth = false)
    {
        isInScene = true;

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha = smooth ? Mathf.Lerp(canvasGroup.alpha, 1, speed * Time.deltaTime) : Mathf.MoveTowards(canvasGroup.alpha, 1, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        enteringExiting = null;
    }

    IEnumerator ExitingScene(float speed = 3, bool smooth = false)
    {
        isInScene = false;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha = smooth ? Mathf.Lerp(canvasGroup.alpha, 0, speed * Time.deltaTime) : Mathf.MoveTowards(canvasGroup.alpha, 0, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        enteringExiting = null;
        CharacterManager.instance.DestroyCharacter(this);
    }*/
    //public CanvasGroup canvasGroup;
    public Character(string _name , bool enabledOnStart = true)
    {
        CharacterManager cm = CharacterManager.instance;
        GameObject prefad = Resources.Load("Characters/Character[" + _name + "]")as GameObject;
        GameObject ob = GameObject.Instantiate (prefad, cm.characterPanel);
        root = ob.GetComponent<RectTransform>();
        //canvasGroup = ob.GetComponent<CanvasGroup>();
        //canvasGroup.alpha = 0;

        characterName = _name;
        //displayName = characterName;
        renderers.bodyRenderer = ob.transform.Find("bodyLayer").GetComponentInChildren<Image>();
        renderers.allBodyRenderers.Add(renderers.bodyRenderer);
       
        dialog = DialogSystem.instance;
        enabled = enabledOnStart;
    }
    [System.Serializable]
    public class Renderers
    {
        //public Image renderer;
        public Image bodyRenderer;
        public List<Image> allBodyRenderers = new List<Image>();
    }
    public Renderers renderers = new Renderers();
}
