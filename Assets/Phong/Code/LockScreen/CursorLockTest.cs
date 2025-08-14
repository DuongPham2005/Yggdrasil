using UnityEngine;

public class CursorLockTest : MonoBehaviour
{
    [SerializeField] private KeyCode holdKey = KeyCode.BackQuote; // ~ key
    [SerializeField] private MonoBehaviour[] behavioursToDisableWhileFree; // e.g., camera/mouse look scripts

    private bool freeCursorActive;

    public static bool IsFreeCursorActive { get; private set; }

    void Start()
    {
        ApplyLock(true);
    }

    void Update()
    {
        bool wantFreeCursor = Input.GetKey(holdKey);
        if (wantFreeCursor != freeCursorActive)
        {
            freeCursorActive = wantFreeCursor;
            IsFreeCursorActive = freeCursorActive;
            ApplyLock(!freeCursorActive);
            SetBehavioursEnabled(!freeCursorActive);
        }
    }

    private void ApplyLock(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    private void SetBehavioursEnabled(bool enabled)
    {
        if (behavioursToDisableWhileFree == null) return;
        for (int i = 0; i < behavioursToDisableWhileFree.Length; i++)
        {
            var b = behavioursToDisableWhileFree[i];
            if (b == null) continue;
            b.enabled = enabled;
        }
    }
}
