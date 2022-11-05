using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
	[SerializeField]
	private bool ShowMilliseconds = true;
	[SerializeField]
	private bool ShowFramesPerSecond = true;

	private float _DeltaTime = 0.0f;
	private float _Milliseconds = 0.0f;
	private float _Fps = 0.0f;
	private string _OutputText = "";
	private Rect _OutputRect = new Rect();
	private GUIStyle _Style = new GUIStyle();
	private int _Width = 0;
    private int _Height = 0;

	void Update()
	{
		_DeltaTime += (Time.unscaledDeltaTime - _DeltaTime) * 0.1f;
	}

	void OnGUI()
	{
		_OutputText = "";
		_Width = Screen.width;
		_Height = Screen.height;
		_OutputRect = new Rect(0, 0, _Width, _Height * 2 / 100);
		_Style.alignment = TextAnchor.LowerRight;
		_Style.fontSize = _Height * 2 / 100;
		_Style.normal.textColor = new Color(1f, 1f, 1f, 1.0f);
		_Milliseconds = _DeltaTime * 1000.0f;
        _Fps = 1.0f / _DeltaTime;

		if (ShowMilliseconds)
		{
			_OutputText = string.Format("{0:0.0} ms  ", _Milliseconds);
		}
		if(ShowFramesPerSecond)
        {
			_OutputText += string.Format("({0:0.} fps)", _Fps);
		}

		if (ShowFramesPerSecond || ShowMilliseconds)
        {
			GUI.Label(_OutputRect, _OutputText, _Style);
		}
	}
}
