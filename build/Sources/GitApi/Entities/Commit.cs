using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GitApi
{
    [DataContract]
    public class CommitVerification
    {
        [DataMember]
        public bool verified { get; set; }
        [DataMember]
        public string reason { get; set; }
        [DataMember]
        public string signature { get; set; }
        [DataMember]
        public string payload { get; set; }
    }

    [DataContract]
    public class CommitData
    {
        [DataMember]
        public Commiter author { get; set; }
        [DataMember]
        public Commiter commiter { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public TreeNode tree { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public int comment_count { get; set; }
        [DataMember]
        public CommitVerification verification { get; set; }

    }

    [DataContract]
    public class Commit
    {
        [DataMember]
        public string sha { get; set; }
        [DataMember]
        public string node_id { get; set; }
        [DataMember]
        public CommitData commit { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string html_url { get; set; }
        [DataMember]
        public string comments_url { get; set; }
        [DataMember]
        public User author { get; set; }
        [DataMember]
        public User commiter { get; set; }
        [DataMember]
        public List<Commit> parents { get; set; }
        [DataMember]
        public Tree tree { get; set; }
    }

    [DataContract]
    public class Commiter
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string date { get; set; }
    }
}
