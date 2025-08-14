using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip moveLoop; // chỉ 1 âm thanh duy nhất cho di chuyển

    [Header("Playback Settings")]
    public bool onlyWhenRunning = false; // true: chỉ phát khi chạy nhanh
    public float minWalkSpeed = 0.2f;    // ngưỡng bắt đầu phát khi đi bộ
    public float runSpeedThreshold = 4.0f; // ngưỡng xem là chạy
    public float pitchAtWalk = 1.0f;     // cao độ khi đi
    public float pitchAtRun = 1.1f;      // cao độ khi chạy
    public float volumeAtWalk = 0.6f;    // âm lượng khi đi
    public float volumeAtRun = 1.0f;     // âm lượng khi chạy
    public float smoothTime = 0.05f;     // mượt thay đổi volume/pitch tránh giật hoặc delay cảm giác

    private CharacterController characterController;
    private float volumeVel;
    private float pitchVel;

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.clip = moveLoop;
            audioSource.volume = 0f;
            audioSource.pitch = pitchAtWalk;
        }
    }

    void Update()
    {
        if (audioSource == null || characterController == null || moveLoop == null) return;

        // Tốc độ ngang
        Vector3 v = characterController.velocity; v.y = 0f;
        float speed = v.magnitude;
        bool grounded = characterController.isGrounded;

        bool shouldPlay;
        if (onlyWhenRunning)
            shouldPlay = grounded && speed >= runSpeedThreshold;
        else
            shouldPlay = grounded && speed >= minWalkSpeed;

        if (shouldPlay)
        {
            if (!audioSource.isPlaying) audioSource.Play();

            // Blend volume/pitch theo tốc độ để nghe tự nhiên, không lệ thuộc nhịp bước -> không bị trễ
            float t = Mathf.InverseLerp(minWalkSpeed, runSpeedThreshold, speed);
            float targetVol = Mathf.Lerp(volumeAtWalk, volumeAtRun, t);
            float targetPitch = Mathf.Lerp(pitchAtWalk, pitchAtRun, t);
            audioSource.volume = Mathf.SmoothDamp(audioSource.volume, targetVol, ref volumeVel, smoothTime);
            audioSource.pitch = Mathf.SmoothDamp(audioSource.pitch, targetPitch, ref pitchVel, smoothTime);
        }
        else
        {
            // Fade out nhanh rồi dừng để tránh click và cảm giác trễ
            audioSource.volume = Mathf.SmoothDamp(audioSource.volume, 0f, ref volumeVel, smoothTime);
            if (audioSource.volume <= 0.01f && audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.pitch = pitchAtWalk;
            }
        }
    }
}
