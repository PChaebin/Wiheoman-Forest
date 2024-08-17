using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance;

    [SerializeField] GameObject canvas;
    [SerializeField] GameObject baseWindow;
    [SerializeField] GameObject textWindow;

    Stack<GameObject> windows;
    int stopTriggerCount = 0;

    bool isStopped = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 가장 첫번째의 윈도우를 닫습니다.
            if (windows.Count <= 0)
            {
                return;
            }

            GameObject one = windows.Pop();

            WindowController windowsController = one.GetComponent<WindowController>();
            if (windowsController != null)
            {
                if (windowsController.hasStopTrigger == true)
                {
                    PopStopTrigger();
                }
            }

            Destroy(one);
        }
    }


    public void PauseScene()
    {
        isStopped = true;
        Time.timeScale = 0;
    }
    public void ResumeScene()
    {
        isStopped = true;
        Time.timeScale = 1;
    }
    public void AddStopTrigger()
    {
        if (stopTriggerCount == 0)
        {
            PauseScene();
        }
        stopTriggerCount++;
    }
    public void PopStopTrigger()
    {
        if (stopTriggerCount <= 0)
        {
            return;
        }
        if (stopTriggerCount == 1)
        {
            ResumeScene();
        }
        stopTriggerCount--;
    }

    public void OpenWindow()
    {
        Instantiate(baseWindow, canvas.transform.position, textWindow.transform.rotation, canvas.transform);
    }

    public void OpenTextWimdow(string message = "메시지를 입력하세요", bool isStopTrigger = false)
    {
        GameObject newWindow = Instantiate(textWindow, canvas.transform);
        WindowController m_WindowController = newWindow.GetComponent<WindowController>();
        m_WindowController.ChangeLog(message);

        RectTransform rectTransform = newWindow.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector3.zero;

        if (isStopTrigger)
        {
            AddStopTrigger();
            m_WindowController.hasStopTrigger = true;
        }
    }

    private void Awake()
    {
        instance = this;
        windows = new Stack<GameObject>();
    }

    private void Start()
    {
        GameObject newWindow = Instantiate(textWindow, canvas.transform);
    }
}