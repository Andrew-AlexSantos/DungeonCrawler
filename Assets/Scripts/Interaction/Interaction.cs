using System;
using EventsArgs;
using UnityEngine;
using Chest;
using System.Collections;
using UnityEngine.Rendering;

public class Interaction : MonoBehaviour {
    public GameObject widgetPrefab;
    [SerializeField] private Vector3 widgetOffset;
    public float radius = 5f;
    private GameObject widget;
    private bool isAvailable = true;
    private bool isActive;
    public event EventHandler<InteractionEventArgs> OnInteraction;
    private void OnEnable() {
        GameManager.Instance.interactionList.Add(this);
    }

    private void OnDisable() {
        GameManager.Instance.interactionList.Remove(this);
    }

    void Awake() {
        // Create widget
        widget = Instantiate(widgetPrefab, transform.position + widgetOffset, widgetPrefab.transform.rotation);
        widget.transform.SetParent(gameObject.transform, true);
        
    }

    void Start() {

        // Set widget camera
        var worldUiCamera = GameManager.Instance.worldUiCamera;
        var canvas = widget.GetComponent<Canvas>();
        if (canvas != null) {
            canvas.worldCamera = worldUiCamera;
        }
        var interactionWidgetComponent = widget.GetComponent<InteractionWidget>();
        if ( interactionWidgetComponent != null) {
            interactionWidgetComponent.WorldUiCamera = worldUiCamera;
        }
    }

    void Update() {}

    public bool IsActive() {
        return isActive;
    }
    public void SetActive(bool isActive)  {
        this.isActive = isActive;


    }
    public bool IsAvailable() {
        return isAvailable;
    }
    public void SetAvailable(bool isAvailable) {
        this.isAvailable = isAvailable;
        // Update InteractionWIdget
        var InteractionWidget = widget.GetComponent<InteractionWidget>();
        if (isActive) {
            InteractionWidget.Show();
        } else {
            InteractionWidget.Hide();
        }
    }
    public void Interact() {
        // Invoke event
            OnInteraction?.Invoke(this, new InteractionEventArgs());

    }

    public void SetActionText(string text) {
        // Update InteractionWIdget
        var InteractionWidget = widget.GetComponent<InteractionWidget>();
         InteractionWidget.SetActionText(text);

        }
     }
        

