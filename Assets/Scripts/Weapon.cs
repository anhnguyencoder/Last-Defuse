using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Cài Đặt Tầm Bắn")]
    [Tooltip("Tầm bắn của vũ khí này")]
    public float range;

    [Header("Muzzle Flare")]
    [Tooltip("GameObject hiển thị lửa nòng súng")]
    public GameObject muzzleFlare;

    [Tooltip("Thời gian hiển thị muzzle flare (giây)")]
    public float flareDisplayTime = .1f;

    [Header("Cài Đặt Bắn")]
    [Tooltip("Có thể bắn tự động không? (giữ chuột để bắn liên tục)")]
    public bool canAutoFire;
    [Tooltip("Thời gian giữa các lần bắn (giây)")]
    public float timeBetweenShots = .1f;

    [Header("Cài Đặt Đạn")]
    [Tooltip("Số đạn hiện tại trong băng")]
    public int currentAmmo = 100;
    [Tooltip("Sức chứa của băng đạn")]
    public int clipSize = 15;
    [Tooltip("Số đạn còn lại trong kho")]
    public int remainingAmmo = 300;

    [Tooltip("Số đạn nhận được khi nhặt ammo pickup")]
    public int pickupAmount;

    [Header("Cài Đặt Sát Thương")]
    [Tooltip("Sát thương mỗi viên đạn")]
    public float damageAmount = 15f;

    [Header("Cài Đặt Âm Thanh")]
    [Tooltip("Chỉ số SFX để phát khi bắn (trong AudioManager)")]
    public int sfxIndex;
}
