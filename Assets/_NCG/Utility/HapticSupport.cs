namespace NCG.template.utility
{
    public static class HapticSupport
    {
        private static bool? _supportHaptic;

        public static bool SupportHaptic
        {
            get
            {
                if (!_supportHaptic.HasValue)
                {
#if !UNITY_EDITOR
                    _supportHaptic = UnityEngine.SystemInfo.supportsVibration;
#else
                    _supportHaptic = true;
#endif
                }

                return _supportHaptic.Value;
            }
        }
    }
}