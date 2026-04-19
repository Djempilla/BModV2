using UnityEngine;

namespace BModv2;

public class GuiBehaviour : MonoBehaviour
{
    private bool _showMenu = true;
    private int _selectedTab = 0;
    private readonly string[] _tabs = { "Main", "Debug", "About" };

    private bool _featureEnabled = false;
    private string _status = "Idle";
    private string _notes = "Hello from IMGUI!";

    private void Awake()
    {
        Plugin.Log.LogInfo("[GUI] Awake");
    }

    private void Start()
    {
        Plugin.Log.LogInfo("[GUI] Start");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            _showMenu = !_showMenu;
            Plugin.Log.LogInfo($"[GUI] Menu visible: {_showMenu}");
        }
    }

    private void OnGUI()
    {
        if (!_showMenu)
            return;

        GUI.Box(new Rect(20f, 20f, 500f, 360f), "BModv2");

        GUILayout.BeginArea(new Rect(35f, 50f, 470f, 320f));

        DrawHeader();
        GUILayout.Space(8f);
        DrawTabs();
        GUILayout.Space(10f);
        DrawActiveTab();

        GUILayout.EndArea();
    }

    private void DrawHeader()
    {
        GUILayout.Label("BModv2 Mod Menu");
        GUILayout.Label("F6 - show / hide");
    }

    private void DrawTabs()
    {
        GUILayout.BeginHorizontal();

        for (int i = 0; i < _tabs.Length; i++)
        {
            if (GUILayout.Button(_tabs[i], GUILayout.Height(28f)))
                _selectedTab = i;
        }

        GUILayout.EndHorizontal();
    }

    private void DrawActiveTab()
    {
        switch (_selectedTab)
        {
            case 0:
                DrawMainTab();
                break;
            case 1:
                DrawDebugTab();
                break;
            case 2:
                DrawAboutTab();
                break;
        }
    }

    private void DrawMainTab()
    {
        GUILayout.Label("Main controls");
        GUILayout.Space(6f);

        _featureEnabled = GUILayout.Toggle(_featureEnabled, "Enable example feature");
        GUILayout.Label($"Feature state: {(_featureEnabled ? "Enabled" : "Disabled")}");

        GUILayout.Space(8f);

        if (GUILayout.Button("Run action", GUILayout.Height(32f)))
        {
            _status = "Action executed";
            Plugin.Log.LogInfo("[GUI] Run action clicked");
        }

        GUILayout.Space(8f);
        GUILayout.Label($"Status: {_status}");
    }

    private void DrawDebugTab()
    {
        GUILayout.Label("Debug tools");
        GUILayout.Space(6f);

        if (GUILayout.Button("Print debug log", GUILayout.Height(32f)))
        {
            Plugin.Log.LogInfo("[GUI] Debug button pressed");
            _status = "Debug log printed";
        }

        GUILayout.Space(8f);
        GUILayout.Label("Notes:");
        _notes = GUILayout.TextArea(_notes, GUILayout.Height(140f));

        if (GUILayout.Button("Clear notes", GUILayout.Height(28f)))
            _notes = "";
    }

    private void DrawAboutTab()
    {
        GUILayout.Label("About");
        GUILayout.Space(6f);

        GUILayout.Label("BModv2 test GUI");
        GUILayout.Label("Built with Unity IMGUI");
        GUILayout.Label("Use this as a base for your real menu");
    }
}