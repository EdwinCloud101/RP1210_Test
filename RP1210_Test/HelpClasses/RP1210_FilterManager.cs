
// Type: RP1210_Test.HelpClasses.RP1210_FilterManager




using System.Collections.Generic;

namespace RP1210_Test.HelpClasses
{
  public class RP1210_FilterManager
  {
    private List<RP1210_FilterCan> m_Can;
    private List<RP1210_FilterJ1939> m_J1939;
    private List<RP1210_FilterIso15765> m_Iso15765;

    public RP1210_FilterManager()
    {
      this.m_Can = new List<RP1210_FilterCan>();
      this.m_J1939 = new List<RP1210_FilterJ1939>();
      this.m_Iso15765 = new List<RP1210_FilterIso15765>();
    }

    public RP1210_FilterManager(RP1210_FilterManager filterMng)
    {
      this.m_Can = new List<RP1210_FilterCan>((IEnumerable<RP1210_FilterCan>) filterMng.m_Can);
      this.m_J1939 = new List<RP1210_FilterJ1939>((IEnumerable<RP1210_FilterJ1939>) filterMng.m_J1939);
      this.m_Iso15765 = new List<RP1210_FilterIso15765>((IEnumerable<RP1210_FilterIso15765>) filterMng.m_Iso15765);
    }

    public void ClearFilterCan() => this.m_Can.Clear();

    public void ClearFilterJ1939() => this.m_J1939.Clear();

    public void ClearFilterIso15765() => this.m_Iso15765.Clear();

    public bool AlreadyExists(RP1210_FilterCan filter)
    {
      foreach (RP1210_FilterCan rp1210FilterCan in this.m_Can)
      {
        if (rp1210FilterCan == filter)
          return true;
      }
      return false;
    }

    public bool AlreadyExists(RP1210_FilterJ1939 filter)
    {
      foreach (RP1210_FilterJ1939 rp1210FilterJ1939 in this.m_J1939)
      {
        if (rp1210FilterJ1939 == filter)
          return true;
      }
      return false;
    }

    public bool AlreadyExists(RP1210_FilterIso15765 filter)
    {
      foreach (RP1210_FilterIso15765 rp1210FilterIso15765 in this.m_Iso15765)
      {
        if (rp1210FilterIso15765 == filter)
          return true;
      }
      return false;
    }

    public bool AddFilter(RP1210_FilterCan filter)
    {
      if (this.AlreadyExists(filter))
        return false;
      this.m_Can.Add(filter);
      return true;
    }

    public bool AddFilter(RP1210_FilterJ1939 filter)
    {
      if (this.AlreadyExists(filter))
        return false;
      this.m_J1939.Add(filter);
      return true;
    }

    public bool AddFilter(RP1210_FilterIso15765 filter)
    {
      if (this.AlreadyExists(filter))
        return false;
      this.m_Iso15765.Add(filter);
      return true;
    }

    public bool RemoveFilter(RP1210_FilterCan filter)
    {
      for (int index = 0; index < this.m_Can.Count; ++index)
      {
        if (this.m_Can[index] == filter)
        {
          this.m_Can.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    public bool RemoveFilter(RP1210_FilterJ1939 filter)
    {
      for (int index = 0; index < this.m_J1939.Count; ++index)
      {
        if (this.m_J1939[index] == filter)
        {
          this.m_J1939.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    public bool RemoveFilter(RP1210_FilterIso15765 filter)
    {
      for (int index = 0; index < this.m_Iso15765.Count; ++index)
      {
        if (this.m_Iso15765[index] == filter)
        {
          this.m_Iso15765.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    public int FilterCountCan => this.m_Can.Count;

    public int FilterCountJ1939 => this.m_J1939.Count;

    public int FilterCountIso15765 => this.m_Iso15765.Count;

    public RP1210_FilterCan[] CanFilters => this.m_Can.ToArray();

    public RP1210_FilterJ1939[] J1939Filters => this.m_J1939.ToArray();

    public RP1210_FilterIso15765[] Iso15765Filters => this.m_Iso15765.ToArray();
  }
}
