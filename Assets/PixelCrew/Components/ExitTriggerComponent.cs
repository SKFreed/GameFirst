using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitTriggerComponent : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private UnityEvent _action;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_tag))
        {
            _action?.Invoke();
        }
    }
}
