using System;

namespace AS.VW.Entities.PauseMerchantAlert
{
    [Serializable]
    public class TrackingMerchantIdsSelectedModel
    {
        public string RowGuid { get; set; }
        public string MerchantIds { get; set; }
    }
}
