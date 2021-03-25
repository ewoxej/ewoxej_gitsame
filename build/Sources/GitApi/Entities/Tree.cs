using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


namespace GitApi
{

[DataContract]
    public class TreeNode
    {
        [DataMember]
        public string sha { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string html_url { get; set; }
        [DataMember]
        public string git_url { get; set; }
        [DataMember]
        public string download_url { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string path { get; set; }
        [DataMember]
        public string mode { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public int size { get; set; }
    }

    [DataContract]
    public class Blob
    {
        [DataMember]
        public string sha { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string node_id { get; set; }
        [DataMember]
        public string encoding { get; set; }
        [DataMember]
        public int size { get; set; }
        [DataMember]
        public string content { get; set; }
    }
    [DataContract]
    public class Tree
    {
        [DataMember]
        public string sha { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public List<TreeNode> tree { get; set; }
    }
}
