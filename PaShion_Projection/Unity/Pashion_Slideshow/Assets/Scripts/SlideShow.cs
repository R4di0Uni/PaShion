using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SlideShow : MonoBehaviour
{
    public Renderer targetRenderer;
    public float switchTime = 3f;

    private List<Texture2D> textures = new List<Texture2D>();
    private int currentIndex = 0;

    void Start()
    {
        LoadTextures();

        if (textures.Count > 0)
        {
            StartCoroutine(Slideshow());
        }
        else
        {
            Debug.Log("No textures found.");
        }
    }

    void LoadTextures()
    {
        string folderPath = @"C:\Users\Asus\Documents\GitHub\PaShion\PaShion_Projection\TouchDesigner\Textures";

        string[] files = Directory.GetFiles(folderPath);

        foreach (string file in files)
        {
            if (file.EndsWith(".png") || file.EndsWith(".jpg"))
            {
                byte[] imageBytes = File.ReadAllBytes(file);

                Texture2D tex = new Texture2D(2, 2);

                tex.LoadImage(imageBytes);

                textures.Add(tex);

                Debug.Log("Loaded: " + file);
            }
        }
    }

    IEnumerator Slideshow()
    {
        while (true)
        {
            targetRenderer.material.mainTexture = textures[currentIndex];

            currentIndex++;

            if (currentIndex >= textures.Count)
                currentIndex = 0;

            yield return new WaitForSeconds(switchTime);
        }
    }
}