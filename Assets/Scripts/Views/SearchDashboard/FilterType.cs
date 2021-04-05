using System;

namespace Views.SearchDashboard
{
    [Flags]
    public enum FilterType
    {
        None = 0,
        Color = 1,
        Numeric = 2,
        String = 4,
        Enabled = 8
    }
}