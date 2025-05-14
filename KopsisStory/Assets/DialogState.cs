// // DialogState.cs
// public enum DialogType { None, Standard, Special }

// public static class DialogState
// {
//     public static DialogType ActiveDialogType { get; private set; } = DialogType.None;
    
//     public static bool TryStartDialog(DialogType type)
//     {
//         if (ActiveDialogType != DialogType.None) return false;
//         ActiveDialogType = type;
//         return true;
//     }

//     public static void EndDialog(DialogType type)
//     {
//         if (ActiveDialogType == type)
//             ActiveDialogType = DialogType.None;
//     }
// }