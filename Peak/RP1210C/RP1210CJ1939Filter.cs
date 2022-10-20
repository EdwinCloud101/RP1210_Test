
// Type: Peak.RP1210C.RP1210CJ1939Filter




using System;

namespace Peak.RP1210C
{
  [Flags]
  public enum RP1210CJ1939Filter : byte
  {
    FILTER_PGN = 1,
    FILTER_PRIORITY = 2,
    FILTER_SOURCE = 4,
    FILTER_DESTINATION = 8,
  }
}
