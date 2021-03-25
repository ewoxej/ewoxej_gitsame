using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GitApi
{
    [DataContract]
    public class Branch
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string sha { get; set; }
        [DataMember]
        public string node_id { get; set; }
        [DataMember]
        public Commit commit { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string html_url { get; set; }
        [DataMember]
        public string comments_url { get; set; }
        [DataMember]
        public User author { get; set; }
        [DataMember]
        public User committer { get; set; }
        [DataMember]
        public List<Commit> parents { get; set; }
        [DataMember]
        public Dictionary<string, int> stats { get; set; }
        [DataMember(Name = "protected")]
        public bool isProtected { get; set; }
    }
}
