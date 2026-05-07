using System.Data;

namespace AS.VW.Scheduler.Tasks
{
    /// <summary>
    /// IExtension
    /// </summary>
    public interface IExtension
    {
        object Excute(object inputData);
    }
}
