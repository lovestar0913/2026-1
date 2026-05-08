using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class Sample : MonoBehaviour
{
    [SerializeField]
    private int _count = 5;

    private List<GameObject> _cells = new List<GameObject>();

    private int _index = 0;

    private void Start()
    {
        for (int i = 0; i < _count; i++)
        {
            var obj = new GameObject($"Cell{i}");
            obj.transform.SetParent(transform, false);

            // 位置固定
            var rect = obj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100, 100);
            rect.anchoredPosition = new Vector2(i * 120, 0);

            var image = obj.AddComponent<Image>();
            image.color = Color.white;

            _cells.Add(obj);
        }

        UpdateSelect();
    }

    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard == null)
        {
            return;
        }

        if (keyboard.aKey.wasPressedThisFrame)
        {
            if (_cells.Count == 0)
            {
                return;
            }

            _index--;

            if (_index < 0)
            {
                _index = 0;
            }

            UpdateSelect();
        }

        if (keyboard.dKey.wasPressedThisFrame)
        {
            if (_cells.Count == 0)
            {
                return;
            }

            _index++;

            if (_index >= _cells.Count)
            {
                _index = _cells.Count - 1;
            }

            UpdateSelect();
        }

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            if (_cells.Count == 0)
            {
                return;
            }

            Destroy(_cells[_index]);

            _cells.RemoveAt(_index);

            if (_cells.Count > 0)
            {
                if (_index >= _cells.Count)
                {
                    _index = _cells.Count - 1;
                }
            }
            else
            {
                _index = 0;
            }

            UpdateSelect();
        }
    }

    private void UpdateSelect()
    {
        if (_cells.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _cells.Count; i++)
        {
            var image = _cells[i].GetComponent<Image>();

            image.color = (i == _index)
                ? Color.red
                : Color.white;
        }
    }
}