using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeleteLevel : MonoBehaviour
{
    [SerializeField] private TMP_Text deleteLevelText;
    
    private GameObject levelGridItem;
    private string levelName;

    public void SetLevel(string levelName, GameObject levelGridItem)
    {
        this.levelName = levelName;
        this.levelGridItem = levelGridItem;
        deleteLevelText.SetText("Are you sure you want to delete \"" + levelName + "\"?");
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    public void Delete()
    {
        Destroy(levelGridItem);

        string path = Application.persistentDataPath + "/User Levels/" + levelName;
        System.IO.Directory.Delete(path, true);
        
        gameObject.SetActive(false);
    }
}
