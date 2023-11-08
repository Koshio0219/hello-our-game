public enum JumpState
{
    IDLE,    // 入力待ち状態
    WAITING,    // ジャンプ溜め状態
    RISING,    // 上昇中状態
    FALLING,    // 下降中状態
    LANDING,    // 着地状態
}