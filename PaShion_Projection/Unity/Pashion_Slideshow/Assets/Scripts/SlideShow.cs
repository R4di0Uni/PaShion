using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SlideShow : MonoBehaviour
{
    public Renderer targetRenderer;
    public float switchTime = 3f;
    public float refreshTime = 2f;

    private List<Texture2D> textures = new List<Texture2D>();
    private List<string> loadedFiles = new List<string>();

    private int currentIndex = 0;

    string folderPath =
        @"C:\Users\Asus\Documents\GitHub\PaShion\PaShion_Projection\TouchDesigner\Textures";

    void Start()
    {
        LoadNewTextures();

        StartCoroutine(Slideshow());
        StartCoroutine(CheckForNewTextures());
    }

    void LoadNewTextures()
    {
        string[] files = Directory.GetFiles(folderPath);

        foreach (string file in files)
        {
            // Skip already loaded files
            if (loadedFiles.Contains(file))
                continue;

            if (file.EndsWith(".png") || file.EndsWith(".jpg"))
            {
                try
                {
                    byte[] imageBytes = File.ReadAllBytes(file);

                    Texture2D tex = new Texture2D(2, 2);

                    tex.LoadImage(imageBytes);

                    textures.Add(tex);
                    loadedFiles.Add(file);

                    Debug.Log("Loaded NEW texture: " + file);
                }
                catch
                {
                    Debug.Log("File still being written: " + file);
                }
            }
        }
    }

    IEnumerator CheckForNewTextures()
    {
        while (true)
        {
            LoadNewTextures();

            yield return new WaitForSeconds(refreshTime);
        }
    }

    IEnumerator Slideshow()
    {
        while (true)
        {
            if (textures.Count > 0)
            {
                Material mat = targetRenderer.material;

                mat.mainTexture = textures[currentIndex];

                // Vertical flip fix
                mat.mainTextureScale = new Vector2(1, -1);
                mat.mainTextureOffset = new Vector2(0, 1);

                currentIndex++;

                if (currentIndex >= textures.Count)
                    currentIndex = 0;
            }

            yield return new WaitForSeconds(switchTime);
        }
    }
}