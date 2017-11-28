public enum MessageCode : byte
{
    Default = 0,
    Login = 1,
    Register = 2,


    AddPlayer = 200,
    RemovePlayer = 201,
    SyncTransform = 202,
    SyncTransition = 203,

    AddBomb = 210,
    OpenBomb = 211,

    PlayerDead = 220,
}