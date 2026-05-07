using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[Serializable]
public class RiskAutoQueueModel
{
	public RiskAutoQueueModel()
	{
		
	}

    public long ID { get; set; }
    public long Index { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Assignments { get; set; }
    public string lastRun { get; set; }
    public string lastRunDts { get; set; }
    public string lastRunText { get; set; }
    public string CreateOn { get; set; }
    public string WorkQueue { get; set; }    
    public Boolean IsActive { get; set; }
}

//public class RiskAutoQueueDetailModel
//{
//    public RiskAutoQueueDetailModel()
//    {
//        Assignments=new List<string>();
//        WorkQueue = new List<string>();
//    }

//    public string ID { get; set; }
//    public string Name { get; set; }
//    public string Description { get; set; }
//    public List<string> Assignments { get; set; }
//    public List<string> WorkQueue { get; set; }
//}

//public class RiskbaseModel
//{
//    public string ID { get; set; }
//    public long Name { get; set; }
//}