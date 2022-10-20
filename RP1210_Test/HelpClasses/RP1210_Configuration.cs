
// Type: RP1210_Test.HelpClasses.RP1210_Configuration




using System.Collections.Generic;

namespace RP1210_Test.HelpClasses
{
  public class RP1210_Configuration
  {
    private static int m_MaxBroadcastCount = 16;
    private RP1210_FilterManager m_FilterMng;
    private List<RP1210_BroadcastMsg> m_Broadcast;

    public RP1210_Configuration()
    {
      this.m_FilterMng = new RP1210_FilterManager();
      this.m_Broadcast = new List<RP1210_BroadcastMsg>();
      this.InterpacketTime = 50U;
      this.ReceivingMessages = true;
      this.ReceivingEcho = false;
      this.FilterStatus = RP1210C_FilterStatus.Close;
      this.FilterType = RP1210C_FilterType.Inclusive;
    }

    public RP1210_Configuration(RP1210_Configuration configuration)
    {
      this.m_FilterMng = new RP1210_FilterManager(configuration.m_FilterMng);
      this.m_Broadcast = new List<RP1210_BroadcastMsg>((IEnumerable<RP1210_BroadcastMsg>) configuration.m_Broadcast);
      this.InterpacketTime = configuration.InterpacketTime;
      this.ReceivingMessages = configuration.ReceivingMessages;
      this.ReceivingEcho = configuration.ReceivingEcho;
      this.FilterStatus = configuration.FilterStatus;
      this.FilterType = configuration.FilterType;
    }

    public void ClearCanFilters() => this.m_FilterMng.ClearFilterCan();

    public void ClearJ1939Filters() => this.m_FilterMng.ClearFilterJ1939();

    public void ClearIso15765Filters() => this.m_FilterMng.ClearFilterIso15765();

    public bool AddFilter(RP1210_FilterCan filter) => this.m_FilterMng.AddFilter(filter);

    public bool AddFilter(RP1210_FilterJ1939 filter) => this.m_FilterMng.AddFilter(filter);

    public bool AddFilter(RP1210_FilterIso15765 filter) => this.m_FilterMng.AddFilter(filter);

    public bool RemoveFilter(RP1210_FilterCan filter) => this.m_FilterMng.RemoveFilter(filter);

    public bool RemoveFilter(RP1210_FilterJ1939 filter) => this.m_FilterMng.RemoveFilter(filter);

    public bool RemoveFilter(RP1210_FilterIso15765 filter) => this.m_FilterMng.RemoveFilter(filter);

    public void ClearBroadcastMessages() => this.m_Broadcast.Clear();

    public bool AddBroadcastMessage(RP1210_BroadcastMsg msg)
    {
      if (this.m_Broadcast.Count >= RP1210_Configuration.m_MaxBroadcastCount || msg == (RP1210_BroadcastMsg) null || this.m_Broadcast.Contains(msg))
        return false;
      this.m_Broadcast.Add(msg);
      return true;
    }

    public bool RemoveBroadcastMessage(RP1210_BroadcastMsg msg) => this.m_Broadcast.Remove(msg);

    public uint InterpacketTime { get; set; }

    public bool ReceivingMessages { get; set; }

    public bool ReceivingEcho { get; set; }

    public RP1210C_FilterStatus FilterStatus { get; set; }

    public RP1210C_FilterType FilterType { get; set; }

    public RP1210_FilterCan[] CanFilters => this.m_FilterMng.CanFilters;

    public RP1210_FilterJ1939[] J1939Filters => this.m_FilterMng.J1939Filters;

    public RP1210_FilterIso15765[] Iso15765Filters => this.m_FilterMng.Iso15765Filters;

    public RP1210_BroadcastMsg[] BroadcastMessages => this.m_Broadcast.ToArray();

    public int BroadcastMessagesCount => this.m_Broadcast.Count;

    public static int MaximumCountBroadcastMessages => RP1210_Configuration.m_MaxBroadcastCount;
  }
}
