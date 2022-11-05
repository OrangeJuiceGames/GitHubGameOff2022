using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class JoystickInput : MonoBehaviour
{
    public bool IsActive { get; set; }
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }


    private KeyCode _JB5 = KeyCode.Joystick1Button5, _JB8 = KeyCode.Joystick1Button8, _JB10 = KeyCode.Joystick1Button10,
        _JB6 = KeyCode.Joystick1Button6, _JB7 = KeyCode.Joystick1Button7, _JB11 = KeyCode.Joystick1Button11;

    private Stopwatch _InputWindow = new Stopwatch();
    private bool _InputBuffering;

    private Queue<KeyCode> _InputCodes = new Queue<KeyCode>();
    private Queue<float> _HorizontalInput = new Queue<float>();
    private Queue<float> _VerticalInput = new Queue<float>();

    private float _PreviousH;
    private float _PreviousV;

    // Update is called once per frame
    void Update()
    {
        if(_InputBuffering)
        {
            System.Array values = System.Enum.GetValues(typeof(KeyCode));
            foreach (KeyCode code in values)
            {
                if (Input.GetKeyDown(code)) 
                {
                    //print(System.Enum.GetName(typeof(KeyCode), code));
                    _InputCodes.Enqueue(code);
                }
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (_PreviousH != h)
            {
                _PreviousH = h;
                _HorizontalInput.Enqueue(h);
            }

            if (_PreviousV != v)
            {
                _PreviousV = v;
                _VerticalInput.Enqueue(v);
            }

        }

        if(_InputWindow.ElapsedMilliseconds > 1500 && _InputBuffering)
        {
            _InputBuffering = false;
            _InputWindow.Stop();

            while(_InputCodes.Count > 0)
            {
                KeyCode code = _InputCodes.Dequeue();
                print(System.Enum.GetName(typeof(KeyCode), code));
            }

            while (_VerticalInput.Count > 0)
            {
                float v = _VerticalInput.Dequeue();
                UnityEngine.Debug.Log($"<color=green>{v}</color>");
            }

            while (_HorizontalInput.Count > 0)
            {
                float h = _HorizontalInput.Dequeue();
                UnityEngine.Debug.Log($"<color=red>{h}</color>");
            }
        }

        if (Input.GetKeyDown(_JB5) && !_InputBuffering)
        {
            _InputBuffering = true;

            UnityEngine.Debug.Log($"<color=yellow>Capturing Input..</color>");

            _InputWindow.Reset();
            _InputWindow.Start();
        }

        CheckForUpdated();
    }

    private void CheckForUpdated()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
    }
}
