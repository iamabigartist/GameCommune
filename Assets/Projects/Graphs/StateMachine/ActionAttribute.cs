using System;
namespace Graphs.StateMachine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ActionAttribute : Attribute
    {
        readonly string m_path;
        readonly string m_info;

        public string Path => m_path;
        public string Info => m_info;

        public ActionAttribute(string path, string info)
        {
            m_path = path;
            m_info = info;
        }

        public ActionAttribute(string path)
        {
            m_path = path;
            m_info = null;
        }
    }
}
