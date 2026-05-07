using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

/// <summary>
/// Summary description for NextQueueInfo
/// </summary>
/// 
[Serializable]
public class NextQueueItem
{
    public string MerchantNumber { get; set; }
    //42598 + Enhance code
    public DataTable[] Data { get; set; }

    public DataTable MerchantInfo
    {
        get
        {
            if (Data != null && Data.Length > 0)
                return Data[0];
            else
                return null;
        }
    }
    public DataTable Barometer
    {
        get
        {
            if (Data != null && Data.Length > 1)
                return Data[1];
            else
                return null;
        }
    }
    public DataTable Transaction
    {
        get
        {
            if (Data != null && Data.Length > 2)
                return Data[2];
            else
                return null;
        }
    }

    public DataTable ChargeBack
    {
        get
        {
            if (Data != null && Data.Length > 3)
                return Data[3];
            else
                return null;
        }
    }
}

[Serializable]
public class NextQueueItemCollection : Collection<NextQueueItem>
{
    public NextQueueItemCollection()
        : base()
    {
    }
    public NextQueueItem Get(int index, bool isMCF = false)
    {
        if (this.Items.Count - 1 >= index)
        {
            NextQueueItem item = this.Items[index];
            Remove(index);
            if (isMCF)
            {
                NextQueueItemCollection mcfList = new NextQueueItemCollection();
                foreach (var itemReal in this.Items)
                {
                    mcfList.Add(new NextQueueItem()
                    {
                        MerchantNumber = itemReal.MerchantNumber,
                        Data = itemReal.Data
                    });
                }
                RiskSessionManager.MCF_RiskNextQueue_Cache = mcfList;
            }
            return item;
        }
        return new NextQueueItem();
    }

    private void Remove(int index)
    {
        this.Items.RemoveAt(index);
    }
}
