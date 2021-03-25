﻿using System.Runtime.Serialization;

namespace GitApi
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string login { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string node_id { get; set; }
        [DataMember]
        public string avatar_url { get; set; }
        [DataMember]
        public string gravatar_id { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string html_url { get; set; }
        [DataMember]
        public string followers_url { get; set; }
        [DataMember]
        public string following_url { get; set; }
        [DataMember]
        public string gists_url { get; set; }
        [DataMember]
        public string starred_url { get; set; }
        [DataMember]
        public string subscriptions_url { get; set; }
        [DataMember]
        public string organizations_url { get; set; }
        [DataMember]
        public string repos_url { get; set; }
        [DataMember]
        public string events_url { get; set; }
        [DataMember]
        public string received_events_url { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string site_admin { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string blog { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string hireable { get; set; }
        [DataMember]
        public string bio { get; set; }
        [DataMember]
        public int public_repos { get; set; }
        [DataMember]
        public int public_gists { get; set; }
        [DataMember]
        public int followers { get; set; }
        [DataMember]
        public int following { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public string date { get; set; }
    }
}
