#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class FullscreenGameView
{
    // Shortcut shortcut: Ctrl + Shift + Alt + F
    [MenuItem("Window/General/True Fullscreen Game View %#&f")]
    public static void ToggleFullscreen()
    {
        Type gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
        if (gameViewType == null) return;

        EditorWindow gameView = EditorWindow.GetWindow(gameViewType);
        if (gameView == null) return;

        // Toggles the maximized state of the tab window layout
        gameView.maximized = !gameView.maximized;
    }
}
#endif