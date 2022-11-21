using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace HiggsyVN
{


public class CutsceneManager : MonoBehaviour
{
    //public Image spriteA;
    //public Image spriteB;
    public SpeakerObjectPair[]  speakerDict_i;
    public Dictionary<Speakers, Image> speakerDict;
    public TextMeshProUGUI dialogue;
    public TextMeshProUGUI nameplate;
    public GameObject backingParent;
    public Dictionary<Background, GameObject> backgroundDict;
    public backgroundEntry[] backgroundDict_i;
    public float skipDelay;
    public float dialogueSpeed;
    public bool usingTypewriter;
    //public Color32 activeColor;
    //public Color32 inactiveColor;
    public Sprite noInputSprite;
    //public DayManager DM;
    [Header("Set programatically")]
    public Scene activeScene;
    private float timeSinceLastSkip;
    
    private bool skipping=false;
    private TypeWriter dialogueTypewriter;
    

    // Start is called before the first frame update
    void Start()
    {
        //Monodam.i.gameControls.Dialogue.Continue.performed += advanceDialogue;
        //Monodam.i.gameControls.Dream_Exploration.AdvanceDialogue.performed += advanceDialogue;
        //Monodam.i.gameControls.Dream_Exploration.SkipDialogue.performed += startSkipping;
        //Monodam.i.gameControls.Dream_Exploration.SkipDialogue.canceled += stopSkipping;

        if (usingTypewriter)
        {
            dialogueTypewriter = dialogue.GetComponent<TypeWriter>();
        }
        backgroundDict = new Dictionary<Background, GameObject>();
        speakerDict = new Dictionary<Speakers, Image>();

        foreach(backgroundEntry entry in backgroundDict_i)
        {
            backgroundDict.Add(entry.key, entry.value);
        }
        foreach (SpeakerObjectPair entry in speakerDict_i)
        {
           speakerDict.Add(entry.key, entry.value);
        }


    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSkip += Time.deltaTime;
        if (skipping && timeSinceLastSkip >= skipDelay)
        {
            advanceDialogue();
            timeSinceLastSkip = 0;
        }
    }


    #region PlayerInput
    public void advanceDialogue(InputAction.CallbackContext context)
    {

        advanceDialogue();

    }

    public void advanceDialogue()
    {
        if (usingTypewriter && dialogueTypewriter != null)
        {
            if (dialogueTypewriter.isTyping)
            {
                dialogueTypewriter.skipTypewrite();
                return;
            }
        }
       
        setLine(activeScene.AdvanceScene());
        
        
    }

    public void startSkipping(InputAction.CallbackContext context)
    {
        startSkipping();
    }

    public void startSkipping()
    {
        skipping = true;
    }

    public void stopSkipping(InputAction.CallbackContext context)
    {
        stopSkipping();
    }

    public void stopSkipping()
    {
        timeSinceLastSkip = 0;
        skipping = false;
    }

    #endregion
    public void setScene(Scene newScene)
    {
        this.gameObject.SetActive(true);
        skipping = false;
        activeScene = newScene;
        //spriteA.enabled = false;
       // spriteB.enabled = false;
        setLine(newScene.StartScene());
        

    }
    public void setLine(Scene.Line line)
    {
        if (line == null)
        {
            atSceneEnd();
        }
        else
        {
            handleDialogue();
            handleSpriteChange();
            handleBackingChange();
            
            //if (line.speakerEnum == Scene.Characters.Blake)
            //{
            //    spriteA.color = activeColor;
            //    spriteB.color = inactiveColor;
            //    if (line.speakerSprite != null)
            //    {
            //        spriteA.enabled = true;
            //        spriteA.sprite = line.speakerSprite;
            //    }
            //}
            //else
            //{
            //    spriteA.color = inactiveColor;
            //    spriteB.color = activeColor;
            //    if (line.speakerSprite != null)
            //    {
            //        spriteB.enabled = true;
            //        spriteB.sprite = line.speakerSprite;
            //    }
            //}

        }

        void handleSpriteChange()
        {
            foreach(Scene.SpeakerSpritePair pair in line.spriteChanges)
            {
                if (speakerDict.ContainsKey(pair.speaker))
                {
                    if (pair.sprite == null)
                    {
                        speakerDict[pair.speaker].sprite = noInputSprite;
                    }
                    else
                    {
                        speakerDict[pair.speaker].sprite = pair.sprite;
                    }
                    
                }
                
            }
        }

        void handleDialogue()
        {
            nameplate.text = line.speaker;
            //GameObject dialogee = dialogue.gameObject;
            if ( usingTypewriter && dialogueTypewriter != null)
            {
                dialogue.gameObject.GetComponent<TypeWriter>().BeginTyping(line.text, dialogueSpeed);
            }
            else
            {
                dialogue.text = line.text;
            }
                
            
        }

        void handleBackingChange()
        { 
            if (backgroundDict.ContainsKey(line.background))
            {
                newBackground(backgroundDict[line.background]);
            }
            
        }

        void atSceneEnd()
        {
            if (activeScene.nextScene == null)
            {
                cutsceneOver();
            }
            else
            {
                setScene(activeScene.nextScene);
            }
        }
    }

    void newBackground(GameObject bg)
    {
        if (bg != null)
        {
            clearBackgrounds();
            bg.SetActive(true);
        }
        

    }

    void cutsceneOver()
    {
        Debug.Log("Cutscene is over...we should do something about that.");
        //if (!activeScene.phased)
        //{
        //    DM.BeginNight();
        //}
        //this.gameObject.SetActive(false);
        
        
    }

    void clearBackgrounds()
    {
        foreach (Transform child in backingParent.transform)
            child.gameObject.SetActive(false);
    }




    [System.Serializable]
    public enum Background
    {
        NoChange,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z
    }

    [System.Serializable]
   // [System.Flags]
    public enum Speakers //each option must be in multiples of 2 for flag attribute to function
    {
        A = 1,
        B = 2,
        C = 4
    }

    [System.Serializable]
    public struct SpeakerObjectPair
    {
        public Speakers key;
        public Image value;
    }

    [System.Serializable]
    public struct backgroundEntry
    {
        public Background key;
        public GameObject value;
    }

   

}

}
