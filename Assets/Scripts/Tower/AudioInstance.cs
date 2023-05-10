using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class AudioInstance : object
{
    //Create a class that actually inheritance from MonoBehaviour
    public class MyStaticMB : MonoBehaviour { }


    //Variable reference for the class
    private static MyStaticMB myStaticMB;



    //Now Initialize the variable (instance)
    private static void Init()
    {
        //If the instance not exit the first time we call the static class
        if (myStaticMB == null)
        {
            //Create an empty object called MyStatic
            GameObject gameObject = new GameObject("MyStatic");


            //Add this script to the object
            myStaticMB = gameObject.AddComponent<MyStaticMB>();
        }
    }



    //Now, a simple function
    public static void Play(AudioClip _audio)
    {
        //Call the Initialization
        Init();


        //Call the Coroutine
        myStaticMB.StartCoroutine(InstantiateAudio(_audio));
    }

    public static void Play(AudioSource _audio)
    {
        //Call the Initialization
        Init();


        //Call the Coroutine
        myStaticMB.StartCoroutine(InstantiateAudio(_audio));
    }
    public static IEnumerator InstantiateAudio(AudioClip _audio)
    {
        GameObject tmp = MonoBehaviour.Instantiate(new GameObject());
        AudioSource source = tmp.AddComponent<AudioSource>();
        source.clip = _audio;
        source.Play();
        //yield return new WaitForFixedUpdate();
        yield return new WaitWhile(new System.Func<bool>(() => source.isPlaying));
        MonoBehaviour.Destroy(tmp);
    }
    public static IEnumerator InstantiateAudio(AudioSource _audio)
    {
        GameObject tmp = MonoBehaviour.Instantiate(new GameObject());
        //AudioSource source = tmp.AddComponent<AudioSource>();
        AudioSource source = CopyComponent(_audio, tmp, false);
        source.Play();
        yield return new WaitWhile(new System.Func<bool>(() => source.isPlaying));
        MonoBehaviour.Destroy(tmp);
    }
    static T CopyComponent<T>(T original, GameObject destination, bool paste_over) where T : Component
    {
        System.Type type = original.GetType();
        Component copy;
        if (paste_over && destination.GetComponent(type))
        {
            copy = destination.GetComponent(type);
        }
        else
        {
            copy = destination.AddComponent(type);
        }
        System.Reflection.PropertyInfo[] props = type.GetProperties();
        foreach (System.Reflection.PropertyInfo prop in props)
        {
            if (prop.CanWrite && prop.GetCustomAttribute<ObsoleteAttribute>() != null)
            {
                prop.SetValue(copy, prop.GetValue(original));
            }          
        }
        return copy as T;
    }
}
