using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ColliderDraw : MonoBehaviour {

    private LineRenderer _line;
    private Collider2D _collider;
    private bool _hasLine, _hasCollider;

    // Use this for initialization
    void Start () {
        _collider = GetComponent<Collider2D>();
        _line = GetComponent<LineRenderer>();
        if (_collider != null) {
            _hasCollider = true;
        }
        if (_line != null) {
            _line.receiveShadows = false;
            _line.useWorldSpace = false;
            _line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            _line.material = new Material(Shader.Find("Sprites/Default"));
            _line.startColor = Color.red;
            _line.endColor = Color.red;
            _line.sortingLayerName = "LayerHud";
            _line.widthMultiplier = 0.02f;
            _hasLine = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_hasLine && _hasCollider) {
            if (_collider.GetType() == typeof(CircleCollider2D)) {
                // Colisor do tipo circular
                CircleCollider2D circle = _collider as CircleCollider2D;
                Vector3 pos;
                int size = (int)((2f * Mathf.PI) / 0.01f);
                float theta = 0f;
                _line.numPositions = size;
                for (int i = 0; i < size; i++) {
                    theta += 2f * Mathf.PI * 0.01f;
                    float x = circle.radius * Mathf.Cos(theta);
                    float y = circle.radius * Mathf.Sin(theta);
                    //x += gameObject.transform.position.x;
                    //y += gameObject.transform.position.y;
                    pos = new Vector3(x, y, 0);
                    _line.SetPosition(i, pos);
                }
            }
            else if (_collider.GetType() == typeof(BoxCollider2D)) {
                // Colisor do tipo retangular
                BoxCollider2D box = _collider as BoxCollider2D;
                int size = 5;
                _line.numPositions = size;
                _line.SetPosition(0, new Vector3(-box.size.x / 2, -box.size.y / 2, 0));
                _line.SetPosition(1, new Vector3(box.size.x / 2, -box.size.y / 2, 0));
                _line.SetPosition(2, new Vector3(box.size.x / 2, box.size.y / 2, 0));
                _line.SetPosition(3, new Vector3(-box.size.x / 2, box.size.y / 2, 0));
                _line.SetPosition(4, new Vector3(-box.size.x / 2, -box.size.y / 2, 0));
            }
        }
    }
}
