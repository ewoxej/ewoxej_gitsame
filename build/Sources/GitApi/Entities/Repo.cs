using System.Runtime.Serialization;


namespace GitApi
{
    [DataContract]
    public class Repo
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string node_id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string full_name { get; set; }
        [DataMember(Name = "private")]
        public bool isPrivate { get; set; }
        [DataMember]
        public User owner { get; set; }
        [DataMember]
        public string html_url { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public bool fork { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string forks_url { get; set; }
        [DataMember]
        public string keys_url { get; set; }
        [DataMember]
        public string collaborators_url { get; set; }
        [DataMember]
        public string teams_url { get; set; }
        [DataMember]
        public string hooks_url { get; set; }
        [DataMember]
        public string issue_events_url { get; set; }
        [DataMember]
        public string events_url { get; set; }
        [DataMember]
        public string assignees_url { get; set; }
        [DataMember]
        public string branches_url { get; set; }
        [DataMember]
        public string tags_url { get; set; }
        [DataMember]
        public string blobs_url { get; set; }
        [DataMember]
        public string git_tags_url { get; set; }
        [DataMember]
        public string git_refs_url { get; set; }
        [DataMember]
        public string trees_url { get; set; }
        [DataMember]
        public string statuses_url { get; set; }
        [DataMember]
        public string languages_url { get; set; }
        [DataMember]
        public string stargazers_url { get; set; }
        [DataMember]
        public string contributors_url { get; set; }
        [DataMember]
        public string subscribers_url { get; set; }
        [DataMember]
        public string subscription_url { get; set; }
        [DataMember]
        public string commits_url { get; set; }
        [DataMember]
        public string git_commits_url { get; set; }
        [DataMember]
        public string comments_url { get; set; }
        [DataMember]
        public string issue_comment_url { get; set; }
        [DataMember]
        public string contents_url { get; set; }
        [DataMember]
        public string compare_url { get; set; }
        [DataMember]
        public string merges_url { get; set; }
        [DataMember]
        public string archive_url { get; set; }
        [DataMember]
        public string downloads_url { get; set; }
        [DataMember]
        public string issues_url { get; set; }
        [DataMember]
        public string pulls_url { get; set; }
        [DataMember]
        public string milestones_url { get; set; }
        [DataMember]
        public string notifications_url { get; set; }
        [DataMember]
        public string labels_url { get; set; }
        [DataMember]
        public string releases_url { get; set; }
        [DataMember]
        public string deployments_url { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public string pushed_at { get; set; }
        [DataMember]
        public string git_url { get; set; }
        [DataMember]
        public string ssh_url { get; set; }
        [DataMember]
        public string clone_url { get; set; }
        [DataMember]
        public string svn_url { get; set; }
        [DataMember]
        public string homepage { get; set; }
        [DataMember]
        public int size { get; set; }
        [DataMember]
        public string stargazers_count { get; set; }
        [DataMember]
        public string watchers_count { get; set; }
        [DataMember]
        public string language { get; set; }
        [DataMember]
        public bool has_issues { get; set; }
        [DataMember]
        public bool has_projects { get; set; }
        [DataMember]
        public bool has_downloads { get; set; }
        [DataMember]
        public bool has_wiki { get; set; }
        [DataMember]
        public bool has_pages { get; set; }
        [DataMember]
        public int forks_count { get; set; }
        [DataMember]
        public string mirror_url { get; set; }
        [DataMember]
        public bool archived { get; set; }
        [DataMember]
        public bool disabled { get; set; }
        [DataMember]
        public int open_issues_count { get; set; }
        [DataMember]
        public string license { get; set; }
        [DataMember]
        public int forks { get; set; }
        [DataMember]
        public int open_issues { get; set; }
        [DataMember]
        public int watchers { get; set; }
        [DataMember]
        public string default_branch { get; set; }
    }
}
