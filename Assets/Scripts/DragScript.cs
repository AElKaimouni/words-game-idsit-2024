using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public static DragScript hurufSedangDrag;

    [SerializeField] TMPro.TextMeshProUGUI hurufDisplay;

    private bool petunjuk, terisi;
    private Vector3 posisiAwal; 
    private Transform parentAwal;

    public string Huruf { get; private set; }

    public void initialiser(Transform parent, string huruf, bool petunjuk)
    {
        Huruf = huruf;
        Debug.Log(Huruf);
        transform.SetParent(parent);
        if (hurufDisplay != null)
        {
            hurufDisplay.SetText(Huruf);
        }
        else
        {
            Debug.LogError("TextMeshPro component is not assigned to hurufDisplay");
        }
        this.petunjuk = petunjuk;
        GetComponent<CanvasGroup>().alpha = petunjuk ? 0.5f : 1f;
    }

    public void Cocok(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        petunjuk = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (petunjuk)
            return;
        posisiAwal = transform.position;
        parentAwal = transform.parent;
        hurufSedangDrag = this;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (petunjuk)
            return;
        transform.position = Input.mousePosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (petunjuk && !terisi)
        {
            if (hurufSedangDrag.Huruf == Huruf)
            {
                ManagerKata.Instance.TambahPoin();
                hurufSedangDrag.Cocok(transform);
                terisi = true;
                GetComponent<CanvasGroup>().alpha = 1f;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (petunjuk)
            return;
        hurufSedangDrag = null;

        if (transform.parent == parentAwal)
        {
            transform.position = posisiAwal;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}